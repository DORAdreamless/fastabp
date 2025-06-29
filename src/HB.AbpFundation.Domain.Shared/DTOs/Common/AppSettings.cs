using System.Collections.Generic;

namespace HB.AbpFundation.DTOs.Common
{
    public class AppSettings
    {
        public List<string> Assemblies { get; set; }

        public string OutputDirectory { get; set; }

        public string Templates { get; set; }
    }

}
