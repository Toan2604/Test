using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class BrandDAO
    {
        public BrandDAO()
        {
            BrandInStores = new HashSet<BrandInStoreDAO>();
            Products = new HashSet<ProductDAO>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Mã nhãn hiệu
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên nhãn nhiệu
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public long StatusId { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        public DateTime UpdatedAt { get; set; }
        /// <summary>
        /// Ngày xoá
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public bool Used { get; set; }
        public Guid RowId { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<BrandInStoreDAO> BrandInStores { get; set; }
        public virtual ICollection<ProductDAO> Products { get; set; }
    }
}
