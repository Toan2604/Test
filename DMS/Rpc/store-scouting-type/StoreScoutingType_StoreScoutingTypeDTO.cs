using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.store_scouting_type
{
    public class StoreScoutingType_StoreScoutingTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public StoreScoutingType_StoreScoutingTypeDTO() { }
        public StoreScoutingType_StoreScoutingTypeDTO(StoreScoutingType StoreScoutingType)
        {
            this.Id = StoreScoutingType.Id;
            this.Code = StoreScoutingType.Code;
            this.Name = StoreScoutingType.Name;
            this.StatusId = StoreScoutingType.StatusId;
            this.CreatedAt = StoreScoutingType.CreatedAt;
            this.UpdatedAt = StoreScoutingType.UpdatedAt;
            this.Used = StoreScoutingType.Used;
            this.Errors = StoreScoutingType.Errors;
        }
    }

    public class StoreScoutingType_StoreScoutingTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public StoreScoutingTypeOrder OrderBy { get; set; }
    }
}
