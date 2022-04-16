using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreCheckingStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public GeneralMobile_StoreCheckingStatusDTO() { }
        public GeneralMobile_StoreCheckingStatusDTO(Status Status)
        {

            this.Id = Status.Id;

            this.Code = Status.Code;

            this.Name = Status.Name;

        }
    }

    public class GeneralMobile_StoreCheckingStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StatusOrder OrderBy { get; set; }
    }
}
