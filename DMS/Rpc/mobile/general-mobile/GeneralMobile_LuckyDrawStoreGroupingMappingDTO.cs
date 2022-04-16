using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_LuckyDrawStoreGroupingMappingDTO : DataDTO
    {
        public long LuckyDrawId { get; set; }
        public long StoreGroupingId { get; set; }
        public GeneralMobile_StoreGroupingDTO StoreGrouping { get; set; }   
        public GeneralMobile_LuckyDrawStoreGroupingMappingDTO() {}
        public GeneralMobile_LuckyDrawStoreGroupingMappingDTO(LuckyDrawStoreGroupingMapping LuckyDrawStoreGroupingMapping)
        {
            this.LuckyDrawId = LuckyDrawStoreGroupingMapping.LuckyDrawId;
            this.StoreGroupingId = LuckyDrawStoreGroupingMapping.StoreGroupingId;
            this.StoreGrouping = LuckyDrawStoreGroupingMapping.StoreGrouping == null ? null : new GeneralMobile_StoreGroupingDTO(LuckyDrawStoreGroupingMapping.StoreGrouping);
            this.Errors = LuckyDrawStoreGroupingMapping.Errors;
        }
    }

    public class GeneralMobile_LuckyDrawStoreGroupingMappingFilterDTO : FilterDTO
    {        
        public IdFilter LuckyDrawId { get; set; }        
        public IdFilter StoreGroupingId { get; set; }        
        public LuckyDrawStoreGroupingMappingOrder OrderBy { get; set; }
    }
}