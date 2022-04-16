using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_AlbumDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }
        public long StatusId { get; set; }
        public GeneralMobile_StatusDTO Status { get; set; }
        public List<GeneralMobile_AlbumImageMappingDTO> AlbumImageMappings { get; set; }
        public GeneralMobile_AlbumDTO() { }
        public GeneralMobile_AlbumDTO(Album Album)
        {

            this.Id = Album.Id;

            this.Name = Album.Name;
            this.StatusId = Album.StatusId;
            this.Status = Album.Status == null ? null : new GeneralMobile_StatusDTO(Album.Status);
            this.AlbumImageMappings = Album.AlbumImageMappings?.Select(x => new GeneralMobile_AlbumImageMappingDTO(x)).ToList();

            this.Errors = Album.Errors;
        }
    }

    public class GeneralMobile_AlbumFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public AlbumOrder OrderBy { get; set; }
    }
}