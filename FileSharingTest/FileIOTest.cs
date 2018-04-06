using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using FileSharing;
using FileSharing.FileIO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSharingTest
{
    [TestClass]
    public class FileIOTest
    {
        [TestMethod]
        public void UploadDownloadTest()
        {
            FileIOUploaderProvider uploaderProvider = new FileIOUploaderProvider();

            var links = uploaderProvider.Upload(FileSplitter.SplitFile("test.jpg", 51200)).ToList();


            byte[] oldFile = File.ReadAllBytes("test.jpg");
            byte[] newFile = new FileIODownloadProvider().Download(links).Aggregate<IEnumerable<byte>>((x,y) => x.Concat(y)).ToArray();

            bool IsEqual = true;
            for (int i = 0; i < oldFile.Length; i++)
            {
                if (oldFile[i] != newFile[i])
                {
                    IsEqual = false;
                    break;
                }
            }
            Assert.IsTrue(IsEqual);
        }

        [TestCleanup]
        public void Wait()
        {
            Thread.Sleep(4000);
        }

        [TestMethod]
        public void UploadDownloadTestException() => Assert.ThrowsException<UploadFailedException>(() => new FileIOUploaderProvider().Upload(FileSplitter.SplitFile("test.jpg", 1024 * 5),5).ToList());
    }
}
