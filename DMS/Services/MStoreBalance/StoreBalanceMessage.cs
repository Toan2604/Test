using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.MStoreBalance
{
    public class StoreBalanceMessage
    {
        public enum Information
        {

        }

        public enum Warning
        {

        }

        public enum Error
        {
            CreditAmountInvalid,
            CreditAmountEmpty,
            DebitAmountInvalid,
            DebitAmountEmpty,
            IdNotExisted,
            OrganizationEmpty,
            OrganizationNotExisted,
            StoreEmpty,
            StoreBalanceExisted,
            StoreNotExisted,
            UpdateNotAllowed,
        }
    }
}
