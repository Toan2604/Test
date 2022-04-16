using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawTypeDTO : DataDTO
    {
        
        public long Id { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public LuckyDraw_LuckyDrawTypeDTO() {}
        public LuckyDraw_LuckyDrawTypeDTO(LuckyDrawType LuckyDrawType)
        {
            
            this.Id = LuckyDrawType.Id;
            
            this.Code = LuckyDrawType.Code;
            
            this.Name = LuckyDrawType.Name;
            
            this.Informations = LuckyDrawType.Informations;
            this.Warnings = LuckyDrawType.Warnings;
            this.Errors = LuckyDrawType.Errors;
        }
    }

    public class LuckyDraw_LuckyDrawTypeFilterDTO : FilterDTO
    {
        
        public IdFilter Id { get; set; }
        
        public StringFilter Code { get; set; }
        
        public StringFilter Name { get; set; }
        
        public LuckyDrawTypeOrder OrderBy { get; set; }
    }
}