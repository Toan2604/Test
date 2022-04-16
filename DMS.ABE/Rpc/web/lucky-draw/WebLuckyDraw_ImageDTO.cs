using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    public class WebLuckyDraw_ImageDTO : DataDTO
    {
        
        public long Id { get; set; }
        
        public string Name { get; set; }
        
        public string Url { get; set; }
        
        public string ThumbnailUrl { get; set; }
        
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public WebLuckyDraw_ImageDTO() {}
        public WebLuckyDraw_ImageDTO(Image Image)
        {
            
            this.Id = Image.Id;
            
            this.Name = Image.Name;
            
            this.Url = Image.Url;
            
            this.ThumbnailUrl = Image.ThumbnailUrl;
            
            this.RowId = Image.RowId;
            this.CreatedAt = Image.CreatedAt;
            this.UpdatedAt = Image.UpdatedAt;
            this.Informations = Image.Informations;
            this.Warnings = Image.Warnings;
            this.Errors = Image.Errors;
        }
    }

    public class WebLuckyDraw_ImageFilterDTO : FilterDTO
    {
        
        public IdFilter Id { get; set; }
        
        public StringFilter Name { get; set; }
        
        public StringFilter Url { get; set; }
        
        public StringFilter ThumbnailUrl { get; set; }
        
        public ImageOrder OrderBy { get; set; }
    }
}