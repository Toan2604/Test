using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class SystemConfigurationDAO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
