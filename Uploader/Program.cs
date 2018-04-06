using FileSharing;
using FileSharing.File.IO;
using System;
using System.Collections.Generic;
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

                foreach (var item in provider.Upload(FileSplitter.SplitFileBytes(path,provider.MaxChunkSize),1000))
                {
                    Console.WriteLine(item);
                }

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

            Console.ReadLine();
        }
    }
}
