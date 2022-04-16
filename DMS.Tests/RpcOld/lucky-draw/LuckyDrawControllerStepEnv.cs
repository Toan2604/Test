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
using DMS.Rpc.lucky_draw;
namespace DMS.Tests.Rpc.lucky_draw
{
    public partial class LuckyDrawControllerFeature
    {
       
        private async Task LoadExcel(string path)
        {
            System.IO.MemoryStream MemoryStream = ReadFile(path);
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
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
                ExcelWorksheet wsStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Status)).FirstOrDefault();
                if (wsStatus != null)
                    await Given_Status(wsStatus);
                ExcelWorksheet wsWard = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Ward)).FirstOrDefault();
                if (wsWard != null)
                    await Given_Ward(wsWard);
                ExcelWorksheet wsOrganization = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Organization)).FirstOrDefault();
                if (wsOrganization != null)
                    await Given_Organization(wsOrganization);
                ExcelWorksheet wsLuckyDrawType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawType)).FirstOrDefault();
                if (wsLuckyDrawType != null)
                    await Given_LuckyDrawType(wsLuckyDrawType);
                ExcelWorksheet wsImage = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Image)).FirstOrDefault();
                if (wsImage != null)
                    await Given_Image(wsImage);
                ExcelWorksheet wsAppUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUser)).FirstOrDefault();
                if (wsAppUser != null)
                    await Given_AppUser(wsAppUser);
                ExcelWorksheet wsStoreGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreGrouping)).FirstOrDefault();
                if (wsStoreGrouping != null)
                    await Given_StoreGrouping(wsStoreGrouping);
                ExcelWorksheet wsStoreType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreType)).FirstOrDefault();
                if (wsStoreType != null)
                    await Given_StoreType(wsStoreType);
                ExcelWorksheet wsStoreScouting = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScouting)).FirstOrDefault();
                if (wsStoreScouting != null)
                    await Given_StoreScouting(wsStoreScouting);
                ExcelWorksheet wsStore = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Store)).FirstOrDefault();
                if (wsStore != null)
                    await Given_Store(wsStore);
                ExcelWorksheet wsLuckyDraw = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDraw)).FirstOrDefault();
                if (wsLuckyDraw != null)
                    await Given_LuckyDraw(wsLuckyDraw);
                ExcelWorksheet wsLuckyDrawStoreGroupingMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStoreGroupingMapping)).FirstOrDefault();
                if (wsLuckyDrawStoreGroupingMapping != null)
                    await Given_LuckyDrawStoreGroupingMapping(wsLuckyDrawStoreGroupingMapping);
                ExcelWorksheet wsLuckyDrawStoreMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStoreMapping)).FirstOrDefault();
                if (wsLuckyDrawStoreMapping != null)
                    await Given_LuckyDrawStoreMapping(wsLuckyDrawStoreMapping);
                ExcelWorksheet wsLuckyDrawStoreTypeMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStoreTypeMapping)).FirstOrDefault();
                if (wsLuckyDrawStoreTypeMapping != null)
                    await Given_LuckyDrawStoreTypeMapping(wsLuckyDrawStoreTypeMapping);
                ExcelWorksheet wsLuckyDrawRegistration = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawRegistration)).FirstOrDefault();
                if (wsLuckyDrawRegistration != null)
                    await Given_LuckyDrawRegistration(wsLuckyDrawRegistration);
                ExcelWorksheet wsLuckyDrawStructure = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStructure)).FirstOrDefault();
                if (wsLuckyDrawStructure != null)
                    await Given_LuckyDrawStructure(wsLuckyDrawStructure);
                ExcelWorksheet wsLuckyDrawWinner = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawWinner)).FirstOrDefault();
                if (wsLuckyDrawWinner != null)
                    await Given_LuckyDrawWinner(wsLuckyDrawWinner);
            }
        }
    }
}
