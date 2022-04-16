using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.kpi_tracking.kpi_general_period_report
{
    public class KpiGeneralPeriodReport_ExportDTO : DataDTO
    {
        public string OrganizationName { get; set; }
        public List<KpiGeneralPeriodReport_UserDTO> Users { get; set; }
        public KpiGeneralPeriodReport_ExportDTO() { }
        public KpiGeneralPeriodReport_ExportDTO(KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO)
        {
            this.OrganizationName = KpiGeneralPeriodReport_KpiGeneralPeriodReportDTO.OrganizationName;
        }
    }

    public class KpiGeneralPeriodReport_CriteriaContentDTO : DataDTO
    {
        public long CriteriaId { get; set; }
        public string CriteriaName { get; set; }
        public decimal? Planned { get; set; }
        public decimal? Result { get; set; }
        public decimal? Ratio { get; set; }

    }

    public class KpiGeneralPeriodReport_UserDTO : DataDTO
    {
        public long STT { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string OrganizationName { get; set; }
        public List<KpiGeneralPeriodReport_CriteriaContentDTO> CriteriaContents { get; set; }

        #region các chỉ tiêu tạm ẩn
        ////Số đơn hàng gián tiếp
        //public string TotalIndirectOrdersPLanned { get; set; }
        //public string TotalIndirectOrders { get; set; }
        //public string TotalIndirectOrdersRatio { get; set; }

        ////Tổng sản lượng theo đơn hàng gián tiếp
        //public string TotalIndirectQuantityPlanned { get; set; }
        //public string TotalIndirectQuantity { get; set; }
        //public string TotalIndirectQuantityRatio { get; set; }

        ////SKU gián tiếp
        //public string SkuIndirectOrderPlanned { get; set; }
        //public string SkuIndirectOrder { get; set; }
        //public string SkuIndirectOrderRatio { get; set; }

        ////Số đơn hàng trực tiếp
        //public string TotalDirectOrdersPLanned { get; set; }
        //public string TotalDirectOrders { get; set; }
        //public string TotalDirectOrdersRatio { get; set; }

        ////Tổng sản lượng theo đơn hàng trực tiếp
        //public string TotalDirectQuantityPlanned { get; set; }
        //public string TotalDirectQuantity { get; set; }
        //public string TotalDirectQuantityRatio { get; set; }

        ////Doanh thu theo đơn hàng trực tiếp
        //public string TotalDirectSalesAmountPlanned { get; set; }
        //public string TotalDirectSalesAmount { get; set; }
        //public string TotalDirectSalesAmountRatio { get; set; }

        ////SKU trực tiếp
        //public string SkuDirectOrderPlanned { get; set; }
        //public string SkuDirectOrder { get; set; }
        //public string SkuDirectOrderRatio { get; set; }
        #endregion

        public KpiGeneralPeriodReport_UserDTO() { }
       
    }
}
