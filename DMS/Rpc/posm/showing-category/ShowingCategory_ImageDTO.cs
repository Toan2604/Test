using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_category
{
    public class ShowingCategory_ImageDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public string ThumbnailUrl { get; set; }

        public Guid RowId { get; set; }


        public ShowingCategory_ImageDTO() { }
        public ShowingCategory_ImageDTO(Image Image)
        {

            this.Id = Image.Id;

            this.Name = Image.Name;

            this.Url = Image.Url;

            this.ThumbnailUrl = Image.ThumbnailUrl;

            this.RowId = Image.RowId;

            this.Errors = Image.Errors;
        }
    }

    public class ShowingCategory_ImageFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Url { get; set; }

        public StringFilter ThumbnailUrl { get; set; }

        public GuidFilter RowId { get; set; }

        public ImageOrder OrderBy { get; set; }
    }
}