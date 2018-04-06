using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSharing
{
    public static class FileSplitter
    {
        /// <summary>
        /// Splits the file lazily and returns an enumerable that contains the file split into byte arrays
        /// </summary>
        /// <param name="filePath">the path to the file</param>
        /// <param name="chunkSize">the size of a chunk in bytes</param>
        /// <remarks>
        /// This method stores the entire split file in memory.
        /// Use <see cref="SplitFile(string, int)"/> if you want to work on files bigger than 2GB on 32bit systems.
        /// </remarks>
        /// <returns></returns>
        /// <exception cref="OutOfMemoryException">If the file is over whats allowed by the clr</exception>
        public static IEnumerable<byte[]> SplitFileBytes(string filePath, int chunkSize)
        {
            FileInfo info = new FileInfo(filePath);

            FileStream stream = info.OpenRead();

            int bytesRead = 0;

            //If the length of the file is smaller than the chunk size just return the entire file as bytes
            if (info.Length < chunkSize)
            {
                yield return System.IO.File.ReadAllBytes(filePath);
                yield break;
            }

            int offset = 0;
            do
            {
                byte[] chunk = new byte[chunkSize];

                stream.Seek(offset, SeekOrigin.Begin);
                bytesRead = stream.Read(chunk, 0, chunkSize);
                offset += chunkSize;

                if (chunkSize == bytesRead)
                    yield return chunk;
                else
                    yield return chunk.Take(bytesRead).ToArray();

            } while (bytesRead == chunkSize);
        }

        /// <summary>
        /// Splits the file into multiple subfiles.
        /// </summary>
        /// <remarks>
        /// This requires that you have permission to write on the system.
        /// If you only need the bytes and your file is smaller than whats allowed by the clr use <see cref="SplitFileBytes(string, int)"/> instead
        /// </remarks>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="chunkSize">the size of the chunks to split the files into</param>
        /// <param name="splitFilesDirectory">the folder where to store the files. if the folder does not exist, then it will created. Default is the working directory</param>
        /// <param name="splitFilesName">the name of the splitfiles. For example if splitFilesName = "Part" then the files will be named "Part1.split","Part2.split" and so on</param>
        /// <returns>An enumerable of all the absoloute paths to the split files</returns>
        /// 
        public static IEnumerable<string> SplitFile(string filePath, int chunkSize,DirectoryInfo splitFilesDirectory = null,string splitFilesName = "Part")
        {
            if (splitFilesDirectory == null)
                splitFilesDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            Directory.CreateDirectory(splitFilesDirectory.FullName);


        }
    }
}
