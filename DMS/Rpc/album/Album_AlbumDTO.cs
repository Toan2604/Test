using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.album
{
    public class Album_AlbumDTO : DataDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public Album_StatusDTO Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Album_AlbumDTO() { }
        public Album_AlbumDTO(Album Album)
        {
            this.Id = Album.Id;
            this.Name = Album.Name;
            this.StatusId = Album.StatusId;
            this.Used = Album.Used;
            this.Status = Album.Status == null ? null : new Album_StatusDTO(Album.Status);
            this.CreatedAt = Album.CreatedAt;
            this.UpdatedAt = Album.UpdatedAt;
            this.Errors = Album.Errors;
        }
    }

    public class Album_AlbumFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public AlbumOrder OrderBy { get; set; }
    }
}
