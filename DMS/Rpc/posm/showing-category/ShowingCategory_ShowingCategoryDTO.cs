using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_category
{
    public class ShowingCategory_ShowingCategoryDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public long? ImageId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public ShowingCategory_ImageDTO Image { get; set; }
        public ShowingCategory_ShowingCategoryDTO Parent { get; set; }
        public ShowingCategory_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ShowingCategory_ShowingCategoryDTO() { }
        public ShowingCategory_ShowingCategoryDTO(ShowingCategory ShowingCategory)
        {
            this.Id = ShowingCategory.Id;
            this.Code = ShowingCategory.Code;
            this.Description = ShowingCategory.Description;
            this.Name = ShowingCategory.Name;
            this.ParentId = ShowingCategory.ParentId;
            this.Path = ShowingCategory.Path;
            this.Level = ShowingCategory.Level;
            this.StatusId = ShowingCategory.StatusId;
            this.ImageId = ShowingCategory.ImageId;
            this.RowId = ShowingCategory.RowId;
            this.Used = ShowingCategory.Used;
            this.Image = ShowingCategory.Image == null ? null : new ShowingCategory_ImageDTO(ShowingCategory.Image);
            this.Parent = ShowingCategory.Parent == null ? null : new ShowingCategory_ShowingCategoryDTO(ShowingCategory.Parent);
            this.Status = ShowingCategory.Status == null ? null : new ShowingCategory_StatusDTO(ShowingCategory.Status);
            this.CreatedAt = ShowingCategory.CreatedAt;
            this.UpdatedAt = ShowingCategory.UpdatedAt;
            this.Errors = ShowingCategory.Errors;
        }
    }

    public class ShowingCategory_ShowingCategoryFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ParentId { get; set; }
        public StringFilter Path { get; set; }
        public LongFilter Level { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter ImageId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public ShowingCategoryOrder OrderBy { get; set; }
    }
}
