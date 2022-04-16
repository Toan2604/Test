using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.MLuckyDrawRegistration
{
    public class LuckyDrawRegistrationMessage
    {
        public enum Information
        {

        }

        public enum Warning
        {

        }

        public enum Error
        {
            IdNotExisted,
            TimeEmpty,
            AppUserEmpty,
            AppUserNotExisted,
            LuckyDrawEmpty,
            LuckyDrawNotExisted,
            PrizesInsufficient,
            RevenueInvalid,
            RevenueInsufficient,
            StoreEmpty,
            StoreNotInScoped,
            StoreNotExisted,
            StoreUserNotExisted
        }
    }
}
