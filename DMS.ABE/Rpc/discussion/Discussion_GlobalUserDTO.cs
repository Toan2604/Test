using DMS.ABE.Entities;
using System;
using TrueSight.Common;

namespace DMS.ABE.Rpc.discussion
{
    public class Discussion_GlobalUserDTO : DataDTO
    {        
        public long Id { get; set; }        
        public string Username { get; set; }        
        public string DisplayName { get; set; }        
        public string Avatar { get; set; }        
        public Guid RowId { get; set; }
        public long GlobalUserTypeId { get; set; }  
        public Discussion_GlobalUserTypeDTO GlobalUserType { get; set; }  
        public Discussion_GlobalUserDTO() {}
        public Discussion_GlobalUserDTO(GlobalUser GlobalUser)
        {
            
            this.Id = GlobalUser.Id;            
            this.Username = GlobalUser.Username;            
            this.DisplayName = GlobalUser.DisplayName;
            this.Avatar = GlobalUser.Avatar;
            this.GlobalUserTypeId = GlobalUser.GlobalUserTypeId;
            this.GlobalUserType = GlobalUser.GlobalUserType != null ? new Discussion_GlobalUserTypeDTO(GlobalUser.GlobalUserType) : null;
            this.RowId = GlobalUser.RowId;            
            this.Errors = GlobalUser.Errors;
        }
    }
    public class Discussion_GlobalUserFilterDTO : FilterDTO
    {        
        public IdFilter Id { get; set; }
        public IdFilter GlobalUserTypeId { get; set; }        
        public StringFilter Username { get; set; }        
        public StringFilter DisplayName { get; set; }        
        public GuidFilter RowId { get; set; }        
        public GlobalUserOrder OrderBy { get; set; }
    }
}