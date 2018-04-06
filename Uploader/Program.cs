using FileSharing;
using FileSharing.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Uploader
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please write path to file");
            string path = Console.ReadLine();
            FileIOUploaderProvider provider = new FileIOUploaderProvider();

            try
            {
                FileSplitter.SplitFile(path, 1024*1024*50, new DirectoryInfo("Parts"), isPathAbsoloute: true).ToList();
                //foreach (var item in provider.Upload(FileSplitter.SplitFile(path, provider.MaxChunkSize, new DirectoryInfo("Parts"), isPathAbsoloute: true), 1000))
                //{
                //    Console.WriteLine(item);
                //}
            }
            catch (UploadFailedException ex)
            {
                Console.WriteLine("Upload failed trying waiting 6s before trying again");
                Thread.Sleep(6000);
                foreach (var item in provider.Upload(ex.UnsuccessfulUploads, 2000))
                {
                    Console.WriteLine(item);
                }

            }

            Console.WriteLine("Done");
            //Console.ReadLine();
        }
    }
}
