using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order
{
    public class ShowingOrder_StatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public ShowingOrder_StatusDTO() { }
        public ShowingOrder_StatusDTO(Status Status)
        {

            this.Id = Status.Id;

            this.Code = Status.Code;

            this.Name = Status.Name;

            this.Errors = Status.Errors;
        }
    }

    public class ShowingOrder_StatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}