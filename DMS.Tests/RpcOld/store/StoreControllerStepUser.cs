using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using DMS.Enums;
using DMS.Rpc.store;
using Microsoft.AspNetCore.Http;
using System.IO;
namespace DMS.Tests.Rpc.store
{
    public partial class StoreControllerFeature
    {
        private async Task When_UserInput()
        {
            Store_StoreDTO = new Store_StoreDTO
            {
                Id = 0,
                Code = null,
                CodeDraft = null,
                Name = null,
                ParentStoreId = null,
                OrganizationId = 0,
                StoreTypeId = 0,
                Telephone = null,
                ProvinceId = null,
                DistrictId = null,
                WardId = null,
                Address = null,
                DeliveryAddress = null,
                Latitude = 0,
                Longitude = 0,
                DeliveryLatitude = null,
                DeliveryLongitude = null,
                OwnerName = null,
                OwnerPhone = null,
                OwnerEmail = null,
                TaxCode = null,
                LegalEntity = null,
                CreatorId = 0,
                AppUserId = null,
                StatusId = 0,
                StoreScoutingId = null,
                StoreStatusId = 0,
                AppUser = new Store_AppUserDTO
                {
                    Id = 0,
                    Username = null,
                    DisplayName = null,
                    Address = null,
                    Email = null,
                    Phone = null,
                    SexId = 0,
                    Avatar = null,
                    PositionId = null,
                    Department = null,
                    OrganizationId = 0,
                    ProvinceId = null,
                    StatusId = 0,
                },
                Creator = new Store_AppUserDTO
                {
                    Id = 0,
                    Username = null,
                    DisplayName = null,
                    Address = null,
                    Email = null,
                    Phone = null,
                    SexId = 0,
                    Avatar = null,
                    PositionId = null,
                    Department = null,
                    OrganizationId = 0,
                    ProvinceId = null,
                    StatusId = 0,
                },
                District = new Store_DistrictDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                    Priority = null,
                    ProvinceId = 0,
                    StatusId = 0,
                },
                Organization = new Store_OrganizationDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                    ParentId = null,
                    Path = null,
                    Level = 0,
                    StatusId = 0,
                    Phone = null,
                    Email = null,
                    Address = null,
                },
                ParentStore = new Store_StoreDTO
                {
                    Id = 0,
                    Code = null,
                    CodeDraft = null,
                    Name = null,
                    ParentStoreId = null,
                    OrganizationId = 0,
                    StoreTypeId = 0,
                    Telephone = null,
                    ProvinceId = null,
                    DistrictId = null,
                    WardId = null,
                    Address = null,
                    DeliveryAddress = null,
                    Latitude = 0,
                    Longitude = 0,
                    DeliveryLatitude = null,
                    DeliveryLongitude = null,
                    OwnerName = null,
                    OwnerPhone = null,
                    OwnerEmail = null,
                    TaxCode = null,
                    LegalEntity = null,
                    CreatorId = 0,
                    AppUserId = null,
                    StatusId = 0,
                    StoreScoutingId = null,
                    StoreStatusId = 0,
                },
                Province = new Store_ProvinceDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                    Priority = null,
                    StatusId = 0,
                },
                Status = new Store_StatusDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                },
                StoreScouting = new Store_StoreScoutingDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                    OwnerPhone = null,
                    ProvinceId = null,
                    DistrictId = null,
                    WardId = null,
                    Address = null,
                    Latitude = 0,
                    Longitude = 0,
                    CreatorId = 0,
                    OrganizationId = 0,
                    StoreScoutingStatusId = 0,
                },
                StoreStatus = new Store_StoreStatusDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                },
                StoreType = new Store_StoreTypeDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                    ColorId = null,
                    StatusId = 0,
                },
                Ward = new Store_WardDTO
                {
                    Id = 0,
                    Code = null,
                    Name = null,
                    Priority = null,
                    DistrictId = 0,
                    StatusId = 0,
                },
            };
        }
        private async Task When_UserImportExcel(string path)
        {
            byte[] array = System.IO.File.ReadAllBytes(path);
            MemoryStream MemoryStream = new MemoryStream(array);
            FormFile = new FormFile(MemoryStream, 0, MemoryStream.Length, null, Path.GetFileName(path))
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
        private async Task When_UserCreate()
        {
            await StoreController.Create(Store_StoreDTO);
        }
        private async Task When_UserUpdate()
        {
            await StoreController.Update(Store_StoreDTO);
        }
        private async Task When_UserDelete()
        {
            await StoreController.Delete(Store_StoreDTO);
        }
        private async Task When_UserImport()
        {
            await StoreController.Import(FormFile);
        }
    }
}
