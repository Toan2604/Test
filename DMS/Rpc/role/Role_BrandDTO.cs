using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.role
{
    public class Role_BrandDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public DateTime UpdateTime { get; set; }
        public Role_StatusDTO Status { get; set; }
        public Role_BrandDTO() { }
        public Role_BrandDTO(Brand Brand)
        {
            this.Id = Brand.Id;
            this.Code = Brand.Code;
            this.Name = Brand.Name;
            this.Description = Brand.Description;
            this.StatusId = Brand.StatusId;
            this.UpdateTime = Brand.UpdateTime;
            this.Status = Brand.Status == null ? null : new Role_StatusDTO(Brand.Status);
            this.Errors = Brand.Errors;
        }
    }

    public class Role_BrandFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter UpdateTime { get; set; }
        public BrandOrder OrderBy { get; set; }
    }
}
