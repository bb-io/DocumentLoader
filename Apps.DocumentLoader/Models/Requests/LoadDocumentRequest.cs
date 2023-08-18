namespace Apps.DocumentLoader.Models.Requests;

public class LoadDocumentRequest
{
    public string Filename { get; set; }
    public byte[] File { get; set; }
}