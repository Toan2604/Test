using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_StoreStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public DirectSalesOrder_StoreStatusDTO() { }
        public DirectSalesOrder_StoreStatusDTO(StoreStatus StoreStatus)
        {

            this.Id = StoreStatus.Id;

            this.Code = StoreStatus.Code;

            this.Name = StoreStatus.Name;

        }
    }

    public class DirectSalesOrder_StoreStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}