using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public class LuckyNumber_RewardHistoryDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public long TurnCounter { get; set; }
        public decimal Revenue { get; set; }
        public Guid RowId { get; set; }
        public LuckyNumber_AppUserDTO AppUser { get; set; }
        public LuckyNumber_StoreDTO Store { get; set; }
        public List<LuckyNumber_RewardHistoryContentDTO> RewardHistoryContents { get; set; }
        public string Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public LuckyNumber_RewardHistoryDTO() { }
        public LuckyNumber_RewardHistoryDTO(RewardHistory RewardHistory)
        {
            this.Id = RewardHistory.Id;
            this.AppUserId = RewardHistory.AppUserId;
            this.StoreId = RewardHistory.StoreId;
            this.TurnCounter = RewardHistory.TurnCounter;
            this.Revenue = RewardHistory.Revenue;
            this.RowId = RewardHistory.RowId;
            this.AppUser = RewardHistory.AppUser == null ? null : new LuckyNumber_AppUserDTO(RewardHistory.AppUser);
            this.Store = RewardHistory.Store == null ? null : new LuckyNumber_StoreDTO(RewardHistory.Store);
            this.RewardHistoryContents = RewardHistory.RewardHistoryContents?.Select(x => new LuckyNumber_RewardHistoryContentDTO(x)).ToList();
            this.CreatedAt = RewardHistory.CreatedAt;
            this.UpdatedAt = RewardHistory.UpdatedAt;
            this.Errors = RewardHistory.Errors;
        }
    }

    public class LuckyNumber_RewardHistoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter StoreId { get; set; }
        public LongFilter TurnCounter { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public RewardHistoryOrder OrderBy { get; set; }
    }
}
