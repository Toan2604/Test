using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_item
{
    public class ShowingItem_ShowingItemDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long ShowingCategoryId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public decimal SalePrice { get; set; }
        public string ERPCode { get; set; }
        public string Description { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }
        public ShowingItem_ShowingCategoryDTO ShowingCategory { get; set; }
        public ShowingItem_StatusDTO Status { get; set; }
        public ShowingItem_UnitOfMeasureDTO UnitOfMeasure { get; set; }
        public List<ShowingItem_ShowingItemImageMappingDTO> ShowingItemImageMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ShowingItem_ShowingItemDTO() { }
        public ShowingItem_ShowingItemDTO(ShowingItem ShowingItem)
        {
            this.Id = ShowingItem.Id;
            this.Code = ShowingItem.Code;
            this.Name = ShowingItem.Name;
            this.ShowingCategoryId = ShowingItem.ShowingCategoryId;
            this.UnitOfMeasureId = ShowingItem.UnitOfMeasureId;
            this.SalePrice = ShowingItem.SalePrice;
            this.ERPCode = ShowingItem.ERPCode;
            this.Description = ShowingItem.Description;
            this.StatusId = ShowingItem.StatusId;
            this.Used = ShowingItem.Used;
            this.RowId = ShowingItem.RowId;
            this.ShowingCategory = ShowingItem.ShowingCategory == null ? null : new ShowingItem_ShowingCategoryDTO(ShowingItem.ShowingCategory);
            this.Status = ShowingItem.Status == null ? null : new ShowingItem_StatusDTO(ShowingItem.Status);
            this.UnitOfMeasure = ShowingItem.UnitOfMeasure == null ? null : new ShowingItem_UnitOfMeasureDTO(ShowingItem.UnitOfMeasure);
            this.ShowingItemImageMappings = ShowingItem.ShowingItemImageMappings?.Select(x => new ShowingItem_ShowingItemImageMappingDTO(x)).ToList();
            this.CreatedAt = ShowingItem.CreatedAt;
            this.UpdatedAt = ShowingItem.UpdatedAt;
            this.Errors = ShowingItem.Errors;
        }
    }

    public class ShowingItem_ShowingItemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ShowingCategoryId { get; set; }
        public IdFilter UnitOfMeasureId { get; set; }
        public DecimalFilter SalePrice { get; set; }
        public StringFilter ERPCode { get; set; }
        public StringFilter Description { get; set; }
        public IdFilter StatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public string Search { get; set; }
        public ShowingItemOrder OrderBy { get; set; }
    }
}
