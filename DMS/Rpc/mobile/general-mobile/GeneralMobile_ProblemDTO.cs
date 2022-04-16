using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_ProblemDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long? StoreCheckingId { get; set; }
        public long StoreId { get; set; }
        public long CreatorId { get; set; }
        public long ProblemTypeId { get; set; }
        public long ProblemStatusId { get; set; }
        public DateTime NoteAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public string Content { get; set; }
        public Guid RowId { get; set; }
        public GeneralMobile_AppUserDTO Creator { get; set; }
        public GeneralMobile_ProblemTypeDTO ProblemType { get; set; }
        public GeneralMobile_ProblemStatusDTO ProblemStatus { get; set; }
        public GeneralMobile_StoreDTO Store { get; set; }
        public List<GeneralMobile_ProblemImageMappingDTO> ProblemImageMappings { get; set; }

        public GeneralMobile_ProblemDTO() { }
        public GeneralMobile_ProblemDTO(Problem Problem)
        {
            this.Id = Problem.Id;
            this.Code = Problem.Code;
            this.StoreCheckingId = Problem.StoreCheckingId;
            this.StoreId = Problem.StoreId;
            this.CreatorId = Problem.CreatorId;
            this.ProblemTypeId = Problem.ProblemTypeId;
            this.ProblemStatusId = Problem.ProblemStatusId;
            this.NoteAt = Problem.NoteAt;
            this.CompletedAt = Problem.CompletedAt;
            this.Content = Problem.Content;
            this.RowId = Problem.RowId;
            this.Creator = Problem.Creator == null ? null : new GeneralMobile_AppUserDTO(Problem.Creator);
            this.ProblemType = Problem.ProblemType == null ? null : new GeneralMobile_ProblemTypeDTO(Problem.ProblemType);
            this.ProblemStatus = Problem.ProblemStatus == null ? null : new GeneralMobile_ProblemStatusDTO(Problem.ProblemStatus);
            this.Store = Problem.Store == null ? null : new GeneralMobile_StoreDTO(Problem.Store);
            this.ProblemImageMappings = Problem.ProblemImageMappings?.Select(pi => new GeneralMobile_ProblemImageMappingDTO(pi)).ToList();
        }
    }

    public class GeneralMobile_ProblemFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter StoreCheckingId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter CreatorId { get; set; }
        public IdFilter ProblemTypeId { get; set; }
        public IdFilter ProblemStatusId { get; set; }
        public DateFilter NoteAt { get; set; }
        public DateFilter CompletedAt { get; set; }
        public StringFilter Content { get; set; }
        public ProblemOrder OrderBy { get; set; }
        public ProblemSelect Selects { get; set; }
    }
}
