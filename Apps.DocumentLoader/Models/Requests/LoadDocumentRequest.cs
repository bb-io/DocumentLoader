using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.DocumentLoader.Models.Requests;

public class LoadDocumentRequest
{
    public File File { get; set; }
}