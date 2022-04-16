using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.MLuckyDraw
{
    public class LuckyDrawMessage
    {
        public enum Information
        {

        }

        public enum Warning
        {

        }

        public enum Error
        {
            AvatarImageEmpty,
            AvatarImageNotExisted,
            CodeEmpty,
            CodeExisted,
            CodeHasSpecialCharacter,
            DescriptionEmpty,
            EndAtEmpty,
            EndAtInvalid,
            IdNotExisted,
            ImageEmpty,
            ImageNotExisted,
            LuckyDrawTypeEmpty,
            LuckyDrawTypeNotExisted,
            LuckyDrawStructuresEmpty,
            MustDrawnByStore,
            NameEmpty,
            PrizeOver,
            OrganizationEmpty,
            OrganizationNotExisted,
            RevenuePerTurnInvalid,
            StartAtEmpty,
            StartAtInvalid,
            StatusEmpty,
            StatusNotExisted,
            StoreNotInScoped,
            StoreOverTurn,
            LuckyDrawStoreGroupingMapping_StoreGroupingEmpty,
            LuckyDrawStoreGroupingMapping_StoreGroupingNotExisted,
            LuckyDrawStoreMapping_StoreEmpty,
            LuckyDrawStoreMapping_StoreNotExisted,
            LuckyDrawStoreTypeMapping_StoreTypeEmpty,
            LuckyDrawStoreTypeMapping_StoreTypeNotExisted,
            LuckyDrawStructure_QuantityInvalid,
            LuckyDrawStructure_NameEmpty,
            LuckyDrawStructure_NameOverLength,
            LuckyDrawStructure_ValueEmpty,
            LuckyDrawUsed,
            LuckyDrawWinner_LuckyDrawStructureEmpty,
            LuckyDrawWinner_LuckyDrawStructureNotExisted,
        }
    }
}
