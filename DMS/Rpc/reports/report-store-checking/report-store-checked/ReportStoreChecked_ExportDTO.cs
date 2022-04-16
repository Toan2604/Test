using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.reports.report_store_checking.report_store_checked
{
    public class ReportStoreChecked_ExportDTO : DataDTO
    {
        public string OrganizationName { get; set; }
        public List<ReportStoreChecked_ExportGroupBySalesEmployeeDTO> SalesEmployees { get; set; }
        public ReportStoreChecked_ExportDTO() { }
        public ReportStoreChecked_ExportDTO(ReportStoreChecked_ReportStoreCheckedDTO ReportStoreChecked_ReportStoreCheckedDTO)
        {
            this.OrganizationName = ReportStoreChecked_ReportStoreCheckedDTO.OrganizationName;
            this.SalesEmployees = ReportStoreChecked_ReportStoreCheckedDTO.SaleEmployees.Select(x => new ReportStoreChecked_ExportGroupBySalesEmployeeDTO(x)).ToList();
        }

    }

    public class ReportStoreChecked_ExportGroupBySalesEmployeeDTO : DataDTO
    {
        public long STT { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public List<ReportStoreChecked_ExportGroupByDateDTO> Dates { get; set; }
        public ReportStoreChecked_ExportGroupBySalesEmployeeDTO() { }
        public ReportStoreChecked_ExportGroupBySalesEmployeeDTO(ReportStoreChecked_SaleEmployeeDTO ReportStoreChecked_SaleEmployeeDTO)
        {
            this.Username = ReportStoreChecked_SaleEmployeeDTO.Username;
            this.DisplayName = ReportStoreChecked_SaleEmployeeDTO.DisplayName;
            this.Dates = ReportStoreChecked_SaleEmployeeDTO.StoreCheckingGroupByDates.Select(x => new ReportStoreChecked_ExportGroupByDateDTO(x)).ToList();
        }
    }

    public class ReportStoreChecked_ExportGroupByDateDTO : DataDTO
    {
        internal DateTime Date { get; set; }
        public string DateString { get; set; }
        public string DayOfWeek { get; set; }
        public List<ReportStoreChecked_ExportContentDTO> Contents { get; set; }
        public ReportStoreChecked_ExportGroupByDateDTO() { }
        public ReportStoreChecked_ExportGroupByDateDTO(ReportStoreChecked_StoreCheckingGroupByDateDTO ReportStoreChecked_StoreCheckingGroupByDateDTO)
        {
            this.Date = ReportStoreChecked_StoreCheckingGroupByDateDTO.Date;
            this.DateString = ReportStoreChecked_StoreCheckingGroupByDateDTO.DateString;
            this.DayOfWeek = ReportStoreChecked_StoreCheckingGroupByDateDTO.DayOfWeek;
            this.Contents = ReportStoreChecked_StoreCheckingGroupByDateDTO.StoreCheckings.Select(x => new ReportStoreChecked_ExportContentDTO(x)).ToList();
        }
    }

    public class ReportStoreChecked_ExportContentDTO : DataDTO
    {
        public long STT { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }
        public string DateString { get; set; }
        public string DayOfWeek { get; set; }

        public string StoreCode { get; set; }
        public string StoreCodeDraft { get; set; }
        public string StoreName { get; set; }
        public string StoreStatusName { get; set; }
        public string StoreAddress { get; set; }
        public string CheckIn { get; set; }
        public string CheckOut { get; set; }
        public string CheckInDistance { get; set; }
        public string CheckOutDistance { get; set; }
        public string Duration { get; set; }
        public string Planned { get; set; }
        public string Image { get; set; }
        public string Order { get; set; }
        public string Closed { get; set; }
        public long ImageCounter { get; set; }
        public long OrderCounter { get; set; }
        public ReportStoreChecked_ExportContentDTO() { }
        public ReportStoreChecked_ExportContentDTO(ReportStoreChecked_StoreCheckingDTO ReportStoreChecked_StoreCheckingDTO)
        {
            this.StoreCode = ReportStoreChecked_StoreCheckingDTO.StoreCode;
            this.StoreCodeDraft = ReportStoreChecked_StoreCheckingDTO.StoreCodeDraft;
            this.StoreName = ReportStoreChecked_StoreCheckingDTO.StoreName;
            this.StoreStatusName = ReportStoreChecked_StoreCheckingDTO.StoreStatusName;
            this.StoreAddress = ReportStoreChecked_StoreCheckingDTO.StoreAddress;
            this.CheckIn = ReportStoreChecked_StoreCheckingDTO.eCheckIn;
            this.CheckOut = ReportStoreChecked_StoreCheckingDTO.eCheckOut;
            this.CheckInDistance = ReportStoreChecked_StoreCheckingDTO.CheckInDistance;
            this.CheckOutDistance = ReportStoreChecked_StoreCheckingDTO.CheckOutDistance;
            this.Duration = ReportStoreChecked_StoreCheckingDTO.Duaration;
            this.Image = ReportStoreChecked_StoreCheckingDTO.ImageCounter.ToString();
            this.ImageCounter = ReportStoreChecked_StoreCheckingDTO.ImageCounter;
            this.Planned = ReportStoreChecked_StoreCheckingDTO.Planned ? "X" : "";
            this.Order = ReportStoreChecked_StoreCheckingDTO.SalesOrderCounter.ToString();
            this.OrderCounter = ReportStoreChecked_StoreCheckingDTO.SalesOrderCounter;
            this.Closed = ReportStoreChecked_StoreCheckingDTO.IsClosed ? "X" : "";
        }
    }
}
