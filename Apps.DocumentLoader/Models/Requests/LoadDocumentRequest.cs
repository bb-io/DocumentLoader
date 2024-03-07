using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.DocumentLoader.Models.Requests;

public class LoadDocumentRequest
{
    public FileReference File { get; set; }
}