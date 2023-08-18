namespace Apps.DocumentLoader.Models.Responses;

public class SplitDocumentResponse
{
    public IEnumerable<string> Chunks { get; set; }
}