using DMS.Entities;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item
{
    public class ReportDirectSalesOrderByStoreAndItem_StoreDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public string CodeDraft { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long OrganizationId { get; set; }
        public long StoreTypeId { get; set; }
        public long StoreStatusId { get; set; }
        public long ProvinceId { get; set; }
        public long DistrictId { get; set; }
        public ReportDirectSalesOrderByStoreAndItem_ProvinceDTO Province { get; set; }
        public ReportDirectSalesOrderByStoreAndItem_DistrictDTO District { get; set; }
        public ReportDirectSalesOrderByStoreAndItem_StoreStatusDTO StoreStatus { get; set; }
        public List<ReportDirectSalesOrderByStoreAndItem_ItemDTO> Items { get; set; }
        public ReportDirectSalesOrderByStoreAndItem_StoreDTO() { }
        public ReportDirectSalesOrderByStoreAndItem_StoreDTO(Store Store)
        {
            this.Id = Store.Id;
            this.Code = Store.Code;
            this.CodeDraft = Store.CodeDraft;
            this.Name = Store.Name;
            this.StoreStatusId = Store.StoreStatusId;
            this.Address = Store.Address;
            this.OrganizationId = Store.OrganizationId;
            this.StoreTypeId = Store.StoreTypeId;
            this.Province = Store.Province == null ? null : new ReportDirectSalesOrderByStoreAndItem_ProvinceDTO(Store.Province);
            this.District = Store.District == null ? null : new ReportDirectSalesOrderByStoreAndItem_DistrictDTO(Store.District);
        }
    }

    public class ReportDirectSalesOrderByStoreAndItem_StoreFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }
        public StringFilter CodeDraft { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ParentStoreId { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter StoreTypeId { get; set; }

        public IdFilter StoreGroupingId { get; set; }
        public IdFilter StatusId { get; set; }
    }
}
