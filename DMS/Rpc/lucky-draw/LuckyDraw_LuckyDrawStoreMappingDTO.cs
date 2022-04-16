using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.lucky_draw
{
    public class LuckyDraw_LuckyDrawStoreMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreId { get; set; }
        public LuckyDraw_StoreDTO Store { get; set; }   
        public LuckyDraw_LuckyDrawStoreMappingDTO() {}
        public LuckyDraw_LuckyDrawStoreMappingDTO(LuckyDrawStoreMapping LuckyDrawStoreMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreMapping.LuckyDrawId;
            this.StoreId = LuckyDrawStoreMapping.StoreId;
            this.Store = LuckyDrawStoreMapping.Store == null ? null : new LuckyDraw_StoreDTO(LuckyDrawStoreMapping.Store);
            this.Errors = LuckyDrawStoreMapping.Errors;
        }
    }

    public class LuckyDraw_LuckyDrawStoreMappingFilterDTO : FilterDTO
    {
        
        public IdFilter LuckyDrawId { get; set; }
        
        public IdFilter StoreId { get; set; }
        
        public LuckyDrawStoreMappingOrder OrderBy { get; set; }
    }
}