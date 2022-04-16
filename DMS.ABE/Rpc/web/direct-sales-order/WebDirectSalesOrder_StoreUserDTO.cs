using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using System;
namespace DMS.ABE.Rpc.web.direct_sales_order
{
    public class WebDirectSalesOrder_StoreUserDTO : DataDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string OtpCode { get; set; }
        public DateTime? OtpExpired { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public WebDirectSalesOrder_StoreUserDTO() { }
        public WebDirectSalesOrder_StoreUserDTO(StoreUser StoreUser)
        {
            this.Id = StoreUser.Id;
            this.StoreId = StoreUser.StoreId;
            this.Username = StoreUser.Username;
            this.DisplayName = StoreUser.DisplayName;
            this.Password = StoreUser.Password;
            this.OtpCode = StoreUser.OtpCode;
            this.OtpExpired = StoreUser.OtpExpired;
            this.StatusId = StoreUser.StatusId;
            this.RowId = StoreUser.RowId;
            this.Used = StoreUser.Used;
            this.Errors = StoreUser.Errors;
        }
    }
    public class WebDirectSalesOrder_StoreUserFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public StringFilter Password { get; set; }
        public StringFilter OtpCode { get; set; }
        public DateFilter OtpExpired { get; set; }
        public IdFilter StatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public StoreUserOrder OrderBy { get; set; }
    }
}