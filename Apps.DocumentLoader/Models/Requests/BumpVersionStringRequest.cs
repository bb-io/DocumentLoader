using Apps.DocumentLoader.DataSourceHandler;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.DocumentLoader.Models.Requests
{
    public class BumpVersionStringRequest
    {
        [Display("Version string")]
        public string VersionString { get; set; }

        [Display("Version type")]
        [DataSource(typeof(VersionTypeDataHandler))]
        public string VersionType { get; set; }
    }
}
