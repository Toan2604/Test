using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long LuckyDrawTypeId { get; set; }
        public long OrganizationId { get; set; }
        public decimal RevenuePerTurn { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime EndAt { get; set; }
        public long AvatarImageId { get; set; }
        public long ImageId { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public long LuckyDrawNumberId { get; set; }
        public bool Used { get; set; }
        public long RemainingTurnCounter { get; set; }
        public long UsedTurnCounter { get; set; }
        public long RemainingLuckyDrawStructureCounter { get; set; }
        public LuckyDraw_ImageDTO AvatarImage { get; set; }
        public LuckyDraw_ImageDTO Image { get; set; }
        public LuckyDraw_LuckyDrawTypeDTO LuckyDrawType { get; set; }
        public LuckyDraw_StatusDTO Status { get; set; }
        public LuckyDraw_LuckyDrawNumberDTO LuckyDrawNumber { get; set; }
        public List<LuckyDraw_LuckyDrawStoreGroupingMappingDTO> LuckyDrawStoreGroupingMappings { get; set; }
        public List<LuckyDraw_LuckyDrawStoreMappingDTO> LuckyDrawStoreMappings { get; set; }
        public List<LuckyDraw_LuckyDrawStoreTypeMappingDTO> LuckyDrawStoreTypeMappings { get; set; }
        public List<LuckyDraw_LuckyDrawStructureDTO> LuckyDrawStructures { get; set; }
        public LuckyDraw_LuckyDrawStructureDTO WinLuckyDrawStructure { get; set; }
        public List<LuckyDraw_LuckyDrawWinnerDTO> LuckyDrawWinners { get; set; }
        public List<LuckyDraw_LuckyDrawRegistrationDTO> LuckyDrawRegistrations { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public LuckyDraw_LuckyDrawDTO() { }
        public LuckyDraw_LuckyDrawDTO(LuckyDraw LuckyDraw)
        {
            this.Id = LuckyDraw.Id;
            this.Code = LuckyDraw.Code;
            this.Name = LuckyDraw.Name;
            this.LuckyDrawTypeId = LuckyDraw.LuckyDrawTypeId;
            this.OrganizationId = LuckyDraw.OrganizationId;
            this.RevenuePerTurn = LuckyDraw.RevenuePerTurn;
            this.StartAt = LuckyDraw.StartAt;
            this.EndAt = LuckyDraw.EndAt;
            this.AvatarImageId = LuckyDraw.AvatarImageId;
            this.ImageId = LuckyDraw.ImageId;
            this.Description = LuckyDraw.Description;
            this.StatusId = LuckyDraw.StatusId;
            this.Used = LuckyDraw.Used;
            this.RemainingLuckyDrawStructureCounter = LuckyDraw.RemainingLuckyDrawStructureCounter;
            this.RemainingTurnCounter = LuckyDraw.RemainingTurnCounter;
            this.UsedTurnCounter = LuckyDraw.UsedTurnCounter;
            this.LuckyDrawNumberId = LuckyDraw.LuckyDrawNumberId;
            this.AvatarImage = LuckyDraw.AvatarImage == null ? null : new LuckyDraw_ImageDTO(LuckyDraw.AvatarImage);
            this.LuckyDrawNumber = LuckyDraw.LuckyDrawNumber == null ? null : new LuckyDraw_LuckyDrawNumberDTO(LuckyDraw.LuckyDrawNumber);
            this.Image = LuckyDraw.Image == null ? null : new LuckyDraw_ImageDTO(LuckyDraw.Image);
            this.LuckyDrawType = LuckyDraw.LuckyDrawType == null ? null : new LuckyDraw_LuckyDrawTypeDTO(LuckyDraw.LuckyDrawType);
            this.Status = LuckyDraw.Status == null ? null : new LuckyDraw_StatusDTO(LuckyDraw.Status);
            this.LuckyDrawStoreGroupingMappings = LuckyDraw.LuckyDrawStoreGroupingMappings?.Select(x => new LuckyDraw_LuckyDrawStoreGroupingMappingDTO(x)).ToList();
            this.LuckyDrawStoreMappings = LuckyDraw.LuckyDrawStoreMappings?.Select(x => new LuckyDraw_LuckyDrawStoreMappingDTO(x)).ToList();
            this.LuckyDrawStoreTypeMappings = LuckyDraw.LuckyDrawStoreTypeMappings?.Select(x => new LuckyDraw_LuckyDrawStoreTypeMappingDTO(x)).ToList();
            this.LuckyDrawStructures = LuckyDraw.LuckyDrawStructures?.Select(x => new LuckyDraw_LuckyDrawStructureDTO(x)).ToList();
            this.LuckyDrawWinners = LuckyDraw.LuckyDrawWinners?.Select(x => new LuckyDraw_LuckyDrawWinnerDTO(x)).ToList();
            this.LuckyDrawRegistrations = LuckyDraw.LuckyDrawRegistrations?.Select(x => new LuckyDraw_LuckyDrawRegistrationDTO(x)).ToList();
            this.RowId = LuckyDraw.RowId;
            this.CreatedAt = LuckyDraw.CreatedAt;
            this.UpdatedAt = LuckyDraw.UpdatedAt;
            this.Informations = LuckyDraw.Informations;
            this.Warnings = LuckyDraw.Warnings;
            this.Errors = LuckyDraw.Errors;
        }
    }

    public class LuckyDraw_LuckyDrawFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter LuckyDrawTypeId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public DecimalFilter RevenuePerTurn { get; set; }
        public DateFilter StartAt { get; set; }
        public DateFilter EndAt { get; set; }
        public IdFilter AvatarImageId { get; set; }
        public IdFilter ImageId { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public LuckyDrawOrder OrderBy { get; set; }
    }
}
