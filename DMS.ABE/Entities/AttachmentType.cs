using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.ABE.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.ABE.Entities
{
    public class AttachmentType : DataEntity,  IEquatable<AttachmentType>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        
        public bool Equals(AttachmentType other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.Code != other.Code) return false;
            if (this.Name != other.Name) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class AttachmentTypeFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public List<AttachmentTypeFilter> OrFilter { get; set; }
        public AttachmentTypeOrder OrderBy {get; set;}
        public AttachmentTypeSelect Selects {get; set;}
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AttachmentTypeOrder
    {
        Id = 0,
        Code = 1,
        Name = 2,
    }

    [Flags]
    public enum AttachmentTypeSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        Name = E._2,
    }
}
