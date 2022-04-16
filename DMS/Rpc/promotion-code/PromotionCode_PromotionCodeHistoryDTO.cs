using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.promotion_code
{
    public class PromotionCode_PromotionCodeHistoryDTO : DataDTO
    {
        public long Id { get; set; }
        public long PromotionCodeId { get; set; }
        public DateTime AppliedAt { get; set; }
        public Guid RowId { get; set; }
        public PromotionCode_DirectSalesOrderDTO DirectSalesOrder { get; set; }
        public PromotionCode_PromotionCodeHistoryDTO() { }
        public PromotionCode_PromotionCodeHistoryDTO(PromotionCodeHistory PromotionCodeHistory)
        {
            this.Id = PromotionCodeHistory.Id;
            this.PromotionCodeId = PromotionCodeHistory.PromotionCodeId;
            this.AppliedAt = PromotionCodeHistory.AppliedAt;
            this.RowId = PromotionCodeHistory.RowId;
            this.RowId = PromotionCodeHistory.RowId;
            this.DirectSalesOrder = PromotionCodeHistory.DirectSalesOrder == null ? null : new PromotionCode_DirectSalesOrderDTO(PromotionCodeHistory.DirectSalesOrder);
            this.Errors = PromotionCodeHistory.Errors;
        }
    }

    public class PromotionCode_PromotionCodeHistoryFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter PromotionCodeId { get; set; }

        public DateFilter AppliedAt { get; set; }

        public GuidFilter RowId { get; set; }

        public PromotionCodeHistoryOrder OrderBy { get; set; }
    }
}