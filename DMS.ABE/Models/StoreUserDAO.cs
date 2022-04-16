using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class StoreUserDAO
    {
        public StoreUserDAO()
        {
            DirectSalesOrders = new HashSet<DirectSalesOrderDAO>();
            StoreUserFavoriteProductMappings = new HashSet<StoreUserFavoriteProductMappingDAO>();
        }

        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string OtpCode { get; set; }
        public DateTime? OtpExpired { get; set; }
        public long StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrders { get; set; }
        public virtual ICollection<StoreUserFavoriteProductMappingDAO> StoreUserFavoriteProductMappings { get; set; }
    }
}
