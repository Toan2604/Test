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
namespace DMS.Tests.Rpc.store
{
    public partial class StoreControllerFeature
    {
        private async Task Then_ImportSuccess(int count)
        {
            int store_count = await DataContext.Store.CountAsync();
            Assert.AreEqual(count, store_count);
        }
        private async Task Then_Success()
        {
            await Then_Id(Store_StoreDTO);
            await Then_Code(Store_StoreDTO);
            await Then_CodeDraft(Store_StoreDTO);
            await Then_Name(Store_StoreDTO);
            await Then_ParentStoreId(Store_StoreDTO);
            await Then_OrganizationId(Store_StoreDTO);
            await Then_StoreTypeId(Store_StoreDTO);
            await Then_Telephone(Store_StoreDTO);
            await Then_ProvinceId(Store_StoreDTO);
            await Then_DistrictId(Store_StoreDTO);
            await Then_WardId(Store_StoreDTO);
            await Then_Address(Store_StoreDTO);
            await Then_DeliveryAddress(Store_StoreDTO);
            await Then_Latitude(Store_StoreDTO);
            await Then_Longitude(Store_StoreDTO);
            await Then_DeliveryLatitude(Store_StoreDTO);
            await Then_DeliveryLongitude(Store_StoreDTO);
            await Then_OwnerName(Store_StoreDTO);
            await Then_OwnerPhone(Store_StoreDTO);
            await Then_OwnerEmail(Store_StoreDTO);
            await Then_TaxCode(Store_StoreDTO);
            await Then_LegalEntity(Store_StoreDTO);
            await Then_CreatorId(Store_StoreDTO);
            await Then_AppUserId(Store_StoreDTO);
            await Then_StatusId(Store_StoreDTO);
            await Then_Used(Store_StoreDTO);
            await Then_StoreScoutingId(Store_StoreDTO);
            await Then_StoreStatusId(Store_StoreDTO);
            await Then_IsStoreApprovalDirectSalesOrder(Store_StoreDTO);
        }
        private async Task Then_Id(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Id)));
            Assert.AreEqual(0, Store_StoreDTO.Id);
        }
        private async Task Then_Code(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Code)));
            Assert.AreEqual(null, Store_StoreDTO.Code);
        }
        private async Task Then_CodeDraft(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.CodeDraft)));
            Assert.AreEqual(null, Store_StoreDTO.CodeDraft);
        }
        private async Task Then_Name(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Name)));
            Assert.AreEqual(null, Store_StoreDTO.Name);
        }
        private async Task Then_ParentStoreId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.ParentStoreId)));
            Assert.AreEqual(null, Store_StoreDTO.ParentStoreId);
        }
        private async Task Then_OrganizationId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.OrganizationId)));
            Assert.AreEqual(0, Store_StoreDTO.OrganizationId);
        }
        private async Task Then_StoreTypeId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.StoreTypeId)));
            Assert.AreEqual(0, Store_StoreDTO.StoreTypeId);
        }
        private async Task Then_Telephone(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Telephone)));
            Assert.AreEqual(null, Store_StoreDTO.Telephone);
        }
        private async Task Then_ProvinceId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.ProvinceId)));
            Assert.AreEqual(null, Store_StoreDTO.ProvinceId);
        }
        private async Task Then_DistrictId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.DistrictId)));
            Assert.AreEqual(null, Store_StoreDTO.DistrictId);
        }
        private async Task Then_WardId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.WardId)));
            Assert.AreEqual(null, Store_StoreDTO.WardId);
        }
        private async Task Then_Address(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Address)));
            Assert.AreEqual(null, Store_StoreDTO.Address);
        }
        private async Task Then_DeliveryAddress(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.DeliveryAddress)));
            Assert.AreEqual(null, Store_StoreDTO.DeliveryAddress);
        }
        private async Task Then_Latitude(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Latitude)));
            Assert.AreEqual(0, Store_StoreDTO.Latitude);
        }
        private async Task Then_Longitude(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Longitude)));
            Assert.AreEqual(0, Store_StoreDTO.Longitude);
        }
        private async Task Then_DeliveryLatitude(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.DeliveryLatitude)));
            Assert.AreEqual(null, Store_StoreDTO.DeliveryLatitude);
        }
        private async Task Then_DeliveryLongitude(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.DeliveryLongitude)));
            Assert.AreEqual(null, Store_StoreDTO.DeliveryLongitude);
        }
        private async Task Then_OwnerName(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.OwnerName)));
            Assert.AreEqual(null, Store_StoreDTO.OwnerName);
        }
        private async Task Then_OwnerPhone(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.OwnerPhone)));
            Assert.AreEqual(null, Store_StoreDTO.OwnerPhone);
        }
        private async Task Then_OwnerEmail(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.OwnerEmail)));
            Assert.AreEqual(null, Store_StoreDTO.OwnerEmail);
        }
        private async Task Then_TaxCode(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.TaxCode)));
            Assert.AreEqual(null, Store_StoreDTO.TaxCode);
        }
        private async Task Then_LegalEntity(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.LegalEntity)));
            Assert.AreEqual(null, Store_StoreDTO.LegalEntity);
        }
        private async Task Then_CreatorId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.CreatorId)));
            Assert.AreEqual(0, Store_StoreDTO.CreatorId);
        }
        private async Task Then_AppUserId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.AppUserId)));
            Assert.AreEqual(null, Store_StoreDTO.AppUserId);
        }
        private async Task Then_StatusId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.StatusId)));
            Assert.AreEqual(0, Store_StoreDTO.StatusId);
        }
        private async Task Then_Used(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.Used)));
            Assert.AreEqual(false, Store_StoreDTO.Used);
        }
        private async Task Then_StoreScoutingId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.StoreScoutingId)));
            Assert.AreEqual(null, Store_StoreDTO.StoreScoutingId);
        }
        private async Task Then_StoreStatusId(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.StoreStatusId)));
            Assert.AreEqual(0, Store_StoreDTO.StoreStatusId);
        }
        private async Task Then_IsStoreApprovalDirectSalesOrder(Store_StoreDTO Store_StoreDTO)
        {
            Assert.AreEqual(false, Store_StoreDTO.Errors.ContainsKey(nameof(Store_StoreDTO.IsStoreApprovalDirectSalesOrder)));
            Assert.AreEqual(null, Store_StoreDTO.IsStoreApprovalDirectSalesOrder);
        }
    }
}
