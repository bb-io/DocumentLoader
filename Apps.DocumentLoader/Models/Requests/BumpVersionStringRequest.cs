using Apps.DocumentLoader.DataSourceHandler;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.DocumentLoader.Models.Requests;

public class BumpVersionStringRequest
{
    [Display("Version string")]
    public string VersionString { get; set; }

    [Display("Version type")]
    [DataSource(typeof(VersionTypeDataHandler))]
    public string VersionType { get; set; }
}