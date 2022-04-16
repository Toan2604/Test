using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.MLuckyDrawWinner
{
    public class LuckyDrawWinnerMessage
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
            AppUserEmpty,
            AppUserNotExisted,
            LuckyDrawEmpty,
            LuckyDrawNotExisted,
            LuckyDrawStructureEmpty,
            LuckyDrawStructureNotExisted,
        }
    }
}
