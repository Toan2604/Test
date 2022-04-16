using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TrueSight.Common;

namespace DMS.ABE.Entities
{
    public class PublicFile : DataEntity
    {
        public long Id { get; set; }
        public string Key
        {
            get
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(Path));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
        }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public byte[] Content { get; set; }
        public string MimeType
        {
            get
            {
                FileInfo fileInfo = new FileInfo(Path);
                if (fileInfo.Extension.ToLower() == ".jpg" || fileInfo.Extension.ToLower() == ".jpeg")
                    return "image/jpeg";
                if (fileInfo.Extension.ToLower() == ".png")
                    return "image/png";
                if (fileInfo.Extension.ToLower() == ".gif")
                    return "image/gif";
                if (fileInfo.Extension.ToLower() == ".pdf")
                    return "application/pdf";
                return "application/octet-stream";
            }
        }
        public long? Size { get; set; }
        public bool IsFile { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class PublicFileFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Path { get; set; }
        public LongFilter Level { get; set; }
        public bool? IsFile { get; set; }
        public PublicFileOrder OrderBy { get; set; }
    }

    public enum PublicFileOrder
    {
        Id = 1,
        Path = 2,
        Level = 3,
        IsPublicFile = 4,
    }
}
