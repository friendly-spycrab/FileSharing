using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FileSharing.FileIO
{
    /// <summary>
    /// Provider for downloading Files from File.IO
    /// </summary>
    public class FileIODownloadProvider : AbstractDownloaderProvider
    {
        /// <summary>
        /// Downloads all the data specified in the File.IO Json Responses.
        /// </summary>
        /// <example>
        /// This example shows how to download the files you just uploaded
        /// <code>
        /// 
        /// FileIOUploaderProvider uploaderProvider = new FileIOUploaderProvider();
        /// 
        /// //Upload the test.jpg after splitting it up in 50kb chunks
        /// var links = uploaderProvider.Upload(FileSplitter.SplitFile("test.jpg", 51200)).ToList();
        /// 
        /// //download the test.jpg
        /// var splitfile = new FileIODownloadProvider().Download(links);
        ///
        /// </code>
        /// </example>
        /// <param name="downloadLinks">Takes strings built like this {"success":true,"key":"2ojE41","link":"https://file.io/2ojE41","expiry":"14 days"}</param>
        /// <remarks>
        /// The files are downloaded lazily
        /// </remarks>
        /// <returns>An enumerable of all the downloads</returns>
        public override IEnumerable<byte[]> Download(IEnumerable<string> downloadLinks)
        {
            using (WebClient client = new WebClient())
            {
                foreach (var item in downloadLinks)
                {
                    yield return client.DownloadData(FileIOJson.FromJson(item).Link);
                }
            }
        }

        /// <summary>
        /// Downloads all the files listed in the downloadLinks param.
        /// </summary>
        /// <param name="downloadLinks">Takes strings built like a File.IO response e.g this {"success":true,"key":"2ojE41","link":"https://file.io/2ojE41","expiry":"14 days"}</param>
        /// <param name="filePath"></param>
        /// <remarks>
        /// The files are downloaded lazily
        /// </remarks>
        /// <returns>An enumerable of all the absoloute paths to the downloads</returns>
        public override IEnumerable<string> Download(IEnumerable<string> downloadLinks, DirectoryInfo filePath = null)
        {
            filePath = filePath ?? new DirectoryInfo(Directory.GetCurrentDirectory());
            using (WebClient client = new WebClient())
            {
                int index = 0;
                foreach (var item in downloadLinks)
                {
                    index++;
                    client.DownloadFile(FileIOJson.FromJson(item).Link, Path.Combine(filePath.FullName,$"Part{index}.split")); 
                    yield return Path.Combine(filePath.FullName, $"Part{index}.split");
                }
            }
        }
    }
}
