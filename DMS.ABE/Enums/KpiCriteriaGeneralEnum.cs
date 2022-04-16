using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;

namespace DMS.ABE.Enums
{
    public class KpiCriteriaGeneralEnum
    {
        #region các chỉ tiêu tạm ẩn
        //public static GenericEnum TOTAL_INDIRECT_SALES_ORDER = new GenericEnum { Id = 1, Code = "TotalIndirectOrders", Name = "Số đơn hàng gián tiếp" };
        //public static GenericEnum TOTAL_INDIRECT_SALES_QUANTITY = new GenericEnum { Id = 2, Code = "TotalIndirectQuantity", Name = "Tổng sản lượng đơn hàng gián tiếp" };
        //public static GenericEnum SKU_INDIRECT_SALES_ORDER = new GenericEnum { Id = 4, Code = "SKUIndirectOrder", Name = "SKU/ Đơn hàng gián tiếp" };
        //public static GenericEnum TOTAL_DIRECT_SALES_ORDER = new GenericEnum { Id = 8, Code = "TotalDirectOrders", Name = "Số đơn hàng trực tiếp" };
        //public static GenericEnum TOTAL_DIRECT_SALES_QUANTITY = new GenericEnum { Id = 9, Code = "TotalDirectQuantity", Name = "Tổng sản lượng đơn hàng trực tiếp" };
        //public static GenericEnum TOTAL_DIRECT_SALES_AMOUNT = new GenericEnum { Id = 10, Code = "TotalDirectSalesAmount", Name = "Doanh thu đơn hàng trực tiếp" };
        //public static GenericEnum SKU_DIRECT_SALES_ORDER = new GenericEnum { Id = 11, Code = "SKUDirectOrder", Name = "SKU/ Đơn hàng trực tiếp" };
        #endregion
        public static GenericEnum TOTAL_INDIRECT_SALES_AMOUNT = new GenericEnum { Id = 3, Code = "TotalIndirectSalesAmount", Name = "Tổng doanh thu đơn hàng" };
        public static GenericEnum STORE_VISITED = new GenericEnum { Id = 5, Code = "StoresVisited", Name = "Số đại lý ghé thăm" };
        public static GenericEnum REVENUE_C2_TD = new GenericEnum { Id = 12, Code = "RevenueC2TD", Name = "Doanh thu C2 Trọng điểm" };
        public static GenericEnum REVENUE_C2_SL = new GenericEnum { Id = 13, Code = "RevenueC2SL", Name = "Doanh thu C2 Siêu lớn" };
        public static GenericEnum REVENUE_C2 = new GenericEnum { Id = 14, Code = "RevenueC2", Name = "Doanh thu C2" };
        public static GenericEnum NEW_STORE_CREATED = new GenericEnum { Id = 6, Code = "NewStoresCreated", Name = "Tổng số đại lý mở mới" };
        public static GenericEnum NEW_STORE_C2_CREATED = new GenericEnum { Id = 15, Code = "NewStoresC2Created", Name = "Số đại lý trọng điểm mở mới" };
        public static GenericEnum NUMBER_OF_STORE_VISIT = new GenericEnum { Id = 7, Code = "NumberOfStoreVisits", Name = "Tổng số lượt ghé thăm" };
        public static GenericEnum TOTAL_PROBLEM = new GenericEnum { Id = 16, Code = "TotalProblem", Name = "Số thông tin phản ánh" };
        public static GenericEnum TOTAL_IMAGE = new GenericEnum { Id = 17, Code = "TotalImage", Name = "Số hình ảnh chụp" };

        public static List<GenericEnum> KpiCriteriaGeneralEnumList = new List<GenericEnum>()
        {
            #region các chỉ tiêu tạm ẩn
            //TOTAL_INDIRECT_SALES_ORDER,
            //TOTAL_INDIRECT_SALES_QUANTITY,
            //SKU_INDIRECT_SALES_ORDER,
            //TOTAL_DIRECT_SALES_ORDER,
            //TOTAL_DIRECT_SALES_QUANTITY,
            //TOTAL_DIRECT_SALES_AMOUNT,
            //SKU_DIRECT_SALES_ORDER,
#endregion
            TOTAL_INDIRECT_SALES_AMOUNT,
            REVENUE_C2_TD,
            REVENUE_C2_SL,
            REVENUE_C2,
            STORE_VISITED,
            NEW_STORE_CREATED,
            NEW_STORE_C2_CREATED,
            NUMBER_OF_STORE_VISIT,
            TOTAL_PROBLEM,
            TOTAL_IMAGE
        };
    }
}
