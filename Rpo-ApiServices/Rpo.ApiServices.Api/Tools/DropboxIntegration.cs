
namespace Rpo.ApiServices.Api.Tools
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Dropbox.Api;
    using Dropbox.Api.Files;
    using Dropbox.Api.Common;
    using System.Collections.Generic;
    using System.Net;

    public class DropboxIntegration
    {
        public async Task<int> RunUpload(string uploadFilePath, string dropBoxFileName, string filepath)
        {
            var accessToken = Properties.Settings.Default.DropboxAccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                return 1;
            }

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            try
            {
                var config = new DropboxClientConfig("RPO CORS")
                {
                    HttpClient = httpClient
                };

                var client = new DropboxClient(accessToken, config);
                if (!string.IsNullOrEmpty(Properties.Settings.Default.DropboxNamespaceId))
                {
                    client = client.WithPathRoot(new PathRoot.Root(Properties.Settings.Default.DropboxNamespaceId));
                }
                await Upload(client, uploadFilePath, dropBoxFileName, filepath);
            }
            catch (HttpException e)
            {
            }

            return 0;
        }

        //private async Task RunUserTests(DropboxClient client)
        //{
        //    var path = "/JobDocuments/PrajeshTest";
        //    await Upload(client, path, "B01-2016356-A22.pdf", "This is a text file");
        //    //await ChunkUpload(client, path, "Binary");
        //    //var folder = await CreateFolder(client, path);
        //    //var folder = await ListFolder(client, path);

        //}

        private async Task Upload(DropboxClient client, string uploadFilePath, string fileName, string filepath)
        {
            try
            {
                client.Files.GetMetadataAsync(uploadFilePath);
            }
            catch (Exception e)
            {
                client.Files.CreateFolderAsync(uploadFilePath);
            }

            using (var stream = new MemoryStream(File.ReadAllBytes(filepath)))
            {
                var response = client.Files.UploadAsync(uploadFilePath + "/" + fileName, WriteMode.Overwrite.Instance, body: stream);
                var rest = response.Result;
            }
        }

        public async Task<int> RunDownload(string source, string destination)
        {
            var accessToken = Properties.Settings.Default.DropboxAccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                return 1;
            }

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            try
            {
                var config = new DropboxClientConfig("RPO CORS")
                {
                    HttpClient = httpClient
                };
                var client = new DropboxClient(accessToken, config);
                if (!string.IsNullOrEmpty(Properties.Settings.Default.DropboxNamespaceId))
                {
                    client = client.WithPathRoot(new PathRoot.Root(Properties.Settings.Default.DropboxNamespaceId));
                }
                Download(client, source, destination);

            }
            catch (HttpException e)
            {
            }

            return 0;

        }

        public async Task<int> RunDownloadTransmittal(string source, string destination)
        {
            var accessToken = Properties.Settings.Default.DropboxAccessToken;

            if (string.IsNullOrEmpty(accessToken))
            {
                return 1;
            }

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            try
            {
                var config = new DropboxClientConfig("RPO CORS")
                {
                    HttpClient = httpClient
                };

                var client = new DropboxClient(accessToken, config);

                if (!string.IsNullOrEmpty(Properties.Settings.Default.DropboxNamespaceId))
                {

                    client = client.WithPathRoot(new PathRoot.Root(Properties.Settings.Default.DropboxNamespaceId));
                }

                bool status = DownloadTransmittal(client, source, destination);

                if (status == false)
                {
                    WriteLogWebclient("Downloading Fail.");
                    return 1;
                }
            }
            catch (HttpException e)
            {
                throw e;
            }
            return 0;
        }

        //private async Task Download(DropboxClient client, string source, string destination)
        //{
        //    using (var response = await client.Files.DownloadAsync(source))
        //    {
        //        Stream input = await response.GetContentAsStreamAsync();

        //        using (Stream file = File.OpenWrite(destination))
        //        {
        //            input.CopyTo(file);
        //            file.Close();
        //        }
        //    }
        //}

        public bool Download(DropboxClient client, string source, string destination)
        {
            try
            {
                var response = client.Files.DownloadAsync(source);
                var result = response.Result.GetContentAsStreamAsync(); //Added to wait for the result from Async method  

                using (Stream file = File.OpenWrite(destination))
                {
                    result.Result.CopyTo(file);
                    file.Close();
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }


        public bool DownloadTransmittal(DropboxClient client, string source, string destination)
        {
            try
            {
                WriteLogWebclient("Start New file-" + source + "--" + DateTime.Now.ToString());

                var response = client.Files.DownloadAsync(source);

                Downloadcontent:

                var result = response.Result.GetContentAsStreamAsync(); //Added to wait for the result from Async method  
                if (result.Status == TaskStatus.RanToCompletion)
                {
                    WriteLogWebclient("RanTOComplete New file-" + source + "--" + DateTime.Now.ToString());
                    using (Stream file = File.OpenWrite(destination))
                    {
                        WriteLogWebclient("Write content into Destination file-" + destination + "--" + DateTime.Now.ToString());
                        result.Result.CopyTo(file);
                        file.Close();
                        WriteLogWebclient("End New file-" + source + "--" + DateTime.Now.ToString());
                    }
                }
                else
                {
                    WriteLogWebclient("In Progress-" + source + "--" + DateTime.Now.ToString());
                    goto Downloadcontent;
                }

                WriteLogWebclient("Donwload done-" + source + "--" + DateTime.Now.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
                return false;
            }
        }

        //public async bool DownloadTransmittal(DropboxClient client, string source, string destination)
        //{
        //    bool status = false;
        //    try
        //    {
        //        using (var response = await client.Files.DownloadAsync(source))
        //        {
        //            using (var fileStream = File.OpenWrite(destination))
        //            {
        //                (await response.GetContentAsStreamAsync()).CopyTo(fileStream);
        //                fileStream.Close();
        //            }
        //        }
        //        status=true;
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return status;
        //}


        public async Task<int> RunDelete(string source)
        {
            var accessToken = Properties.Settings.Default.DropboxAccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                return 1;
            }

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            try
            {
                var config = new DropboxClientConfig("RPO CORS")
                {
                    HttpClient = httpClient
                };

                var client = new DropboxClient(accessToken, config);
                Delete(client, source);
            }
            catch (HttpException e)
            {
            }

            return 0;
        }
        public bool Delete(DropboxClient client, string source)
        {
            try
            {
                var response = client.Files.DeleteAsync(source);
                var rest = response.Result;

                //var result = response.Result.GetContentAsStreamAsync(); //Added to wait for the result from Async method  
                //using (Stream file = File.OpenWrite(destination))
                //{
                //    result.Result.CopyTo(file);
                //    file.Close();
                //}

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public static void WriteLogWebclient(string message)
        {
            string errorLogFilename = "Dropbox_" + DateTime.Now.ToString("dd-MM-yyyy") + ".txt";

            string directory = AppDomain.CurrentDomain.BaseDirectory + "Log";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string path = AppDomain.CurrentDomain.BaseDirectory + "Log/" + errorLogFilename;

            if (File.Exists(path))
            {
                using (StreamWriter stwriter = new StreamWriter(path, true))
                {
                    stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                    stwriter.WriteLine("Message: " + message);
                    stwriter.WriteLine("-------------------End----------------------------");
                    stwriter.Close();
                }
            }
            else
            {
                StreamWriter stwriter = File.CreateText(path);
                stwriter.WriteLine("-------------------Web Client Log Start-----------as on " + DateTime.Now.ToString("hh:mm tt"));
                stwriter.WriteLine("Message: " + message);
                stwriter.WriteLine("-------------------End----------------------------");
                stwriter.Close();
            }


            var attachments = new List<string>();
            if (File.Exists(path))
            {
                attachments.Add(path);
            }

            var to = new List<KeyValuePair<string, string>>();

            to.Add(new KeyValuePair<string, string>("rposupportgroup@credencys.com", "RPO Team"));

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplate/MasterEmailTemplate.htm")))
            {
                body = reader.ReadToEnd();
            }

            var cc = new List<KeyValuePair<string, string>>();

            string emailBody = body;
            emailBody = emailBody.Replace("##EmailBody##", "Please correct the issue.");

            //Mail.Send(
            //           new KeyValuePair<string, string>("noreply@rpoinc.com", "RPO Log"),
            //           to,
            //           cc,
            //           "DropBox Log",
            //           emailBody,
            //           attachments
            //       );
        }


        public async Task<int> RunCopy(string FromFile, string ToFile)
        {
            var accessToken = Properties.Settings.Default.DropboxAccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                return 1;
            }

            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };

            try
            {
                var config = new DropboxClientConfig("RPO CORS")
                {
                    HttpClient = httpClient
                };

                var client = new DropboxClient(accessToken, config);
                if (!string.IsNullOrEmpty(Properties.Settings.Default.DropboxNamespaceId))
                {
                    client = client.WithPathRoot(new PathRoot.Root(Properties.Settings.Default.DropboxNamespaceId));
                }
                await CopyFiles(client, FromFile, ToFile);
            }
            catch (HttpException e)
            {
            }

            return 0;
        }

        public async Task CopyFiles(DropboxClient client, string from_path, string to_path)
        {
            try
            {
                client.Files.BeginCopyV2(from_path, to_path, false, false, false);

            }
            catch (Exception ex)
            {
                client.Files.BeginCopyV2(from_path, to_path, false, false, false);
            }
        }


        //public bool CopyFiles(DropboxClient client, string from_path, string to_path)
        //{
        //    try
        //    {
        //        var response = client.Files.BeginCopyV2(from_path, to_path, false, false, false);
        //        response.
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public bool ReNameFiles(DropboxClient client, string from_path, string to_path)
        {
            try
            {
                var response = client.Files.BeginCopyV2(from_path, to_path, false, false, false);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public void RunCreateFolder(string uploadFilePath)
        {
            var accessToken = Properties.Settings.Default.DropboxAccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                //  return 1;
            }
            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };
            try
            {
                var config = new DropboxClientConfig("RPO CORS")
                {
                    HttpClient = httpClient
                };
                var client = new DropboxClient(accessToken, config);
                if (!string.IsNullOrEmpty(Properties.Settings.Default.DropboxNamespaceId))
                {
                    client = client.WithPathRoot(new PathRoot.Root(Properties.Settings.Default.DropboxNamespaceId));
                }
                CreateFolder2(client, uploadFilePath);
            }
            catch (HttpException e)
            {
            }
            // return 0;
        }
        //private async Task<FolderMetadata> CreateFolder(DropboxClient client, string uploadFilePath)
        //{
        //    Console.WriteLine("--- Creating Folder ---");
        //    var folderArg = new CreateFolderArg(uploadFilePath);
        //    var folder = await client.Files.CreateFolderV2Async(folderArg);
        //    Console.WriteLine("Folder: " + uploadFilePath + " created!");
        //    return folder.Metadata;
        //}
        private void CreateFolder2(DropboxClient client, string uploadFilePath)
        {
            Console.WriteLine("--- Creating Folder ---");
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                               | SecurityProtocolType.Tls11
                               | SecurityProtocolType.Tls12;
            var folderArg = new CreateFolderArg(uploadFilePath);
            var folder = client.Files.CreateFolderV2Async(folderArg).Result;
            Console.WriteLine("Folder: " + uploadFilePath + " created!");
            //return folder.Result;
        }
        string isFolder2;
        public string RunFolderExists(string uploadFilePath)
        {          
            var accessToken = Properties.Settings.Default.DropboxAccessToken;
            if (string.IsNullOrEmpty(accessToken))
            {
                return "1";
            }
            var httpClient = new HttpClient(new WebRequestHandler { ReadWriteTimeout = 10 * 1000 })
            {
                Timeout = TimeSpan.FromMinutes(20)
            };
            try
            {
                var config = new DropboxClientConfig("RPO CORS")
                {
                    HttpClient = httpClient
                };
                var client = new DropboxClient(accessToken, config);
                if (!string.IsNullOrEmpty(Properties.Settings.Default.DropboxNamespaceId))
                {
                    client = client.WithPathRoot(new PathRoot.Root(Properties.Settings.Default.DropboxNamespaceId));
                }
                isFolder2 = FolderExists(client, uploadFilePath);
                if (isFolder2 == "0")
                {
                    isFolder2 = "0";
                }
                else
                { isFolder2 = "1"; }
            }
            catch (HttpException e)
            {
            }
            return isFolder2;
        }
        string isFolder;
        public string  FolderExists(DropboxClient client, string path)
        {            
            try
            {
              var  isFolderExists = client.Files.GetMetadataAsync(path).Result.IsFolder;
                if (isFolderExists == true)
                {
                    isFolder = "0";
                }
            }
            catch (Exception e)
            {
                isFolder = "1";
            }
            return isFolder;
        }
    }
}