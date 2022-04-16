using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.survey
{
    public class Survey_ImageDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public Guid? RowId { get; set; }


        public Survey_ImageDTO() { }
        public Survey_ImageDTO(Image Image)
        {

            this.Id = Image.Id;

            this.Name = Image.Name;

            this.Url = Image.Url;

            this.Errors = Image.Errors;
        }
    }

    public class Survey_ImageFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Url { get; set; }

        public ImageOrder OrderBy { get; set; }
    }
}