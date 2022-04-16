using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OfficeOpenXml;
using TrueSight.Common;
using DMS.Models;
using DMS.Enums;
using DMS.Entities;
using DMS.Helpers;
using DMS.Rpc.store_balance;
namespace DMS.Tests.Rpc.store_balance
{
    public partial class StoreBalanceControllerFeature
    {
        private async Task LoadExcel(string path)
        {
            System.IO.MemoryStream MemoryStream = ReadFile(path);
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                ExcelWorksheet wsStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Status)).FirstOrDefault();
                if (wsStatus != null)
                    await Given_Status(wsStatus);
                ExcelWorksheet wsStoreStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreStatus)).FirstOrDefault();
                if (wsStoreStatus != null)
                    await Given_StoreStatus(wsStoreStatus);
                ExcelWorksheet wsStoreScoutingStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScoutingStatus)).FirstOrDefault();
                if (wsStoreScoutingStatus != null)
                    await Given_StoreScoutingStatus(wsStoreScoutingStatus);
                ExcelWorksheet wsStoreScoutingType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScoutingType)).FirstOrDefault();
                if (wsStoreScoutingType != null)
                    await Given_StoreScoutingType(wsStoreScoutingType);
                ExcelWorksheet wsColor = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Color)).FirstOrDefault();
                if (wsColor != null)
                    await Given_Color(wsColor);
                ExcelWorksheet wsSex = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Sex)).FirstOrDefault();
                if (wsSex != null)
                    await Given_Sex(wsSex);
                ExcelWorksheet wsOrganization = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Organization)).FirstOrDefault();
                if (wsOrganization != null)
                    await Given_Organization(wsOrganization);
                ExcelWorksheet wsAppUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUser)).FirstOrDefault();
                if (wsAppUser != null)
                    await Given_AppUser(wsAppUser);
                ExcelWorksheet wsStoreGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreGrouping)).FirstOrDefault();
                if (wsStoreGrouping != null)
                    await Given_StoreGrouping(wsStoreGrouping);
                ExcelWorksheet wsStoreType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreType)).FirstOrDefault();
                if (wsStoreType != null)
                    await Given_StoreType(wsStoreType);
                ExcelWorksheet wsProvince = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Province)).FirstOrDefault();
                if (wsProvince != null)
                    await Given_Province(wsProvince);
                ExcelWorksheet wsDistrict = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(District)).FirstOrDefault();
                if (wsDistrict != null)
                    await Given_District(wsDistrict);
                ExcelWorksheet wsWard = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Ward)).FirstOrDefault();
                if (wsWard != null)
                    await Given_Ward(wsWard);
                ExcelWorksheet wsStoreScouting = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScouting)).FirstOrDefault();
                if (wsStoreScouting != null)
                    await Given_StoreScouting(wsStoreScouting);
                ExcelWorksheet wsStore = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Store)).FirstOrDefault();
                if (wsStore != null)
                    await Given_Store(wsStore);
                ExcelWorksheet wsStoreBalance = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreBalance)).FirstOrDefault();
                if (wsStoreBalance != null)
                    await Given_StoreBalance(wsStoreBalance);
            }
        }
    }
}
