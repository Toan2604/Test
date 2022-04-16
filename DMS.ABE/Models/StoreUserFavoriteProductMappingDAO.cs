using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreUserFavoriteProductMappingDAO
    {
        public long FavoriteProductId { get; set; }
        public long StoreUserId { get; set; }

        public virtual ProductDAO FavoriteProduct { get; set; }
        public virtual StoreUserDAO StoreUser { get; set; }
    }
}
