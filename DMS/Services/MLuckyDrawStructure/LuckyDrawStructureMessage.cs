using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Services.MLuckyDrawStructure
{
    public class LuckyDrawStructureMessage
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
            NameEmpty,
            NameOverLength,
            ValueEmpty,
            ValueOverLength,
            LuckyDrawEmpty,
            LuckyDrawNotExisted,
        }
    }
}
