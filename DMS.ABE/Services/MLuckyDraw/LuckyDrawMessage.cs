using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MLuckyDraw
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
            NameEmpty,
            OrganizationEmpty,
            OrganizationNotExisted,
            PrizeOver,
            RevenuePerTurnInvalid,
            StoreNotRegistered,
            StoreOverTurn,
            StartAtEmpty,
            StartAtInvalid,
            StatusEmpty,
            StatusNotExisted,
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
            LuckyDrawWinner_LuckyDrawStructureEmpty,
            LuckyDrawWinner_LuckyDrawStructureNotExisted,
        }
    }
}
