using DMS.ABE.Common; using TrueSight.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DMS.ABE.Entities
{
    public class ExportTemplate : DataEntity, IEquatable<ExportTemplate>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        public bool Equals(ExportTemplate other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.Code != other.Code) return false;
            if (this.Name != other.Name) return false;
            if (this.FileName != other.FileName) return false;
            if (this.Extension != other.Extension) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ExportTemplateFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter FileName { get; set; }
        public StringFilter Extension { get; set; }
        public List<ExportTemplateFilter> OrFilter { get; set; }
        public ExportTemplateOrder OrderBy { get; set; }
        public ExportTemplateSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ExportTemplateOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
        Extension = 4,
        FileName = 5,
    }

    [Flags]
    public enum ExportTemplateSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
        Extension = E._4,
        FileName = E._5,
    }
}
