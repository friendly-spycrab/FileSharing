using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSharing
{
    public abstract class AbstractUploaderProvider
    {
        public abstract long MaxChunkSize { get; protected set; }


        /// <summary>
        /// Uploads the byte arrays to the website
        /// </summary>
        /// <param name="inputs">Should probably a Lazy loaded array</param>
        /// <param name="sleepTimeBetweenDownloadsMS">The amount of time the method should sleep the thread after each download in milliseconds. Default 1000Ms</param>
        /// <seealso cref="FileSplitter.SplitFile(string, int)"/>
        /// <returns></returns>
        public abstract IEnumerable<string> Upload(IEnumerable<byte[]> inputs,int sleepTimeBetweenDownloadsMS = 1000);

        /// <summary>
        /// Uploads the files inputted to files enumerable
        /// </summary>
        /// <param name="Files">an enumerable to all the paths for the files</param>
        /// <param name="sleepTimeBetweenDownloadsMS">The amount of time the method should sleep the thread after each download in milliseconds. Default 1000Ms</param>
        /// <returns></returns>
        /// <seealso cref="FileSplitter.SplitFile"/>
        public abstract IEnumerable<string> Upload(IEnumerable<string> Files, int sleepTimeBetweenDownloadsMS = 1000);

    }
}
