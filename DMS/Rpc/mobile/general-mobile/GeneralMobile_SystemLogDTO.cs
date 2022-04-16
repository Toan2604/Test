using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_SystemLogDTO : DataDTO
    {
        public long Id { get; set; }
        public long? AppUserId { get; set; }
        public string AppUser { get; set; }
        public string Exception { get; set; }
        public string ModuleName { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
        public DateTime Time { get; set; }
        public Guid RowId { get; set; }
        public GeneralMobile_SystemLogDTO() { }
        public GeneralMobile_SystemLogDTO(SystemLog SystemLog) 
        { 
            this.Id = SystemLog.Id;
            this.AppUserId = SystemLog.AppUserId;
            this.Warnings = SystemLog.Warnings;
            this.Exception = SystemLog.Exception;
            this.ModuleName = SystemLog.ModuleName;
            this.ClassName = SystemLog.ClassName;
            this.MethodName = SystemLog.MethodName;
            this.Time = SystemLog.Time;
            this.RowId = SystemLog.RowId;
        }
    }
}
