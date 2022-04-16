using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_ResellerDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string TaxCode { get; set; }
        public string CompanyName { get; set; }
        public string DeputyName { get; set; }
        public string Description { get; set; }
        public Role_ResellerDTO() { }
        public Role_ResellerDTO(Reseller Reseller)
        {
            this.Id = Reseller.Id;
            this.Code = Reseller.Code;
            this.Name = Reseller.Name;
            this.Phone = Reseller.Phone;
            this.Email = Reseller.Email;
            this.Address = Reseller.Address;
            this.TaxCode = Reseller.TaxCode;
            this.CompanyName = Reseller.CompanyName;
            this.DeputyName = Reseller.DeputyName;
            this.Description = Reseller.Description;
            this.Errors = Reseller.Errors;
        }
    }

    public class Role_ResellerFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Phone { get; set; }
        public StringFilter Email { get; set; }
        public StringFilter Address { get; set; }
        public StringFilter TaxCode { get; set; }
        public StringFilter CompanyName { get; set; }
        public StringFilter DeputyName { get; set; }
        public StringFilter Description { get; set; }
        public ResellerOrder OrderBy { get; set; }
    }
}
