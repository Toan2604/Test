using System;
using System.Collections.Generic;

namespace DMS.Models
{
    /// <summary>
    /// B&#7843;ng &#273;&#7883;nh ngh&#297;a c&#225;c c&#7917;a h&#224;ng thu&#7897;c v&#249;ng &#273;i tuy&#7871;n
    /// </summary>
    public partial class AppUserStoreMappingDAO
    {
        public long AppUserId { get; set; }
        public long StoreId { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
