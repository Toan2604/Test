using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public class LuckyNumber_LuckyNumberDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public long RewardStatusId { get; set; }
        public Guid RowId { get; set; }
        public LuckyNumber_LuckyNumberGroupingDTO LuckyNumberGrouping { get; set; }
        public LuckyNumber_RewardStatusDTO RewardStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public string eUsedAt => UsedAt?.ToString("dd-MM-yyyy");
        public bool Used { get; set; }
        public LuckyNumber_LuckyNumberDTO() { }
        public LuckyNumber_LuckyNumberDTO(LuckyNumber LuckyNumber)
        {
            this.Id = LuckyNumber.Id;
            this.Code = LuckyNumber.Code;
            this.Name = LuckyNumber.Name;
            this.Value = LuckyNumber.Value;
            this.RewardStatusId = LuckyNumber.RewardStatusId;
            this.RowId = LuckyNumber.RowId;
            this.Used = LuckyNumber.Used;
            this.RewardStatus = LuckyNumber.RewardStatus == null ? null : new LuckyNumber_RewardStatusDTO(LuckyNumber.RewardStatus);
            this.LuckyNumberGrouping = LuckyNumber.LuckyNumberGrouping == null ? null : new LuckyNumber_LuckyNumberGroupingDTO(LuckyNumber.LuckyNumberGrouping);
            this.CreatedAt = LuckyNumber.CreatedAt;
            this.UpdatedAt = LuckyNumber.UpdatedAt;
            this.UsedAt = LuckyNumber.UsedAt;
            this.Errors = LuckyNumber.Errors;
        }
    }

    public class LuckyNumber_LuckyNumberFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter RewardStatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public DateFilter UsedAt { get; set; }
        public LuckyNumberOrder OrderBy { get; set; }
    }
}
