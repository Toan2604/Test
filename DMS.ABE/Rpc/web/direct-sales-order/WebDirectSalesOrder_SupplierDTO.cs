using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_SupplierDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TaxCode { get; set; }
        public long StatusId { get; set; }
        public WebDirectSalesOrder_SupplierDTO() { }
        public WebDirectSalesOrder_SupplierDTO(Supplier Supplier)
        {
            this.Id = Supplier.Id;
            this.Code = Supplier.Code;
            this.Name = Supplier.Name;
            this.TaxCode = Supplier.TaxCode;
            this.StatusId = Supplier.StatusId;
        }
    }
    public class WebDirectSalesOrder_SupplierFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter TaxCode { get; set; }
        public IdFilter StatusId { get; set; }
        public SupplierOrder OrderBy { get; set; }
    }
}
