using TrueSight.Common;
using DMS.ABE.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    public class WebLuckyDraw_LuckyDrawStoreMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreId { get; set; }
        public WebLuckyDraw_StoreDTO Store { get; set; }   
        public WebLuckyDraw_LuckyDrawStoreMappingDTO() {}
        public WebLuckyDraw_LuckyDrawStoreMappingDTO(LuckyDrawStoreMapping LuckyDrawStoreMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreMapping.LuckyDrawId;
            this.StoreId = LuckyDrawStoreMapping.StoreId;
            this.Store = LuckyDrawStoreMapping.Store == null ? null : new WebLuckyDraw_StoreDTO(LuckyDrawStoreMapping.Store);
            this.Errors = LuckyDrawStoreMapping.Errors;
        }
    }

    public class WebLuckyDraw_LuckyDrawStoreMappingFilterDTO : FilterDTO
    {        
        public IdFilter LuckyDrawId { get; set; }        
        public IdFilter StoreId { get; set; }        
        public LuckyDrawStoreMappingOrder OrderBy { get; set; }
    }
}