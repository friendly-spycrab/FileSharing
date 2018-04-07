using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSharing
{
    public abstract class AbstractDownloaderProvider
    {
        public abstract IEnumerable<byte[]> Download(IEnumerable<string> downloadLinks);

        public abstract IEnumerable<string> Download(IEnumerable<string> downloadLinks, DirectoryInfo filePath = null);
    }
}
