using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyDrawNumberDTO : DataDTO
    {
        public long Id { get; set; }
        public long LuckyDrawStructureId { get; set; }
        public bool Used { get; set; }
        public GeneralMobile_LuckyDrawStructureDTO LuckyDrawStructure { get; set; }
        public GeneralMobile_LuckyDrawNumberDTO() {}
        public GeneralMobile_LuckyDrawNumberDTO(LuckyDrawNumber LuckyDrawNumber)
        {
            this.Id = LuckyDrawNumber.Id;
            this.LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId;
            this.Used = LuckyDrawNumber.Used;
            this.LuckyDrawStructure = LuckyDrawNumber.LuckyDrawStructure == null ? null : new GeneralMobile_LuckyDrawStructureDTO(LuckyDrawNumber.LuckyDrawStructure);
            this.Informations = LuckyDrawNumber.Informations;
            this.Warnings = LuckyDrawNumber.Warnings;
            this.Errors = LuckyDrawNumber.Errors;
        }
    }

    public class GeneralMobile_LuckyDrawNumberFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter LuckyDrawStructureId { get; set; }
        public LuckyDrawNumberOrder OrderBy { get; set; }
    }
}
