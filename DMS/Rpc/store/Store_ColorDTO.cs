using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_ColorDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Store_ColorDTO() { }
        public Store_ColorDTO(Color Color)
        {
            this.Id = Color.Id;
            this.Code = Color.Code;
            this.Name = Color.Name;
            this.Errors = Color.Errors;
        }
    }

    public class Store_ColorFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public ColorOrder OrderBy { get; set; }
    }
}
