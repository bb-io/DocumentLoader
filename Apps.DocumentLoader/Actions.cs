using Apps.DocumentLoader.Models.Requests;
using Apps.DocumentLoader.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;

namespace Apps.DocumentLoader;

[ActionList]
public class Actions
{
    [Action("Load document", Description = "Load document's text. Document must be in docx/doc, pdf or txt format.")]
    public async Task<LoadDocumentResponse> LoadDocument([ActionParameter] LoadDocumentRequest request)
    {
        var text = await DocumentReader.ReadDocument(request.File, request.Filename);
        return new LoadDocumentResponse { Text = text };
    }

    [Action("Split document into chunks", Description = "Split loaded document into chunks.")]
    public async Task<SplitDocumentResponse> SplitDocument([ActionParameter] SplitDocumentRequest request)
    {
        var textSplitter = new TextSplitter(maximumChunkSize: request.MaximumChunkSize ?? 4000, 
            maximumChunkOverlap: request.MaximumChunkOverlap ?? 200);
        var chunks = textSplitter.SplitText(request.Document);
        return new SplitDocumentResponse { Chunks = chunks };
    }
}