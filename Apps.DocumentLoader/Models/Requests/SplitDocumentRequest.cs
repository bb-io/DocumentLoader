using Blackbird.Applications.Sdk.Common;

namespace Apps.DocumentLoader.Models.Requests;

public class SplitDocumentRequest
{
    public string Document { get; set; }
    
    [Display("Max size of chunks in characters")]
    public int? MaximumChunkSize { get; set; }
    
    [Display("Max overlap in characters between chunks")]
    public int? MaximumChunkOverlap { get; set; }
}