using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_PriceListTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public PriceList_PriceListTypeDTO() { }
        public PriceList_PriceListTypeDTO(PriceListType PriceListType)
        {

            this.Id = PriceListType.Id;

            this.Code = PriceListType.Code;

            this.Name = PriceListType.Name;

            this.Errors = PriceListType.Errors;
        }
    }

    public class PriceList_PriceListTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public PriceListTypeOrder OrderBy { get; set; }
    }
}