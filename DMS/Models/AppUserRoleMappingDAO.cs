using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class AppUserRoleMappingDAO
    {
        /// <summary>
        /// Id nhân viên
        /// </summary>
        public long AppUserId { get; set; }
        /// <summary>
        /// Id nhóm quyền
        /// </summary>
        public long RoleId { get; set; }

        public virtual AppUserDAO AppUser { get; set; }
        public virtual RoleDAO Role { get; set; }
    }
}
