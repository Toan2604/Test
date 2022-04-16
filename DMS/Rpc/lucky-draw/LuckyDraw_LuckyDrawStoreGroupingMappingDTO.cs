using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawStoreGroupingMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreGroupingId { get; set; }
        public LuckyDraw_StoreGroupingDTO StoreGrouping { get; set; }   
        public LuckyDraw_LuckyDrawStoreGroupingMappingDTO() {}
        public LuckyDraw_LuckyDrawStoreGroupingMappingDTO(LuckyDrawStoreGroupingMapping LuckyDrawStoreGroupingMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreGroupingMapping.LuckyDrawId;
            this.StoreGroupingId = LuckyDrawStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = LuckyDrawStoreGroupingMapping.StoreGrouping == null ? null : new LuckyDraw_StoreGroupingDTO(LuckyDrawStoreGroupingMapping.StoreGrouping);
            this.Errors = LuckyDrawStoreGroupingMapping.Errors;
        }
    }

    public class LuckyDraw_LuckyDrawStoreGroupingMappingFilterDTO : FilterDTO
    {
        
        public IdFilter LuckyDrawId { get; set; }
        
        public IdFilter StoreGroupingId { get; set; }
        
        public LuckyDrawStoreGroupingMappingOrder OrderBy { get; set; }
    }
}