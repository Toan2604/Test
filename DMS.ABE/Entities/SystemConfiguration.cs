using DMS.ABE.Common; using TrueSight.Common;

namespace DMS.ABE.Entities
{
    public class SystemConfiguration : DataEntity
    {
        public long STORE_CHECKING_DISTANCE { get; set; }
        public bool STORE_CHECKING_OFFLINE_CONSTRAINT_DISTANCE { get; set; }
        public bool USE_DIRECT_SALES_ORDER { get; set; }
        public bool USE_INDIRECT_SALES_ORDER { get; set; }
        public bool ALLOW_EDIT_KPI_IN_PERIOD { get; set; }
        /// <summary>
        /// 0: lấy giá thấp
        /// 1: lấy giá cao
        /// </summary>
        public long PRIORITY_USE_PRICE_LIST { get; set; }
        public long PRIORITY_USE_PROMOTION { get; set; }
        public long STORE_CHECKING_MINIMUM_TIME { get; set; }
        public long DASH_BOARD_REFRESH_TIME { get; set; }
        public decimal AMPLITUDE_PRICE_IN_DIRECT { get; set; }
        public decimal AMPLITUDE_PRICE_IN_INDIRECT { get; set; }
        public string YOUTUBE_ID { get; set; }
        public bool ALLOW_DRAFT_STORE_TO_CREATE_ORDER { get; set; }
        public bool ALLOW_EDIT_PRICE_IN_DIRECT_SALES_ORDER { get; set; }
        public bool ALLOW_EDIT_PRICE_IN_INDIRECT_SALES_ORDER { get; set; }
        public bool ALLOW_BUYER_STORE_APPROVE_DIRECT_SALES_ORDER { get; set; }
        public bool SYNC_DIRECT_SALES_ORDER_WITH_ERP { get; set; }
        public bool ERP_CALCULATE_SALES_ORDER_PRICE { get; set; }
        public string URL_API_FOR_ERP_CALCULATE_SALES_ORDER { get; set; }
        public bool ALLOW_UPDATE_APPROVED_PRICE_LIST { get; set; }
        public long START_WORKFLOW_BY_USER_TYPE { get; set; }
        public bool USE_ELASTICSEARCH { get; set; }
    }
}
