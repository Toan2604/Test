using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_ProblemDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public long? StoreCheckingId { get; set; }
        public long StoreId { get; set; }
        public long CreatorId { get; set; }
        public long? OrganizationId { get; set; }
        public long ProblemTypeId { get; set; }
        public DateTime NoteAt { get; set; }
        public string NoteAtDisplay => NoteAt.ToString("dd-MM-yyyy");
        public DateTime? CompletedAt { get; set; }
        public string Content { get; set; }
        public long ProblemStatusId { get; set; }
        public Guid RowId { get; set; }
        public MobileSync_AppUserDTO Creator { get; set; }
        public MobileSync_OrganizationDTO Organization { get; set; }
        public MobileSync_ProblemStatusDTO ProblemStatus { get; set; }
        public MobileSync_ProblemTypeDTO ProblemType { get; set; }
        public MobileSync_StoreDTO Store { get; set; }
        public MobileSync_StoreCheckingDTO StoreChecking { get; set; }
        public List<MobileSync_ProblemImageMappingDTO> ProblemImageMappings { get; set; }
        public List<MobileSync_ProblemHistoryDTO> ProblemHistorys { get; set; }
        public MobileSync_ProblemDTO() { }
        public MobileSync_ProblemDTO(Problem ItemImageMapping)
        {
            this.Id = ItemImageMapping.Id;
            this.Code = ItemImageMapping.Code;
            this.StoreCheckingId = ItemImageMapping.StoreCheckingId;
            this.StoreId = ItemImageMapping.StoreId;
            this.CreatorId = ItemImageMapping.CreatorId;
            this.OrganizationId = ItemImageMapping.Creator.OrganizationId;
            this.ProblemTypeId = ItemImageMapping.ProblemTypeId;
            this.NoteAt = ItemImageMapping.NoteAt;
            this.CompletedAt = ItemImageMapping.CompletedAt;
            this.Content = ItemImageMapping.Content;
            this.ProblemStatusId = ItemImageMapping.ProblemStatusId;
            this.RowId = ItemImageMapping.RowId;
            this.Creator = ItemImageMapping.Creator == null ? null : new MobileSync_AppUserDTO(ItemImageMapping.Creator);
            this.Organization = ItemImageMapping.Creator.Organization == null ? null : new MobileSync_OrganizationDTO(ItemImageMapping.Creator.Organization);
            this.ProblemStatus = ItemImageMapping.ProblemStatus == null ? null : new MobileSync_ProblemStatusDTO(ItemImageMapping.ProblemStatus);
            this.ProblemType = ItemImageMapping.ProblemType == null ? null : new MobileSync_ProblemTypeDTO(ItemImageMapping.ProblemType);
            this.Store = ItemImageMapping.Store == null ? null : new MobileSync_StoreDTO(ItemImageMapping.Store);
            this.StoreChecking = ItemImageMapping.StoreChecking == null ? null : new MobileSync_StoreCheckingDTO(ItemImageMapping.StoreChecking);
            this.ProblemImageMappings = ItemImageMapping.ProblemImageMappings?.Select(x => new MobileSync_ProblemImageMappingDTO(x)).ToList();
            this.ProblemHistorys = ItemImageMapping.ProblemHistories?.Select(x => new MobileSync_ProblemHistoryDTO(x)).ToList();
        }
    }

    public class MobileSync_ProblemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter ProblemTypeId { get; set; }
        public DateFilter NoteAt { get; set; }
        public DateFilter CompletedAt { get; set; }
        public StringFilter Content { get; set; }
        public IdFilter ProblemStatusId { get; set; }
        public ProblemOrder OrderBy { get; set; }
        public List<MobileSync_ProblemFilterDTO> OrFilters { get; set; }
    }
}
