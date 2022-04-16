using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.lucky_number
{
    public class LuckyNumber_RewardStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public LuckyNumber_RewardStatusDTO() { }
        public LuckyNumber_RewardStatusDTO(RewardStatus RewardStatus)
        {

            this.Id = RewardStatus.Id;

            this.Code = RewardStatus.Code;

            this.Name = RewardStatus.Name;

            this.Errors = RewardStatus.Errors;
        }
    }

    public class LuckyNumber_RewardStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public RewardStatusOrder OrderBy { get; set; }
    }
}