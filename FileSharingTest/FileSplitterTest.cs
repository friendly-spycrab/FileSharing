using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileSharing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSharingTest
{
    [TestClass]
    public class FileSplitterTest
    {
        [TestMethod]
        public void TestSplitFileBytes()
        {

            IEnumerable<byte[]> newSplitFile = FileSplitter.SplitFile("test.jpg", 1024);

            if (File.Exists("newTest.jpg"))
                File.Delete("newTest.jpg");

            File.Create("newTest.jpg").Close();

            using (var stream = new FileStream("newTest.jpg", FileMode.Append))
            {
                foreach (var item in newSplitFile)
                {
                    stream.Write(item, 0, item.Length);
                }
            }



            byte[] oldFile = File.ReadAllBytes("test.jpg");
            byte[] newFile = File.ReadAllBytes("newTest.jpg");

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

        [TestMethod]
        public void TestSplitFilesSmall()
        {
            IEnumerable<string> splitFile= FileSplitter.SplitFile("test.jpg", 1024 * 15,new DirectoryInfo("Parts"),isPathAbsoloute: true);

            List<byte> splitFiles = new List<byte>();
            foreach (var item in splitFile)
            {
                splitFiles.AddRange(File.ReadAllBytes(item));
                File.Delete(item);
            }

            byte[] oldFile = File.ReadAllBytes("test.jpg");
            byte[] newFile = splitFiles.ToArray();

            bool IsEqual = true;
            for (int i = 0; i < oldFile.Length; i++)
            {
                if (oldFile[i] != newFile[i])
                {
                    IsEqual = false;
                    break;
                }
            }


            Directory.Delete("Parts");

            Assert.IsTrue(IsEqual);
        }

        [TestMethod]
        public void TestSplitFilesHuge()
        {
            IEnumerable<string> splitFile = FileSplitter.SplitFile("testhuge.zip", 1024 * 1024 * 1024, new DirectoryInfo("TestHugeParts"), isPathAbsoloute: true).ToList();

            // Todo: use an md5 hash instead and then precompute testhuge.zip
            // Comparing for length. because actually comparing 2.6GB of data takes too long
            bool sameLength = new FileInfo("testhuge.zip").Length == splitFile.Select(x => new FileInfo(x)).Sum(x => x.Length);

            foreach (var item in splitFile)
                File.Delete(item);
            

            Directory.Delete("TestHugeParts");

            Assert.IsTrue(sameLength);
        }
    }
}
