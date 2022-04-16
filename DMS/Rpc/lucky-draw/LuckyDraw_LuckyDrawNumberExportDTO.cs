using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawNumberExportDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public long LuckyDrawStructureId { get; set; }
        public bool Used { get; set; }
        public string UsedString { get; set; }
        public string TimeString { get; set; }
        public LuckyDraw_LuckyDrawStructureExportDTO LuckyDrawStructure { get; set; }
        public LuckyDraw_LuckyDrawNumberExportDTO() {}
        public LuckyDraw_LuckyDrawNumberExportDTO(LuckyDrawNumber LuckyDrawNumber)
        {
            this.Id = LuckyDrawNumber.Id;
            this.LuckyDrawStructureId = LuckyDrawNumber.LuckyDrawStructureId;
            this.Used = LuckyDrawNumber.Used;
            this.LuckyDrawStructure = LuckyDrawNumber.LuckyDrawStructure == null ? null : new LuckyDraw_LuckyDrawStructureExportDTO(LuckyDrawNumber.LuckyDrawStructure);
            this.Informations = LuckyDrawNumber.Informations;
            this.Warnings = LuckyDrawNumber.Warnings;
            this.Errors = LuckyDrawNumber.Errors;
        }
    }
}
