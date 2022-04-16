using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawStructureDTO : DataDTO
    {       
        public long Id { get; set; }        
        public long LuckyDrawId { get; set; }        
        public string Name { get; set; }        
        public string Value { get; set; }        
        public long Quantity { get; set; }
        public long UsedQuantity { get; set; }        
        public LuckyDraw_LuckyDrawStructureDTO() {}
        public LuckyDraw_LuckyDrawStructureDTO(LuckyDrawStructure LuckyDrawStructure)
        {            
            this.Id = LuckyDrawStructure.Id;            
            this.LuckyDrawId = LuckyDrawStructure.LuckyDrawId;            
            this.Name = LuckyDrawStructure.Name;            
            this.Value = LuckyDrawStructure.Value;
            this.UsedQuantity = LuckyDrawStructure.UsedQuantity;            
            this.Quantity = LuckyDrawStructure.Quantity;            
            this.Informations = LuckyDrawStructure.Informations;
            this.Warnings = LuckyDrawStructure.Warnings;
            this.Errors = LuckyDrawStructure.Errors;
        }
    }

    public class LuckyDraw_LuckyDrawStructureFilterDTO : FilterDTO
    {        
        public IdFilter Id { get; set; }        
        public IdFilter LuckyDrawId { get; set; }        
        public StringFilter Name { get; set; }        
        public StringFilter Value { get; set; }        
        public LongFilter Quantity { get; set; }        
        public LuckyDrawStructureOrder OrderBy { get; set; }
    }
}