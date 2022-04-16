using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order_with_draw
{
    public class ShowingOrderWithDraw_ShowingCategoryDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ParentId { get; set; }

        public string Path { get; set; }

        public long Level { get; set; }

        public long StatusId { get; set; }

        public long? ImageId { get; set; }

        public Guid RowId { get; set; }

        public bool Used { get; set; }


        public ShowingOrderWithDraw_ShowingCategoryDTO() { }
        public ShowingOrderWithDraw_ShowingCategoryDTO(ShowingCategory ShowingCategory)
        {

            this.Id = ShowingCategory.Id;

            this.Code = ShowingCategory.Code;

            this.Name = ShowingCategory.Name;

            this.ParentId = ShowingCategory.ParentId;

            this.Path = ShowingCategory.Path;

            this.Level = ShowingCategory.Level;

            this.StatusId = ShowingCategory.StatusId;

            this.ImageId = ShowingCategory.ImageId;

            this.RowId = ShowingCategory.RowId;

            this.Used = ShowingCategory.Used;

            this.Errors = ShowingCategory.Errors;
        }
    }

    public class ShowingOrderWithDraw_ShowingCategoryFilterDTO : FilterDTO
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

        public ShowingCategoryOrder OrderBy { get; set; }
    }
}