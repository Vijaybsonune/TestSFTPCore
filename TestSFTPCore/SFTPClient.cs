using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace TestSFTPCore
{   
    public class SFTPConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

    }
    public class SFTPClient 
    {
        //private readonly ILogger _logger;

        private readonly SFTPConfig _config;

        public SFTPClient( SFTPConfig sftpConfig)
        {
            //_logger = logger;
            _config = sftpConfig;
        }

        public bool CanConnect()
        {
            string connError = string.Empty;
            try
            {
                using var client = new Renci.SshNet.SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);
                try
                {
                    client.Connect();
                    return true;
                }
                catch (Exception exception)
                {
                    connError = exception.Message;
                   // _logger.LogError(exception, $"Failed to connect");
                    return false;
                }
                finally
                {
                    client.Disconnect();
                }
            }
            catch (Exception exception)
            {
                connError = exception.Message;
                Console.WriteLine(exception.Message);
                //_logger.LogError(exception, $"Failed to connect");
                return false;
            }
        }

        public IEnumerable<string> List(string remoteDirectory = ".")
        {
            using var client = new Renci.SshNet.SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);
            try
            {
                client.Connect();
                return client.ListDirectory(remoteDirectory).Where(f => f.IsRegularFile).OrderBy(f => f.LastWriteTime).Select(f => f.FullName);
            }
            catch (Exception exception)
            {
                //_logger.LogError(exception, $"Failed in listing files under [{remoteDirectory}]");
                return null;
            }
            finally
            {
                client.Disconnect();
            }
        }

        public bool Delete(string remoteFilePath)
        {
            using var client = new Renci.SshNet.SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);
            try
            {
                client.Connect();
                client.DeleteFile(remoteFilePath);
               // _logger.LogInformation($"File [{remoteFilePath}] deleted.");
                return true;
            }
            catch (Exception exception)
            {
               // _logger.LogError(exception, $"Failed in deleting file [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }

            return false;
        }

        public bool Download(string remoteFilePath, string localFilePath)
        {
            using var client = new Renci.SshNet.SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);
            try
            {
                client.Connect();
                using var s = File.Create(localFilePath);
                client.DownloadFile(remoteFilePath, s);
                //_logger.LogInformation($"Finished downloading file [{localFilePath}] from [{remoteFilePath}]");
                return true;
            }
            catch (Exception exception)
            {
               // _logger.LogError(exception, $"Failed in downloading file [{localFilePath}] from [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }
            return false;
        }


        public bool Upload(string localFilePath, string remoteFilePath)
        {
            using var client = new Renci.SshNet.SftpClient(_config.Host, _config.Port == 0 ? 22 : _config.Port, _config.UserName, _config.Password);
            try
            {
                client.Connect();
                using var s = File.OpenRead(localFilePath);
                client.UploadFile(s, remoteFilePath);
                //_logger.LogInformation($"Finished uploading file [{localFilePath}] to [{remoteFilePath}]");
                return true;
            }
            catch (Exception exception)
            {
               // _logger.LogError(exception, $"Failed in uploading file [{localFilePath}] to [{remoteFilePath}]");
            }
            finally
            {
                client.Disconnect();
            }
            return false;
        }

    }
}
