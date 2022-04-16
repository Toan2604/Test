using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    public class WebLuckyDraw_LuckyDrawNumberDTO : DataDTO
    {
        public long Id { get; set; }
        public long LuckyDrawStructureId { get; set; }
        public bool Used { get; set; }
        public WebLuckyDraw_LuckyDrawStructureDTO LuckyDrawStructure { get; set; }
        public WebLuckyDraw_LuckyDrawNumberDTO() {}
        public WebLuckyDraw_LuckyDrawNumberDTO(LuckyDrawNumber LuckyDrawNumber)
        {
            this.Id = LuckyDrawNumber.Id;
            this.LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId;
            this.Used = LuckyDrawNumber.Used;
            this.LuckyDrawStructure = LuckyDrawNumber.LuckyDrawStructure == null ? null : new WebLuckyDraw_LuckyDrawStructureDTO(LuckyDrawNumber.LuckyDrawStructure);
            this.Informations = LuckyDrawNumber.Informations;
            this.Warnings = LuckyDrawNumber.Warnings;
            this.Errors = LuckyDrawNumber.Errors;
        }
    }

    public class WebLuckyDraw_LuckyDrawNumberFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawStructureId { get; set; }
        public LuckyDrawNumberOrder OrderBy { get; set; }
    }
}
