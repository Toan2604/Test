using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.sync_fast
{
    public class Sync_PriceListDTO : DataDTO
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
        public List<Sync_PriceListItemMappingDTO> PriceListItemMappings { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Sync_PriceListDTO() { }
        public Sync_PriceListDTO(PriceList PriceList)
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
            this.PriceListItemMappings = PriceList.PriceListItemMappings?.Select(x => new Sync_PriceListItemMappingDTO(x)).ToList();
            this.CreatedAt = PriceList.CreatedAt;
            this.UpdatedAt = PriceList.UpdatedAt;
            this.Errors = PriceList.Errors;
        }
    }

    public class Sync_PriceListFilterDTO : FilterDTO
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
