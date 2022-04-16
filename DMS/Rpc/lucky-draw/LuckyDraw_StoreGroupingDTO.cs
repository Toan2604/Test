using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_StoreGroupingDTO : DataDTO
    {
        
        public long Id { get; set; }
        
        public string Code { get; set; }
        
        public string Name { get; set; }
        
        public long? ParentId { get; set; }
        
        public string Path { get; set; }
        
        public long Level { get; set; }
        
        public long StatusId { get; set; }
        
        public bool Used { get; set; }
        
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public LuckyDraw_StoreGroupingDTO() {}
        public LuckyDraw_StoreGroupingDTO(StoreGrouping StoreGrouping)
        {
            
            this.Id = StoreGrouping.Id;
            
            this.Code = StoreGrouping.Code;
            
            this.Name = StoreGrouping.Name;
            
            this.ParentId = StoreGrouping.ParentId;
            
            this.Path = StoreGrouping.Path;
            
            this.Level = StoreGrouping.Level;
            
            this.StatusId = StoreGrouping.StatusId;
            
            this.Used = StoreGrouping.Used;
            
            this.RowId = StoreGrouping.RowId;
            this.CreatedAt = StoreGrouping.CreatedAt;
            this.UpdatedAt = StoreGrouping.UpdatedAt;
            this.Informations = StoreGrouping.Informations;
            this.Warnings = StoreGrouping.Warnings;
            this.Errors = StoreGrouping.Errors;
        }
    }

    public class LuckyDraw_StoreGroupingFilterDTO : FilterDTO
    {
        
        public IdFilter Id { get; set; }
        
        public StringFilter Code { get; set; }
        
        public StringFilter Name { get; set; }
        
        public IdFilter ParentId { get; set; }
        
        public StringFilter Path { get; set; }
        
        public LongFilter Level { get; set; }
        
        public IdFilter StatusId { get; set; }
        
        public StoreGroupingOrder OrderBy { get; set; }
    }
}