using System;
using System.Collections.Generic;

namespace DMS.DWModels
{
    public partial class Dim_BrandDAO
    {
        /// <summary>
        /// Id
        /// </summary>
        public long BrandId { get; set; }
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
        public Guid RowId { get; set; }
    }
}
