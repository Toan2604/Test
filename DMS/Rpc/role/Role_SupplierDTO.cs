using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_SupplierDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string TaxCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string OwnerName { get; set; }
        public string Description { get; set; }

        public Role_SupplierDTO() { }
        public Role_SupplierDTO(Supplier Supplier)
        {
            this.Id = Supplier.Id;
            this.Code = Supplier.Code;
            this.Name = Supplier.Name;
            this.TaxCode = Supplier.TaxCode;
            this.Phone = Supplier.Phone;
            this.Email = Supplier.Email;
            this.Address = Supplier.Address;
            this.OwnerName = Supplier.OwnerName;
            this.Description = Supplier.Description;
            this.Errors = Supplier.Errors;
        }
    }

    public class Role_SupplierFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter TaxCode { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter OwnerName { get; set; }
        public StringFilter Description { get; set; }
        public SupplierOrder OrderBy { get; set; }
    }
}
