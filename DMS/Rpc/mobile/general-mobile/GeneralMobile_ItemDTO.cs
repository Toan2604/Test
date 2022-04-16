using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_ItemDTO : DataDTO
    {
        public long Id { get; set; }
        public long ProductId { get; set; }
        public string Code { get; set; }
        public string ERPCode { get; set; }
        public string Name { get; set; }
        public string ScanCode { get; set; }
        public decimal? SalePrice { get; set; }
        public decimal? RetailPrice { get; set; }
        public decimal SaleStock { get; set; }
        public long StatusId { get; set; }
        public bool HasInventory { get; set; }
        public DateTime? LastUpdateInventory { get; set; }
        public GeneralMobile_ProductDTO Product { get; set; }
        public List<GeneralMobile_ImageDTO> Images { get; set; }
        public List<GeneralMobile_InventoryDTO> Inventories { get; set; }
        public GeneralMobile_ItemDTO() { }
        public GeneralMobile_ItemDTO(Item Item)
        {
            this.Id = Item.Id;
            this.ProductId = Item.ProductId;
            this.Code = Item.Code;
            this.ERPCode = Item.ERPCode;
            this.Name = Item.Name;
            this.ScanCode = Item.ScanCode;
            this.SalePrice = Item.SalePrice;
            this.RetailPrice = Item.RetailPrice;
            this.SaleStock = Item.SaleStock;
            this.StatusId = Item.StatusId;
            this.HasInventory = Item.HasInventory;
            this.LastUpdateInventory = Item.LastUpdateInventory;
            this.Product = Item.Product == null ? null : new GeneralMobile_ProductDTO(Item.Product);
            this.Images = Item.ItemImageMappings?.Where(iim => iim.Image != null).Select(iim => new GeneralMobile_ImageDTO(iim.Image)).ToList();
            this.Inventories = Item.Inventories?.Select(iv => new GeneralMobile_InventoryDTO(iv)).ToList();
            this.Errors = Item.Errors;
        }
    }

    public class GeneralMobile_ItemFilterDTO : FilterDTO
    {
        public string Search { get; set; }
        public IdFilter Id { get; set; }
        public IdFilter ProductId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public StringFilter OtherName { get; set; }
        public StringFilter ScanCode { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public DecimalFilter RetailPrice { get; set; }
        public IdFilter ProductGroupingId { get; set; }
        public IdFilter ProductTypeId { get; set; }
        public IdFilter SupplierId { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter BrandId { get; set; }
        public IdFilter CategoryId { get; set; }
        public bool? IsNew { get; set; }
        public ItemOrder OrderBy { get; set; }
    }
}
