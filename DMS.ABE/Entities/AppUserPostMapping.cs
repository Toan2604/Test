using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.ABE.Entities
{
    public class AppUserPostMapping : DataEntity
    {
        public long AppUserId { get; set; }
        public long PostId { get; set; }
        public AppUser AppUser { get; set; }
        public Post Post { get; set; }
    }
}
