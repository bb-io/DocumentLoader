using System.Net.Mime;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Apps.DocumentLoader.Models.Requests;
using Apps.DocumentLoader.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;

namespace Apps.DocumentLoader;

[ActionList]
public class Actions
{
    private readonly IFileManagementClient _fileManagementClient;

    public Actions(IFileManagementClient fileManagementClient)
    {
        _fileManagementClient = fileManagementClient;
    }

    [Action("Load document", Description = "Load document's text. Document must be in docx/doc, pdf or txt format.")]
    public async Task<LoadDocumentResponse> LoadDocument([ActionParameter] LoadDocumentRequest request)
    {
        var file = await _fileManagementClient.DownloadAsync(request.File);
        var text = await DocumentReader.ReadDocument(await file.GetByteData(), request.File.Name);

        return new() { Text = text };
    }

    [Action("Split document into chunks", Description = "Split loaded document into chunks.")]
    public SplitDocumentResponse SplitDocument([ActionParameter] SplitDocumentRequest request)
    {
        var textSplitter = new TextSplitter(maximumChunkSize: request.MaximumChunkSize ?? 4000,
            maximumChunkOverlap: request.MaximumChunkOverlap ?? 200);
        var chunks = textSplitter.SplitText(request.Document);
        return new SplitDocumentResponse { Chunks = chunks };
    }

    [Action("Convert text to txt document", Description = "Convert text to txt document.")]
    public async Task<ConvertTextToDocumentResponse> ConvertTextToTxtDocument(
        [ActionParameter] ConvertTextToDocumentRequest request)
    {
        const string txtExtension = ".txt";

        var bytes = Encoding.UTF8.GetBytes(request.Text);
        var filename = request.Filename;

        if (!Path.HasExtension(filename))
            filename += txtExtension;
        else
        {
            var extension = Path.GetExtension(request.Filename);
            if (extension != txtExtension)
                throw new Exception("Can convert to txt file only. Please specify filename with .txt extension or " +
                                    "don't specify extension at all.");
        }

        var file = await _fileManagementClient.UploadAsync(new MemoryStream(bytes), MediaTypeNames.Application.Octet,
            filename);
        return new ConvertTextToDocumentResponse
        {
            File = file
        };
    }

    [Action("Change XML file property", Description = "Change XML file property")]
    public async Task<ConvertTextToDocumentResponse> ChangeXML([ActionParameter] ChangeXMLRequest request)
    {
        await using var streamIn = await _fileManagementClient.DownloadAsync(request.File);
        var doc = XDocument.Load(streamIn);
        var items = doc.Root.Descendants(request.Property);

        foreach (var itemElement in items)
            itemElement.Value = request.Value;

        await using var streamOut = new MemoryStream();
        var settings = new XmlWriterSettings();
        settings.OmitXmlDeclaration = true;
        settings.Indent = true;
        var writer = XmlWriter.Create(streamOut, settings);
        doc.Save(writer);
        await writer.FlushAsync();

        var resultFile =
            await _fileManagementClient.UploadAsync(streamOut, request.File.ContentType, request.File.Name);
        return new ConvertTextToDocumentResponse
        {
            File = resultFile
        };
    }

    [Action("Get XML file property", Description = "Get XML file property")]
    public async Task<GetXMLPropertyResponse> GetXMLProperty([ActionParameter] GetXMLPropertyRequest request)
    {
        await using var streamIn = await _fileManagementClient.DownloadAsync(request.File);

        var doc = XDocument.Load(streamIn);
        var items = doc.Root.Descendants(request.Property);
        return new() { Value = items.First().Value };
    }

    [Action("Bump version string", Description = "Bump version string")]
    public async Task<GetXMLPropertyResponse> BumpVersionString([ActionParameter] BumpVersionStringRequest request)
    {
        Version version = Version.Parse(request.VersionString);
        int major = request.VersionType == "major" ? version.Major + 1 : version.Major;
        int minor = request.VersionType == "minor" ? version.Minor + 1 : version.Minor;
        int patch = request.VersionType == "patch" ? version.Build + 1 : version.Build;
        return new GetXMLPropertyResponse() { Value = $"{major}.{minor}.{patch}" };
    }
}