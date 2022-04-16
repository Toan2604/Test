using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawNumberDTO : DataDTO
    {       
        public long Id { get; set; }        
        public long LuckyDrawStructureId { get; set; }        
        public bool Used { get; set; }        
        public LuckyDraw_LuckyDrawNumberDTO() {}
        public LuckyDraw_LuckyDrawNumberDTO(LuckyDrawNumber LuckyDrawNumber)
        {            
            this.Id = LuckyDrawNumber.Id;            
            this.LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId;            
            this.Used = LuckyDrawNumber.Used;            
            this.Informations = LuckyDrawNumber.Informations;
            this.Warnings = LuckyDrawNumber.Warnings;
            this.Errors = LuckyDrawNumber.Errors;
        }
    }
    public class LuckyDraw_LuckyDrawNumberFilterDTO : FilterDTO
    {        
        public IdFilter Id { get; set; }        
        public IdFilter LuckyDrawStructureId { get; set; }        
        public LuckyDrawNumberOrder OrderBy { get; set; }
    }
}