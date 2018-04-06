using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSharing.FileIO
{
    /// <summary>
    /// Provides methods for uploading to FileIO
    /// </summary>
    /// <remarks>
    /// FileIO deletes files after 2 weeks in default.
    /// FileIO links can only be used once after that the files is deleted
    /// </remarks>
    public class FileIOUploaderProvider : AbstractUploaderProvider
    {
        /// <summary>
        /// The max size of an upload for FileIO. Currently 5GB or 1024 * 1024 * 1024 * 5
        /// </summary>
        /// <remarks>
        /// Even though the actual max file size of a FileIO upload is 5GB. because of the fact that C# streams only takes an integer for offsets and lengths.
        /// </remarks>
        public override long MaxChunkSize { get; protected set; } = (long)(1024 * 1024 * 1024) * 5;

        /// <summary>
        /// Uploads a the bytes to FileIO and gives links back for the file
        /// </summary>
        /// <param name="inputs">Uploads all arrays and returns an enumerable with all the download links</param>
        /// <param name="sleepTimeBetweenDownloadsMS">
        /// The amount of time the method should sleep the thread after each download in milliseconds. Default 1000Ms.
        /// The default should be enough. Change this only if you have problems with uploading
        /// </param>
        /// <returns>Returns the responses for the uploads. To get the download link Use the <see cref="FileIOJson.FromJson(string)"/> to get a class that has that property</returns>
        /// <remarks>
        /// The method waits 1s after uploading a file. this is so there is a smaller chance of the server returning success:false.
        /// </remarks>
        /// <example>
        /// This example shows how to upload the files and then download them again
        /// <code>
        /// 
        /// FileIOUploaderProvider uploaderProvider = new FileIOUploaderProvider();
        /// 
        /// //Upload the test.jpg after splitting it up in 50kb chunks
        /// var links = uploaderProvider.Upload(FileSplitter.SplitFileBytes("test.jpg", 51200)).ToList();
        /// 
        /// //download the test.jpg
        /// var splitfile = new FileIODownloadProvider().Download(links);
        ///
        /// </code>
        /// </example>
        /// <exception cref="UploadFailedException">
        /// Throws UploadFailedException if an upload is unsuccesful
        /// </exception>
        public override IEnumerable<string> Upload(IEnumerable<byte[]> inputs, int sleepTimeBetweenDownloadsMS = 1000)
        {
            //Keep a list of successful download. So if an upload fails the UploadFailedException can give an enumerable that contains the successful uploads.
            List<string> successfulDownloads = new List<string>();
            int index = 0;

            foreach (var item in inputs)
            {

                var client = new RestClient("https://File.IO");
                var request = new RestRequest(Method.POST);

                request.AddFile("file", item, $"Part{index}.bin");

                index++;

                IRestResponse response = client.Execute(request);

                FileIOJson responseData = FileIOJson.FromJson(response.Content);

                if (!responseData.Success)
                {
                    List<byte[]> inputsList = inputs.ToList();
                    throw new UploadFailedException($"Raw Response: {response.Content}", index, inputsList.GetRange(index, inputsList.Count - index), successfulDownloads);
                }


                Thread.Sleep(sleepTimeBetweenDownloadsMS);

                successfulDownloads.Add(response.Content);
                yield return response.Content;

            }

            successfulDownloads.Clear();
        }

        public override IEnumerable<string> Upload(IEnumerable<string> Files, int sleepTimeBetweenDownloadsMS = 1000)
        {
            //Keep a list of successful download. So if an upload fails the UploadFailedException can give an enumerable that contains the successful uploads.
            List<string> successfulDownloads = new List<string>();
            int index = 0;

            foreach (var item in Files)
            {

                var client = new RestClient("https://File.IO");
                var request = new RestRequest(Method.POST);

                request.AddFile("file", item);

                index++;

                IRestResponse response = client.Execute(request);

                FileIOJson responseData = FileIOJson.FromJson(response.Content);

                //if (!responseData.Success)
                //{
                //    List<string> inputsList = inputs.ToList();
                //    throw new UploadFailedException($"Raw Response: {response.Content}", index, inputsList.GetRange(index, inputsList.Count - index), successfulDownloads);
                //}


                Thread.Sleep(sleepTimeBetweenDownloadsMS);

                successfulDownloads.Add(response.Content);
                yield return response.Content;

            }

            successfulDownloads.Clear();
        }
    }
}
