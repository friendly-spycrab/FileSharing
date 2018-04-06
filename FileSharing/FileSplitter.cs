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
        /// <exception cref="OutOfMemoryException">If the file is over whats allowed by the max</exception>
        public static IEnumerable<byte[]> SplitFile(string filePath, int chunkSize)
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
        /// Splits the file lazily into multiple subfiles.
        /// </summary>
        /// <remarks>
        /// This requires that you have permission to write on the system.
        /// If you only need the bytes and your file is smaller than whats allowed by the clr use <see cref="SplitFile(string, int)"/> instead
        /// </remarks>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="chunkSize">the size of the chunks to split the files into</param>
        /// <param name="splitFilesDirectory">the folder where to store the files. if the folder does not exist, then it will created. Default is the current directory</param>
        /// <param name="splitFilesName">the name of the splitfiles. For example if splitFilesName = "Part" then the files will be named "Part1.split","Part2.split" and so on</param>
        /// <param name="isPathAbsoloute">Whether the returned paths to the files are absoloute or relative to the splitFilesDirectory</param>
        /// <returns>An enumerable of all the paths to the split files.</returns>
        /// <example>
        /// This example shows how to use this method
        /// <code>
        /// //Splits the test.jpg into 50kb chunks and outputs them into the current directory
        /// var files = SplitFile("Test.jpg", 1024 * 50);
        /// foreach (var item in files)
        /// {
        ///     Console.WriteLine(item);
        ///     /* Prints
        ///      * Path
        ///      * Part1.split
        ///      * Part2.split
        ///      * Part3.split
        ///      * And so on
        ///      */
        /// }
        /// </code>
        /// </example>
        public static IEnumerable<string> SplitFile(string filePath, long chunkSize, DirectoryInfo splitFilesDirectory = null, string splitFilesName = "Part", bool isPathAbsoloute = false)
        {
            if (splitFilesDirectory == null)
                splitFilesDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());

            Directory.CreateDirectory(splitFilesDirectory.FullName);

            FileInfo info = new FileInfo(filePath);

            FileStream stream = info.OpenRead();


            //If the length of the file is smaller than the chunk size just copy the entire file to the folder with the new name
            if (info.Length < chunkSize)
            {
                File.Copy(info.FullName, Path.Combine(splitFilesDirectory.FullName, $"{splitFilesName}1.split"), true);
                yield return isPathAbsoloute ? 
                    Path.Combine(splitFilesDirectory.FullName, $"{splitFilesName}1.split") :
                    Path.Combine(splitFilesDirectory.FullName, $"{splitFilesName}1.split").Replace(splitFilesDirectory.FullName,"").Remove(0, 1);

                yield break;
            }


            int index = 1;
            int bytesRead = 1;
            do
            {
                FileStream writeStream = File.Create(Path.Combine(splitFilesDirectory.FullName, $"{splitFilesName}{index}.split"));
                

                //Read file using pages
                int pageSize = Environment.SystemPageSize;

                
                byte[] bytes = new byte[pageSize];
                int chunksizeRead = 0;

                //Reads the file in a system page or less
                while (chunkSize - chunksizeRead > 0 && bytesRead > 0)
                {
                    if (chunkSize - chunksizeRead > pageSize)
                        bytesRead = stream.Read(bytes, 0, pageSize);
                    else
                        bytesRead = stream.Read(bytes, 0, (int)(chunkSize - chunksizeRead));

                    writeStream.Write(bytes, 0, bytesRead);
                    chunksizeRead += bytesRead;
                }

                writeStream.Close();
                index++;

                yield return isPathAbsoloute ?
                    Path.Combine(splitFilesDirectory.FullName, $"{splitFilesName}{index - 1}.split") :
                    Path.Combine(splitFilesDirectory.FullName, $"{splitFilesName}{index - 1}.split").Replace(splitFilesDirectory.FullName, "").Remove(0,1);

            } while (bytesRead > 0);

        }
    }
}
