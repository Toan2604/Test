using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyDrawStoreMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreId { get; set; }
        public GeneralMobile_StoreDTO Store { get; set; }   
        public GeneralMobile_LuckyDrawStoreMappingDTO() {}
        public GeneralMobile_LuckyDrawStoreMappingDTO(LuckyDrawStoreMapping LuckyDrawStoreMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreMapping.LuckyDrawId;
            this.StoreId = LuckyDrawStoreMapping.StoreId;
            this.Store = LuckyDrawStoreMapping.Store == null ? null : new GeneralMobile_StoreDTO(LuckyDrawStoreMapping.Store);
            this.Errors = LuckyDrawStoreMapping.Errors;
        }
    }

    public class GeneralMobile_LuckyDrawStoreMappingFilterDTO : FilterDTO
    {        
        public IdFilter LuckyDrawId { get; set; }        
        public IdFilter StoreId { get; set; }        
        public LuckyDrawStoreMappingOrder OrderBy { get; set; }
    }
}