using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_item_report
{
    public class KpiItemReport_KpiItemContentDTO : DataDTO
    {
        public long STT { get; set; }
        public long SaleEmployeeId { get; set; }
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }

        //Sản lượng theo đơn hàng gián tiếp
        public decimal? IndirectQuantityPlanned { get; set; }
        public decimal? IndirectQuantity { get; set; }
        public decimal? IndirectQuantityRatio { get; set; }

        //Doanh số theo đơn hàng gián tiếp
        public decimal? IndirectRevenuePlanned { get; set; }
        public decimal? IndirectRevenue { get; set; }
        public decimal? IndirectRevenueRatio { get; set; }

        //Số đơn hàng gián tiếp
        public decimal? IndirectAmountPlanned { get; set; }
        internal HashSet<long> IndirectSalesOrderIds { get; set; }
        public decimal? IndirectAmount
        {
            get
            {
                if (IndirectSalesOrderIds == null || IndirectAmountPlanned == null) return null;
                return IndirectSalesOrderIds.Count;
            }
        }
        public decimal? IndirectAmountRatio { get; set; }

        //Số đại lý theo đơn gián tiếp
        public decimal? IndirectStorePlanned { get; set; }
        internal HashSet<long> StoreIndirectIds { get; set; }
        public decimal? IndirectStore
        {
            get
            {
                if (StoreIndirectIds == null || IndirectStorePlanned == null) return null;
                return StoreIndirectIds.Count;
            }
        }
        public decimal? IndirectStoreRatio { get; set; }

        //Sản lượng theo đơn hàng trực tiếp
        public decimal? DirectQuantityPlanned { get; set; }
        public decimal? DirectQuantity { get; set; }
        public decimal? DirectQuantityRatio { get; set; }

        //Doanh số theo đơn hàng trực tiếp
        public decimal? DirectRevenuePlanned { get; set; }
        public decimal? DirectRevenue { get; set; }
        public decimal? DirectRevenueRatio { get; set; }

        //Số đơn hàng trực tiếp
        public decimal? DirectAmountPlanned { get; set; }
        internal HashSet<long> DirectSalesOrderIds { get; set; }
        public decimal? DirectAmount
        {
            get
            {
                if (DirectSalesOrderIds == null || DirectAmountPlanned == null) return null;
                return DirectSalesOrderIds.Count;
            }
        }
        public decimal? DirectAmountRatio { get; set; }

        //Số đại lý theo đơn trực tiếp
        public decimal? DirectStorePlanned { get; set; }
        internal HashSet<long> StoreDirectIds { get; set; }
        public decimal? DirectStore
        {
            get
            {
                if (StoreDirectIds == null || DirectStorePlanned == null) return null;
                return StoreDirectIds.Count;
            }
        }
        public decimal? DirectStoreRatio { get; set; }
    }
}
