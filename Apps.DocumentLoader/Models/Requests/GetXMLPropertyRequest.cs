using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.DocumentLoader.Models.Requests;

public class GetXMLPropertyRequest
{
    public FileReference File { get; set; }

    public string Property { get; set; }
}