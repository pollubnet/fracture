using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fracture.Shared.External.Providers.Ai
{
    public class PromptTemplate
    {
        public required string System { get; set; }

        public required string Input { get; set; }

        public required string Response { get; set; }

        public string? ResponseParams { get; set; }
    }
}
