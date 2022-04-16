using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_StoreUserDTO : DataDTO
    {
        public long Id { get; set; }
        public long StoreId { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public long StatusId { get; set; }
        public Guid RowId { get; set; }
        public bool Used { get; set; }
        public Store_StatusDTO Status { get; set; }
        public Store_StoreDTO Store { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Store_StoreUserDTO() { }
        public Store_StoreUserDTO(StoreUser StoreUser)
        {
            this.Id = StoreUser.Id;
            this.StoreId = StoreUser.StoreId;
            this.Username = StoreUser.Username;
            this.DisplayName = StoreUser.DisplayName;
            this.Password = StoreUser.Password;
            this.StatusId = StoreUser.StatusId;
            this.RowId = StoreUser.RowId;
            this.Used = StoreUser.Used;
            this.Status = StoreUser.Status == null ? null : new Store_StatusDTO(StoreUser.Status);
            this.Store = StoreUser.Store == null ? null : new Store_StoreDTO(StoreUser.Store);
            this.CreatedAt = StoreUser.CreatedAt;
            this.UpdatedAt = StoreUser.UpdatedAt;
            this.Errors = StoreUser.Errors;
        }
    }

    public class Store_StoreUserFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreId { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public StringFilter Password { get; set; }
        public IdFilter StatusId { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public StoreUserOrder OrderBy { get; set; }
    }
}
