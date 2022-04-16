using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_period_report
{
    public class KpiGeneralPeriodReport_SaleEmployeeDTO : DataDTO
    {
        public long STT { get; set; }
        public long SaleEmployeeId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string OrganizationName { get; set; }

        #region các chỉ tiêu tạm ẩn

        ////SKU gián tiếp
        //public decimal? SkuIndirectOrderPlanned { get; set; }
        //internal List<long> SKUIndirectItems { get; set; }
        //public decimal? SkuIndirectOrder { get; set; }
        //public decimal? SkuIndirectOrderRatio { get; set; }

        ////Tổng sản lượng theo đơn hàng trực tiếp
        //public decimal? TotalDirectQuantityPlanned { get; set; }
        //public decimal? TotalDirectQuantity { get; set; }
        //public decimal? TotalDirectQuantityRatio { get; set; }

        ////SKU trực tiếp
        //public decimal? SkuDirectOrderPlanned { get; set; }
        //internal List<long> SKUDirectItems { get; set; }
        //public decimal? SkuDirectOrder { get; set; }
        //public decimal? SkuDirectOrderRatio { get; set; }
        #endregion

        //Tổng số đại lý mua đơn hàng gián tiếp
        public decimal? IndirectSalesBuyerStorePlanned { get; set; }
        public decimal? IndirectSalesBuyerStore { get; set; }
        public decimal? IndirectSalesBuyerStoreRatio { get; set; }
        //Tổng số đại lý mua đơn hàng trực tiếp
        public decimal? DirectSalesBuyerStorePlanned { get; set; }
        public decimal? DirectSalesBuyerStore { get; set; }
        public decimal? DirectSalesBuyerStoreRatio { get; set; }


        //Tổng sản lượng theo đơn hàng gián tiếp
        public decimal? TotalIndirectSalesQuantityPlanned { get; set; }
        public decimal? TotalIndirectSalesQuantity { get; set; }
        public decimal? TotalIndirectSalesQuantityRatio { get; set; }
        //Tổng sản lượng theo đơn hàng trực tiếp
        public decimal? TotalDirectSalesQuantityPlanned { get; set; }
        public decimal? TotalDirectSalesQuantity { get; set; }
        public decimal? TotalDirectSalesQuantityRatio { get; set; }

        //Số đơn hàng trực tiếp
        public decimal? TotalDirectOrdersPlanned { get; set; }
        public decimal? TotalDirectOrders { get; set; }
        public decimal? TotalDirectOrdersRatio { get; set; }

        //Doanh thu theo đơn hàng trực tiếp
        public decimal? TotalDirectSalesAmountPlanned { get; set; }
        public decimal? TotalDirectSalesAmount { get; set; }
        public decimal? TotalDirectSalesAmountRatio { get; set; }

        //Doanh thu theo đơn hàng gián tiếp
        public decimal? TotalIndirectSalesAmountPlanned { get; set; }
        public decimal? TotalIndirectSalesAmount { get; set; }
        public decimal? TotalIndirectSalesAmountRatio { get; set; }

        //Doanh thu C2 Trọng điểm
        public decimal? RevenueC2TDPlanned { get; set; }
        public decimal? RevenueC2TD { get; set; }
        public decimal? RevenueC2TDRatio { get; set; }

        //Doanh thu C2 Siêu lớn
        public decimal? RevenueC2SLPlanned { get; set; }
        public decimal? RevenueC2SL { get; set; }
        public decimal? RevenueC2SLRatio { get; set; }

        //Doanh thu C2
        public decimal? RevenueC2Planned { get; set; }
        public decimal? RevenueC2 { get; set; }
        public decimal? RevenueC2Ratio { get; set; }

        // Số đại lý trọng điểm mở mới
        public decimal? NewStoreC2CreatedPlanned { get; set; }
        public decimal? NewStoreC2Created { get; set; }
        public decimal? NewStoreC2CreatedRatio { get; set; }

        // Số thông tin phản ánh
        public decimal? TotalProblemPlanned { get; set; }
        public decimal? TotalProblem { get; set; }
        public decimal? TotalProblemRatio { get; set; }

        //Số hình ảnh chụp
        public decimal? TotalImagePlanned { get; set; }
        public decimal? TotalImage { get; set; }
        public decimal? TotalImageRatio { get; set; }

        //Số cửa hàng viếng thăm
        public decimal? StoresVisitedPlanned { get; set; }
        internal HashSet<long> StoreIds { get; set; }
        public decimal? StoresVisited
        {
            get
            {
                if (StoreIds == null) return null;
                return StoreIds.Count;
            }
        }
        public decimal? StoresVisitedRatio { get; set; }

        //Số cửa hàng tạo mới
        public decimal? NewStoreCreatedPlanned { get; set; }
        public decimal? NewStoreCreated { get; set; }
        public decimal? NewStoreCreatedRatio { get; set; }

        //Số lần viếng thăm
        public decimal? NumberOfStoreVisitsPlanned { get; set; }
        public decimal? NumberOfStoreVisits { get; set; }
        public decimal? NumberOfStoreVisitsRatio { get; set; }
    }
}
