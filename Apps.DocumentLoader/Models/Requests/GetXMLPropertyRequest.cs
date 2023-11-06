using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.DocumentLoader.Models.Requests
{
    public class GetXMLPropertyRequest
    {
        public File File { get; set; }

        public string Property { get; set; }
    }
}
