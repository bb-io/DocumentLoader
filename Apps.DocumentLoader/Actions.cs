using System.Net.Mime;
using System.Text;
using Apps.DocumentLoader.Models.Requests;
using Apps.DocumentLoader.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.DocumentLoader;

[ActionList]
public class Actions
{
    [Action("Load document", Description = "Load document's text. Document must be in docx/doc, pdf or txt format.")]
    public async Task<LoadDocumentResponse> LoadDocument([ActionParameter] LoadDocumentRequest request)
    {
        var text = await DocumentReader.ReadDocument(request.File.Bytes, request.File.Name);
        return new LoadDocumentResponse { Text = text };
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
    public ConvertTextToDocumentResponse ConvertTextToTxtDocument([ActionParameter] ConvertTextToDocumentRequest request)
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

        return new ConvertTextToDocumentResponse
        {
            File = new File(bytes)
            {
                Name = filename,
                ContentType = MediaTypeNames.Application.Octet
            }
        };
    }
}