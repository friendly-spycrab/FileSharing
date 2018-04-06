﻿using System;
using System.Collections.Generic;
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
        /// <param name="downloadLinks"></param>
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
        /// Todo: implement downloading files larger than 2GB
        /// </summary>
        /// <param name="downloadLinks"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public override IEnumerable<string> Download(IEnumerable<string> downloadLinks, string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
