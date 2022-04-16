using System.IO;

namespace DMS.Common
{
    public class DataFile
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Stream Content { get; set; }
    }
}
