using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.DocumentLoader.Models.Requests
{
    public class ChangeXMLRequest
    {
        public File File { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }
    }
}
