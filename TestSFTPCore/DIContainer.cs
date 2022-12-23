using System;
using System.Collections.Generic;
using System.Text;

namespace TestSFTPCore
{
    public class DIContainer
    {
        public Country[] Countries { get; set; }
    }
    public class Country
    {
        public string LocalFolderPath { get; set; }
        public string FilePrefix { get; set; }
        public string CountryCode { get; set; }
    }
}
