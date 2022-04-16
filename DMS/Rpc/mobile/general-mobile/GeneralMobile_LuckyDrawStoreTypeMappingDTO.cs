using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyDrawStoreTypeMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreTypeId { get; set; }
        public GeneralMobile_StoreTypeDTO StoreType { get; set; }   
        public GeneralMobile_LuckyDrawStoreTypeMappingDTO() {}
        public GeneralMobile_LuckyDrawStoreTypeMappingDTO(LuckyDrawStoreTypeMapping LuckyDrawStoreTypeMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreTypeMapping.LuckyDrawId;
            this.StoreTypeId = LuckyDrawStoreTypeMapping.StoreTypeId;
            this.StoreType = LuckyDrawStoreTypeMapping.StoreType == null ? null : new GeneralMobile_StoreTypeDTO(LuckyDrawStoreTypeMapping.StoreType);
            this.Errors = LuckyDrawStoreTypeMapping.Errors;
        }
    }

    public class GeneralMobile_LuckyDrawStoreTypeMappingFilterDTO : FilterDTO
    {
        
        public IdFilter LuckyDrawId { get; set; }
        
        public IdFilter StoreTypeId { get; set; }
        
        public LuckyDrawStoreTypeMappingOrder OrderBy { get; set; }
    }
}