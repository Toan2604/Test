using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_ShowingCategoryDTO : DataDTO
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ParentId { get; set; }

        public string Path { get; set; }

        public string Description { get; set; }


        public GeneralMobile_ShowingCategoryDTO() { }
        public GeneralMobile_ShowingCategoryDTO(ShowingCategory ShowingCategory)
        {

            this.Id = ShowingCategory.Id;

            this.Code = ShowingCategory.Code;

            this.Name = ShowingCategory.Name;

            this.ParentId = ShowingCategory.ParentId;

            this.Path = ShowingCategory.Path;

            this.Description = ShowingCategory.Description;

        }
    }

    public class GeneralMobile_ShowingCategoryFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ParentId { get; set; }

        public StringFilter Path { get; set; }

        public StringFilter Description { get; set; }

        public ShowingCategoryOrder OrderBy { get; set; }
    }
}
