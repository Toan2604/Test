using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.price_list
{
    public class PriceList_PriceListDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long CreatorId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long StatusId { get; set; }
        public long OrganizationId { get; set; }
        public long PriceListTypeId { get; set; }
        public long SalesOrderTypeId { get; set; }
        public long RequestStateId { get; set; }
        public Guid RowId { get; set; }
        public PriceList_AppUserDTO Creator { get; set; }
        public PriceList_PriceListTypeDTO PriceListType { get; set; }
        public PriceList_RequestStateDTO RequestState { get; set; }
        public PriceList_OrganizationDTO Organization { get; set; }
        public PriceList_SalesOrderTypeDTO SalesOrderType { get; set; }
        public PriceList_StatusDTO Status { get; set; }
        public List<PriceList_PriceListItemMappingDTO> PriceListItemMappings { get; set; }
        public List<PriceList_PriceListStoreGroupingMappingDTO> PriceListStoreGroupingMappings { get; set; }
        public List<PriceList_PriceListStoreMappingDTO> PriceListStoreMappings { get; set; }
        public List<PriceList_PriceListStoreTypeMappingDTO> PriceListStoreTypeMappings { get; set; }
        public List<PriceList_RequestWorkflowStepMappingDTO> RequestWorkflowStepMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public PriceList_PriceListDTO() { }
        public PriceList_PriceListDTO(PriceList PriceList)
        {
            this.Id = PriceList.Id;
            this.Code = PriceList.Code;
            this.Name = PriceList.Name;
            this.CreatorId = PriceList.CreatorId;
            this.StartDate = PriceList.StartDate;
            this.EndDate = PriceList.EndDate;
            this.StatusId = PriceList.StatusId;
            this.OrganizationId = PriceList.OrganizationId;
            this.PriceListTypeId = PriceList.PriceListTypeId;
            this.SalesOrderTypeId = PriceList.SalesOrderTypeId;
            this.RequestStateId = PriceList.RequestStateId;
            this.RowId = PriceList.RowId;
            this.Creator = PriceList.Creator == null ? null : new PriceList_AppUserDTO(PriceList.Creator);
            this.PriceListType = PriceList.PriceListType == null ? null : new PriceList_PriceListTypeDTO(PriceList.PriceListType);
            this.RequestState = PriceList.RequestState == null ? null : new PriceList_RequestStateDTO(PriceList.RequestState);
            this.Organization = PriceList.Organization == null ? null : new PriceList_OrganizationDTO(PriceList.Organization);
            this.SalesOrderType = PriceList.SalesOrderType == null ? null : new PriceList_SalesOrderTypeDTO(PriceList.SalesOrderType);
            this.Status = PriceList.Status == null ? null : new PriceList_StatusDTO(PriceList.Status);
            this.PriceListItemMappings = PriceList.PriceListItemMappings?.Select(x => new PriceList_PriceListItemMappingDTO(x)).ToList();
            this.PriceListStoreGroupingMappings = PriceList.PriceListStoreGroupingMappings?.Select(x => new PriceList_PriceListStoreGroupingMappingDTO(x)).ToList();
            this.PriceListStoreMappings = PriceList.PriceListStoreMappings?.Select(x => new PriceList_PriceListStoreMappingDTO(x)).ToList();
            this.PriceListStoreTypeMappings = PriceList.PriceListStoreTypeMappings?.Select(x => new PriceList_PriceListStoreTypeMappingDTO(x)).ToList();
            this.RequestWorkflowStepMappings = PriceList.RequestWorkflowStepMappings?.Select(x => new PriceList_RequestWorkflowStepMappingDTO(x)).ToList();
            this.CreatedAt = PriceList.CreatedAt;
            this.UpdatedAt = PriceList.UpdatedAt;
            this.Errors = PriceList.Errors;
        }
    }

    public class PriceList_PriceListFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter CreatorId { get; set; }
        public DateFilter StartDate { get; set; }
        public DateFilter EndDate { get; set; }
        public IdFilter StatusId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter PriceListTypeId { get; set; }
        public IdFilter SalesOrderTypeId { get; set; }
        public IdFilter RequestStateId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public PriceListOrder OrderBy { get; set; }
    }
}
