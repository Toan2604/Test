using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_ProductDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string SupplierCode { get; set; }
        public string ERPCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ScanCode { get; set; }
        public string OtherName { get; set; }
        public string TechnicalName { get; set; }

        public Role_ProductDTO() { }
        public Role_ProductDTO(Product Product)
        {
            this.Id = Product.Id;
            this.Code = Product.Code;
            this.SupplierCode = Product.SupplierCode;
            this.ERPCode = Product.ERPCode;
            this.Name = Product.Name;
            this.Description = Product.Description;
            this.ScanCode = Product.ScanCode;
            this.OtherName = Product.OtherName;
            this.TechnicalName = Product.TechnicalName;
            this.Errors = Product.Errors;
        }
    }

    public class Role_ProductFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter SupplierCode { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public StringFilter ScanCode { get; set; }
        public StringFilter OtherName { get; set; }
        public StringFilter TechnicalName { get; set; }
        public ProductOrder OrderBy { get; set; }
    }
}
