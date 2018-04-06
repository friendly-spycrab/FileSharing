using FileSharing;
using FileSharing.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uploader
{
    /// <summary>
    /// Example program for uploading a file to file.io and getting responses back
    /// </summary>
    static class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Please write path to file");
            string path = Console.ReadLine();
            FileIOUploaderProvider provider = new FileIOUploaderProvider();

            try
            {
                IEnumerable<string> files = FileSplitter.SplitFile(path, provider.MaxChunkSize, new DirectoryInfo("Parts"), isPathAbsoloute: true).ToList();

                foreach (var item in provider.Upload(files, 1000))
                {
                    Console.WriteLine(item);
                }

                foreach (var item in files)
                    File.Delete(item);
                    

            }
            catch (UploadFailedException ex)
            {
                Console.WriteLine("Upload failed");
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
