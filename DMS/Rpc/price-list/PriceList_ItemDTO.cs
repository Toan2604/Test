using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_ItemDTO : DataDTO
    {

        public long Id { get; set; }

        public long ProductId { get; set; }

        public string Code { get; set; }
        public string ERPCode { get; set; }

        public string Name { get; set; }

        public string ScanCode { get; set; }
        public string OtherName { get; set; }

        public decimal? SalePrice { get; set; }

        public decimal? RetailPrice { get; set; }

        public long StatusId { get; set; }

        public PriceList_ProductDTO Product { get; set; }
        public PriceList_ItemDTO() { }
        public PriceList_ItemDTO(Item Item)
        {

            this.Id = Item.Id;
            this.ProductId = Item.ProductId;
            this.Code = Item.Code;
            this.ERPCode = Item.ERPCode;
            this.Name = Item.Name;
            this.ScanCode = Item.ScanCode;
            this.OtherName = Item.Product?.OtherName;
            this.SalePrice = Item.SalePrice;
            this.RetailPrice = Item.RetailPrice;
            this.StatusId = Item.StatusId;
            this.Product = Item.Product == null ? null : new PriceList_ProductDTO(Item.Product);
            this.Errors = Item.Errors;
        }
    }

    public class PriceList_ItemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter ProductId { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter ERPCode { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter ScanCode { get; set; }
        public StringFilter OtherName { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter RetailPrice { get; set; }
        public IdFilter StatusId { get; set; }
        public string Search { get; set; }

        public ItemOrder OrderBy { get; set; }
    }
}