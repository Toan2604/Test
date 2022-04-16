using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_SupplierDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string TaxCode { get; set; }

        public long StatusId { get; set; }


        public MobileSync_SupplierDTO() { }
        public MobileSync_SupplierDTO(Supplier Supplier)
        {
            this.Id = Supplier.Id;
            this.Code = Supplier.Code;
            this.Name = Supplier.Name;
            this.TaxCode = Supplier.TaxCode;
            this.StatusId = Supplier.StatusId;
        }
    }
}
