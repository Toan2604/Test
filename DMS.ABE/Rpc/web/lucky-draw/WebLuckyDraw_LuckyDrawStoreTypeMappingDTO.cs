using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    public class WebLuckyDraw_LuckyDrawStoreTypeMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreTypeId { get; set; }
        public WebLuckyDraw_LuckyDrawStoreTypeMappingDTO() {}
        public WebLuckyDraw_LuckyDrawStoreTypeMappingDTO(LuckyDrawStoreTypeMapping LuckyDrawStoreTypeMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreTypeMapping.LuckyDrawId;
            this.StoreTypeId = LuckyDrawStoreTypeMapping.StoreTypeId;
            this.Errors = LuckyDrawStoreTypeMapping.Errors;
        }
    }

    public class WebLuckyDraw_LuckyDrawStoreTypeMappingFilterDTO : FilterDTO
    {
        
        public IdFilter LuckyDrawId { get; set; }
        
        public IdFilter StoreTypeId { get; set; }
        
        public LuckyDrawStoreTypeMappingOrder OrderBy { get; set; }
    }
}