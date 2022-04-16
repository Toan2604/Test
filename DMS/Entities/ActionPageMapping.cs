using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ActionPageMapping : DataEntity
    {
        public long ActionId { get; set; }
        public long PageId { get; set; }
    }
}
