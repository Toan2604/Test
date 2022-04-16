using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.monitor_store_problems
{
    public class MonitorStoreProblem_ProblemDTO : DataDTO
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
        public string NoteAtDisplay { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Content { get; set; }
        public long ProblemStatusId { get; set; }
        public Guid RowId { get; set; }
        public MonitorStoreProblem_AppUserDTO Creator { get; set; }
        public MonitorStoreProblem_OrganizationDTO Organization { get; set; }
        public MonitorStoreProblem_ProblemStatusDTO ProblemStatus { get; set; }
        public MonitorStoreProblem_ProblemTypeDTO ProblemType { get; set; }
        public MonitorStoreProblem_StoreDTO Store { get; set; }
        public MonitorStoreProblem_StoreCheckingDTO StoreChecking { get; set; }
        public List<MonitorStoreProblem_ProblemImageMappingDTO> ProblemImageMappings { get; set; }
        public List<MonitorStoreProblem_ProblemHistoryDTO> ProblemHistorys { get; set; }
        public MonitorStoreProblem_ProblemDTO() { }
        public MonitorStoreProblem_ProblemDTO(Problem Problem)
        {
            this.Id = Problem.Id;
            this.Code = Problem.Code;
            this.StoreCheckingId = Problem.StoreCheckingId;
            this.StoreId = Problem.StoreId;
            this.CreatorId = Problem.CreatorId;
            this.OrganizationId = Problem.Creator.OrganizationId;
            this.ProblemTypeId = Problem.ProblemTypeId;
            this.NoteAt = Problem.NoteAt;
            this.CompletedAt = Problem.CompletedAt;
            this.Content = Problem.Content;
            this.ProblemStatusId = Problem.ProblemStatusId;
            this.RowId = Problem.RowId;
            this.Creator = Problem.Creator == null ? null : new MonitorStoreProblem_AppUserDTO(Problem.Creator);
            this.Organization = Problem.Creator.Organization == null ? null : new MonitorStoreProblem_OrganizationDTO(Problem.Creator.Organization);
            this.ProblemStatus = Problem.ProblemStatus == null ? null : new MonitorStoreProblem_ProblemStatusDTO(Problem.ProblemStatus);
            this.ProblemType = Problem.ProblemType == null ? null : new MonitorStoreProblem_ProblemTypeDTO(Problem.ProblemType);
            this.Store = Problem.Store == null ? null : new MonitorStoreProblem_StoreDTO(Problem.Store);
            this.StoreChecking = Problem.StoreChecking == null ? null : new MonitorStoreProblem_StoreCheckingDTO(Problem.StoreChecking);
            this.ProblemImageMappings = Problem.ProblemImageMappings?.Select(x => new MonitorStoreProblem_ProblemImageMappingDTO(x)).ToList();
            this.ProblemHistorys = Problem.ProblemHistories?.Select(x => new MonitorStoreProblem_ProblemHistoryDTO(x)).ToList();
            this.Errors = Problem.Errors;
            this.NoteAtDisplay = this.NoteAt.ToString("dd-MM-yyyy");
        }
    }

    public class MonitorStoreProblem_ProblemFilterDTO : FilterDTO
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
        public IdFilter ProvinceId { get; set; }
        public IdFilter DistrictId { get; set; }
        public ProblemOrder OrderBy { get; set; }
        public List<MonitorStoreProblem_ProblemFilterDTO> OrFilters { get; set; }
    }
}
