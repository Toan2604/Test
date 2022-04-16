using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawStructureExportDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }        
        public long LuckyDrawId { get; set; }        
        public LuckyDraw_LuckyDrawDTO LuckyDraw { get; set; }        
        public string Name { get; set; }        
        public string Value { get; set; }        
        public long Quantity { get; set; }        
        public long GivenPrizeCounter { get; set; }        
        public long RemainingPrizeCounter { get; set; }        
        public LuckyDraw_LuckyDrawStructureExportDTO() {}
        public LuckyDraw_LuckyDrawStructureExportDTO(LuckyDrawStructure LuckyDrawStructure)
        {            
            this.Id = LuckyDrawStructure.Id;
            this.LuckyDrawId = LuckyDrawStructure.LuckyDrawId;            
            this.Name = LuckyDrawStructure.Name;            
            this.Value = LuckyDrawStructure.Value;            
            this.Quantity = LuckyDrawStructure.Quantity;            
            this.Informations = LuckyDrawStructure.Informations;
            this.Warnings = LuckyDrawStructure.Warnings;
            this.Errors = LuckyDrawStructure.Errors;
            this.LuckyDraw = LuckyDrawStructure.LuckyDraw == null ? null : new LuckyDraw_LuckyDrawDTO(LuckyDrawStructure.LuckyDraw);
        }
    }
}