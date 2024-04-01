

namespace Rpo.Efiling
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Dropbox.Api;
    using Dropbox.Api.Files;

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
                Download(client, source, destination);
            }
            catch (HttpException e)
            {
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
    }
}