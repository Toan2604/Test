using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.position
{
    public class Position_PositionDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public Position_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Position_PositionDTO() { }
        public Position_PositionDTO(Position Position)
        {
            this.Id = Position.Id;
            this.Code = Position.Code;
            this.Name = Position.Name;
            this.StatusId = Position.StatusId;
            this.Status = Position.Status == null ? null : new Position_StatusDTO(Position.Status);
            this.CreatedAt = Position.CreatedAt;
            this.UpdatedAt = Position.UpdatedAt;
            this.Errors = Position.Errors;
        }
    }

    public class Position_PositionFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public PositionOrder OrderBy { get; set; }
    }
}
