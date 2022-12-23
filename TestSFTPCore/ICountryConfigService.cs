using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace TestSFTPCore
{
    interface ICountryConfigService
    {
        Country[] GetCountries();
    }
    public class CountryConfigService : ICountryConfigService
    {
        public Country[] GetCountries()
        {
            FileStream myFileStream;
            XmlSerializer ser;
            try
            { 
            ser = new XmlSerializer(typeof(DIContainer));
            myFileStream = new FileStream(@"CountryConfig.xml", FileMode.Open);
            
                return ((DIContainer)ser.Deserialize(myFileStream)).Countries;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                ser = null;
                myFileStream = null;
            }
            return null;
        }

       // private readonly IHostingEnvironment _env;
        
    }
}
