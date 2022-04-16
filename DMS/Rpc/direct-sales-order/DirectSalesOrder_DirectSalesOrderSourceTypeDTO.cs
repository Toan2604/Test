using DMS.Common; 
using TrueSight.Common;
using DMS.Entities;

namespace DMS.Rpc.direct_sales_order
{
    public class DirectSalesOrder_DirectSalesOrderSourceTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public DirectSalesOrder_DirectSalesOrderSourceTypeDTO() { }
        public DirectSalesOrder_DirectSalesOrderSourceTypeDTO(DirectSalesOrderSourceType DirectSalesOrderSourceType)
        {

            this.Id = DirectSalesOrderSourceType.Id;

            this.Code = DirectSalesOrderSourceType.Code;

            this.Name = DirectSalesOrderSourceType.Name;

            this.Errors = DirectSalesOrderSourceType.Errors;
        }
    }

    public class DirectSalesOrder_DirectSalesOrderSourceTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public DirectSalesOrderSourceTypeOrder OrderBy { get; set; }
    }
}