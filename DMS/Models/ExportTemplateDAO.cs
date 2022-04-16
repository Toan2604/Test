using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class ExportTemplateDAO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}
