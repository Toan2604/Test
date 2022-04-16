using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_ColorDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public GeneralMobile_ColorDTO() { }
        public GeneralMobile_ColorDTO(Color Color)
        {
            this.Id = Color.Id;
            this.Code = Color.Code;
            this.Name = Color.Name;
            this.Errors = Color.Errors;
        }
    }

    public class GeneralMobile_ColorFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public ColorOrder OrderBy { get; set; }
    }
}
