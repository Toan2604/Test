using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawStoreGroupingMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreGroupingId { get; set; }
        public LuckyDraw_LuckyDrawStoreGroupingMappingDTO() {}
        public LuckyDraw_LuckyDrawStoreGroupingMappingDTO(LuckyDrawStoreGroupingMapping LuckyDrawStoreGroupingMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreGroupingMapping.LuckyDrawId;
            this.StoreGroupingId = LuckyDrawStoreGroupingMapping.StoreGroupingId;
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