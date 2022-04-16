using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class DistrictDAO
    {
        public DistrictDAO()
        {
            StoreScoutings = new HashSet<StoreScoutingDAO>();
            Stores = new HashSet<StoreDAO>();
            Suppliers = new HashSet<SupplierDAO>();
            Warehouses = new HashSet<WarehouseDAO>();
        }

        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Mã quận huyện
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên quận huyện
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Thứ tự ưu tiên
        /// </summary>
        public long? Priority { get; set; }
        /// <summary>
        /// Tỉnh phụ thuộc
        /// </summary>
        public long ProvinceId { get; set; }
        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        public long StatusId { get; set; }
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
        /// <summary>
        /// Trường để đồng bộ
        /// </summary>
        public Guid RowId { get; set; }

        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<StoreScoutingDAO> StoreScoutings { get; set; }
        public virtual ICollection<StoreDAO> Stores { get; set; }
        public virtual ICollection<SupplierDAO> Suppliers { get; set; }
        public virtual ICollection<WarehouseDAO> Warehouses { get; set; }
    }
}
