using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyNumberDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string Value { get; set; }

        public long RewardStatusId { get; set; }

        public Guid RowId { get; set; }


        public GeneralMobile_LuckyNumberDTO() { }
        public GeneralMobile_LuckyNumberDTO(LuckyNumber LuckyNumber)
        {

            this.Id = LuckyNumber.Id;

            this.Code = LuckyNumber.Code;

            this.Name = LuckyNumber.Name;
            this.Value = LuckyNumber.Value;

            this.RewardStatusId = LuckyNumber.RewardStatusId;

            this.RowId = LuckyNumber.RowId;

            this.Errors = LuckyNumber.Errors;
        }
    }

    public class GeneralMobile_LuckyNumberFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }
        public StringFilter Value { get; set; }

        public IdFilter RewardStatusId { get; set; }

        public GuidFilter RowId { get; set; }

        public LuckyNumberOrder OrderBy { get; set; }
    }
}