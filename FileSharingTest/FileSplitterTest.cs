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
        public void TestFileSplit()
        {

            IEnumerable<byte[]> newSplitFile = FileSplitter.SplitFileBytes("test.jpg", 1024);

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
    }
}
