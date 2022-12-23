using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace TestSFTPCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("POC To test SFTP !");
            var sFTPCOnfig = new SFTPConfig();

            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false);

            IConfiguration config = builder.Build();
            
            sFTPCOnfig.Host = config.GetSection("Downloading").GetSection("SFTP").GetSection("Host").Value;
            sFTPCOnfig.Port = Convert.ToInt32(config.GetSection("Downloading").GetSection("SFTP").GetSection("Port").Value);
            sFTPCOnfig.UserName = config.GetSection("Downloading").GetSection("SFTP").GetSection("UserName").Value;
            sFTPCOnfig.Password = config.GetSection("Downloading").GetSection("SFTP").GetSection("Password").Value;
            var Path = config.GetSection("Downloading").GetSection("CountryConfig").GetSection("Path").Value;

            ICountryConfigService di = new CountryConfigService();
            var countryConfig = di.GetCountries();

            SFTPClient sFTP = new SFTPClient(sFTPCOnfig);
            sFTP.CanConnect();
            Console.WriteLine("SFTP COnnected successfully");
            sFTP.Download(@"/C:/Gopi/sftptest.txt", @"C:\Vijay\sftptest.txt");


            Console.ReadLine();
        }
    }
}
