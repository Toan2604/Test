using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyDrawTypeDTO : DataDTO
    {        
        public long Id { get; set; }        
        public string Code { get; set; }        
        public string Name { get; set; }        
        public GeneralMobile_LuckyDrawTypeDTO() {}
        public GeneralMobile_LuckyDrawTypeDTO(LuckyDrawType LuckyDrawType)
        {            
            this.Id = LuckyDrawType.Id;            
            this.Code = LuckyDrawType.Code;            
            this.Name = LuckyDrawType.Name;            
            this.Informations = LuckyDrawType.Informations;
            this.Warnings = LuckyDrawType.Warnings;
            this.Errors = LuckyDrawType.Errors;
        }
    }

    public class GeneralMobile_LuckyDrawTypeFilterDTO : FilterDTO
    {        
        public IdFilter Id { get; set; }        
        public StringFilter Code { get; set; }        
        public StringFilter Name { get; set; }        
        public LuckyDrawTypeOrder OrderBy { get; set; }
    }
}