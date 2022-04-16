using DMS.Entities;

namespace DMS.Rpc.survey
{
    public class Survey_FileDTO
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string MimeType { get; set; }
        public bool IsFile { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public Survey_FileDTO() { }
        public Survey_FileDTO(File File)
        {
            this.Id = File.Id;
            this.Key = File.Key;
            this.Name = File.Name;
            this.MimeType = File.MimeType;
            this.IsFile = File.IsFile;
            this.Path = File.Path;
            this.Level = File.Level;
        }
    }
}
