using DMS.ABE.Common; using TrueSight.Common;
using System.IO;

namespace DMS.ABE.Entities
{
    public class Attachment : DataEntity
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public string Extension
        {
            get
            {
                var fileInfo = new FileInfo(FileName);
                return fileInfo.Extension;
            }
        }
    }
}
