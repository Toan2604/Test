using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_RewardHistoryContentDTO : DataDTO
    {
        public long Id { get; set; }
        public long RewardHistoryId { get; set; }
        public long LuckyNumberId { get; set; }
        public GeneralMobile_LuckyNumberDTO LuckyNumber { get; set; }

        public GeneralMobile_RewardHistoryContentDTO() { }
        public GeneralMobile_RewardHistoryContentDTO(RewardHistoryContent RewardHistoryContent)
        {
            this.Id = RewardHistoryContent.Id;
            this.RewardHistoryId = RewardHistoryContent.RewardHistoryId;
            this.LuckyNumberId = RewardHistoryContent.LuckyNumberId;
            this.LuckyNumber = RewardHistoryContent.LuckyNumber == null ? null : new GeneralMobile_LuckyNumberDTO(RewardHistoryContent.LuckyNumber);
            this.Errors = RewardHistoryContent.Errors;
        }
    }

    public class GeneralMobile_RewardHistoryContentFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter RewardHistoryId { get; set; }

        public IdFilter LuckyNumberId { get; set; }

        public RewardHistoryContentOrder OrderBy { get; set; }
    }
}