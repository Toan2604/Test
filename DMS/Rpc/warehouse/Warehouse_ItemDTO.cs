using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.warehouse
{
    public class Warehouse_ItemDTO : DataDTO
    {

        public long Id { get; set; }

        public long ProductId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }
        public string CodeLower { get; set; }
        public string NameLower { get; set; }

        public string ScanCode { get; set; }

        public decimal? SalePrice { get; set; }

        public decimal? RetailPrice { get; set; }
        public Warehouse_ProductDTO Product { get; set; }

        public Warehouse_ItemDTO() { }
        public Warehouse_ItemDTO(Item Item)
        {

            this.Id = Item.Id;

            this.ProductId = Item.ProductId;

            this.Code = Item.Code;

            this.Name = Item.Name;
            this.NameLower = Item.Name.ToLower();
            this.CodeLower = Item.Code.ToLower();

            this.ScanCode = Item.ScanCode;

            this.SalePrice = Item.SalePrice;

            this.RetailPrice = Item.RetailPrice;

            this.Product = Item.Product == null ? null : new Warehouse_ProductDTO(Item.Product);

            this.Errors = Item.Errors;
        }
    }

    public class Warehouse_ItemFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public IdFilter ProductId { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter ScanCode { get; set; }

        public DecimalFilter SalePrice { get; set; }

        public DecimalFilter RetailPrice { get; set; }
        public string Search { get; set; }

        public ItemOrder OrderBy { get; set; }
    }
}