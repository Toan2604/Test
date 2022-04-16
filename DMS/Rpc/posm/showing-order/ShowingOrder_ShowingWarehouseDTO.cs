using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order
{
    public class ShowingOrder_ShowingWarehouseDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public long OrganizationId { get; set; }

        public long? ProvinceId { get; set; }

        public long? DistrictId { get; set; }

        public long? WardId { get; set; }

        public long StatusId { get; set; }

        public Guid RowId { get; set; }


        public ShowingOrder_ShowingWarehouseDTO() { }
        public ShowingOrder_ShowingWarehouseDTO(ShowingWarehouse ShowingWarehouse)
        {

            this.Id = ShowingWarehouse.Id;

            this.Code = ShowingWarehouse.Code;

            this.Name = ShowingWarehouse.Name;

            this.Address = ShowingWarehouse.Address;

            this.OrganizationId = ShowingWarehouse.OrganizationId;

            this.ProvinceId = ShowingWarehouse.ProvinceId;

            this.DistrictId = ShowingWarehouse.DistrictId;

            this.WardId = ShowingWarehouse.WardId;

            this.StatusId = ShowingWarehouse.StatusId;

            this.RowId = ShowingWarehouse.RowId;

            this.Errors = ShowingWarehouse.Errors;
        }
    }

    public class ShowingOrder_ShowingWarehouseFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Address { get; set; }

        public IdFilter OrganizationId { get; set; }

        public IdFilter ProvinceId { get; set; }

        public IdFilter DistrictId { get; set; }

        public IdFilter WardId { get; set; }

        public IdFilter StatusId { get; set; }

        public GuidFilter RowId { get; set; }

        public ShowingWarehouseOrder OrderBy { get; set; }
    }
}