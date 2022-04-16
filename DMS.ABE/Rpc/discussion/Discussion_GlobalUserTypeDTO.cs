using TrueSight.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.discussion
{
    public class Discussion_GlobalUserTypeDTO : DataDTO
    {        
        public long Id { get; set; }        
        public string Code { get; set; }        
        public string Name { get; set; }        
        public Discussion_GlobalUserTypeDTO() {}
        public Discussion_GlobalUserTypeDTO(GlobalUserType GlobalUserType)
        {
            
            this.Id = GlobalUserType.Id;            
            this.Code = GlobalUserType.Code;            
            this.Name = GlobalUserType.Name;
        }
    }
    public class Discussion_GlobalUserTypeFilterDTO : FilterDTO
    {        
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }        
        public StringFilter Name { get; set; }        
        public GlobalUserTypeOrder OrderBy { get; set; }
    }
}