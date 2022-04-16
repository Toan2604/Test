using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_SalesOrderTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public PriceList_SalesOrderTypeDTO() { }
        public PriceList_SalesOrderTypeDTO(SalesOrderType SalesOrderType)
        {

            this.Id = SalesOrderType.Id;

            this.Code = SalesOrderType.Code;

            this.Name = SalesOrderType.Name;

            this.Errors = SalesOrderType.Errors;
        }
    }

    public class PriceList_SalesOrderTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public SalesOrderTypeOrder OrderBy { get; set; }
    }
}