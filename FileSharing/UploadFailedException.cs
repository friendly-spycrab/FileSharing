using FileSharing.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSharing
{
    /// <summary>
    /// This exception is thrown when an upload fails.
    /// </summary>
    public class UploadFailedException : Exception
    {

        /// <summary>
        /// The index in the enumerable where the upload failure happened
        /// </summary>
        /// <example>
        /// <code>
        /// FileIOUploaderProvider uploaderProvider = new FileIOUploaderProvider();
        /// try
        /// {
        ///     //Here we try to upload to File.IO
        ///     uploaderProvider.Upload(new Byte[][]
        ///                             {
        ///                             new byte[] {1,1}, //Index 0
        ///                             new byte[] {2,2}, //Index 1
        ///                             new byte[] {3,3}, //Index 2
        ///                             new byte[] {4,4}, //Index 3
        ///                             new byte[] {5,5}  //Index 4
        ///                             });
        ///    
        ///     //But it fails at new byte[] {3,3},
        /// }
        /// catch (UploadFailedException ex)
        /// {
        ///     //This means UnsuccessfulUploads value will be 
        ///     //2
        ///     var indexFailure = ex.IndexOfFailure);
        /// }        
        /// </code>
        /// </example>
        public int IndexOfFailure { get; }

        /// <summary>
        /// The uploads that did not succeed. This includes the item where the exception failed
        /// </summary>
        /// <example>
        /// <code>
        /// FileIOUploaderProvider uploaderProvider = new FileIOUploaderProvider();
        /// try
        /// {
        ///     //Here we try to upload to File.IO
        ///     uploaderProvider.Upload(new Byte[][]
        ///                             {
        ///                             new byte[] {1,1},
        ///                             new byte[] {2,2},
        ///                             new byte[] {3,3},
        ///                             new byte[] {4,4},
        ///                             new byte[] {5,5}
        ///                             });
        ///    
        ///     //But it fails at new byte[] {3,3},
        /// }
        /// catch (UploadFailedException ex)
        /// {
        ///     //This means UnsuccessfulUploads value will be 
        ///     //{3,3},{4,4},{5,5}
        ///     var faileduploads = ex.UnsuccessfulUploads);
        /// }        
        /// </code>
        /// </example>
        public IEnumerable<byte[]> UnsuccessfulUploads { get; }

        /// <summary>
        /// The downloads that were successful
        /// </summary>
        public IEnumerable<string> SuccessfulDownloads { get; }

        public UploadFailedException() : base()
        {

        }

        public UploadFailedException(int indexOfFailure, IEnumerable<Byte[]> unsuccessfulUploads,IEnumerable<string> successfulDownloads) : base()
        {
            SuccessfulDownloads = successfulDownloads;
            IndexOfFailure = indexOfFailure;
            UnsuccessfulUploads = unsuccessfulUploads;
        }

        public UploadFailedException(string message) : base(message)
        {

        }

        public UploadFailedException(string message,int indexOfFailure,IEnumerable<Byte[]> unsuccessfulUploads,IEnumerable<string> successfulDownloads) : base(message)
        {
            SuccessfulDownloads = successfulDownloads;
            IndexOfFailure = indexOfFailure;
            UnsuccessfulUploads = unsuccessfulUploads;
        }

        public UploadFailedException(string message, Exception innerException) : base(message, innerException)
        {


        }
        public UploadFailedException(string message, Exception innerException, int indexOfFailure, IEnumerable<Byte[]> unsuccessfulUploads,IEnumerable<string> successfulDownloads) : base(message, innerException)
        {
            SuccessfulDownloads = successfulDownloads;
            IndexOfFailure = indexOfFailure;
            UnsuccessfulUploads = unsuccessfulUploads;
        }


        protected UploadFailedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {

        }


    }
}
