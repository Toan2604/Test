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
using DMS.Rpc.dashboards.store_information;
using Newtonsoft.Json;

namespace DMS.Tests.Rpc.dashboard
{
    public partial class DashboardControllerFeature
    {
        private async Task Then_StoreCounter_Result(int TotalSurveyedStores, int TotalStores)
        {
            Assert.AreEqual(TotalSurveyedStores, DashboardStoreInformation_StoreCounterDTO.SurveyedStoreCounter);
            Assert.AreEqual(TotalStores, DashboardStoreInformation_StoreCounterDTO.StoreCounter);
        }
        private async Task Then_BrandStatistics_Result(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            List<DashboardStoreInformation_BrandStatisticsDTO> Expected = JsonConvert.DeserializeObject<List<DashboardStoreInformation_BrandStatisticsDTO>>(content);

            foreach (var BrandStatisticsDTO_Expected in Expected)
            {
                var SubBrandStatisticsDTO = DashboardStoreInformation_BrandStatisticsDTOs.Where(x => x.BrandId == BrandStatisticsDTO_Expected.BrandId).FirstOrDefault();
                Assert.AreEqual(BrandStatisticsDTO_Expected.BrandName, SubBrandStatisticsDTO.BrandName);
                Assert.AreEqual(BrandStatisticsDTO_Expected.Value, SubBrandStatisticsDTO.Value);
                Assert.AreEqual(BrandStatisticsDTO_Expected.Total, SubBrandStatisticsDTO.Total);
                Assert.AreEqual(BrandStatisticsDTO_Expected.Rate, SubBrandStatisticsDTO.Rate);
            }
        }

        private async Task Then_BrandUnStatistics_Result(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            List<DashboardStoreInformation_BrandStatisticsDTO> Expected = JsonConvert.DeserializeObject<List<DashboardStoreInformation_BrandStatisticsDTO>>(content);

            foreach (var BrandUnStatisticsDTO_Expected in Expected)
            {
                var SubBrandUnStatisticsDTO = DashboardStoreInformation_BrandUnStatisticsDTOs.Where(x => x.BrandId == BrandUnStatisticsDTO_Expected.BrandId).FirstOrDefault();
                Assert.AreEqual(BrandUnStatisticsDTO_Expected.BrandName, SubBrandUnStatisticsDTO.BrandName);
                Assert.AreEqual(BrandUnStatisticsDTO_Expected.Value, SubBrandUnStatisticsDTO.Value);
                Assert.AreEqual(BrandUnStatisticsDTO_Expected.Total, SubBrandUnStatisticsDTO.Total);
                Assert.AreEqual(BrandUnStatisticsDTO_Expected.Rate, SubBrandUnStatisticsDTO.Rate);
            }
        }

        private async Task Then_StoreCoverage_Result(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            List<DashboardStoreInformation_StoreDTO> Expected = JsonConvert.DeserializeObject<List<DashboardStoreInformation_StoreDTO>>(content);

            Assert.AreEqual(Expected.Count, DashboardStoreInformation_StoreDTOs.Count);
        }

        private async Task Then_ProductGroupingStatistics_Result(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            List<DashboardStoreInformation_ProductGroupingStatisticsDTO> Expected = JsonConvert.DeserializeObject<List<DashboardStoreInformation_ProductGroupingStatisticsDTO>>(content);

            foreach (var ProductGroupingStatistics_Expected in Expected)
            {
                var SubDTO = DashboardStoreInformation_ProductGroupingStatisticsDTOs.Where(x => x.BrandId == ProductGroupingStatistics_Expected.BrandId).FirstOrDefault();
                foreach (var ProductGrouping_Expected in ProductGroupingStatistics_Expected.ProductGroupings)
                {
                    var SubProductGrouping = SubDTO.ProductGroupings.Where(x => x.ProductGroupingId == ProductGrouping_Expected.ProductGroupingId).FirstOrDefault();
                    Assert.AreEqual(ProductGrouping_Expected.ProductGroupingName, SubProductGrouping.ProductGroupingName);
                    Assert.AreEqual(ProductGrouping_Expected.Value, SubProductGrouping.Value);
                    Assert.AreEqual(ProductGrouping_Expected.Total, SubProductGrouping.Total);
                    Assert.AreEqual(ProductGrouping_Expected.Rate, SubProductGrouping.Rate);
                }
            }
        }

        private async Task Then_ProductGroupingNumberStatistics_Result(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            List<DashboardStoreInformation_ProductGroupingNumberStatisticsDTO> Expected = JsonConvert.DeserializeObject<List<DashboardStoreInformation_ProductGroupingNumberStatisticsDTO>>(content);

            foreach (var ProductGroupingNumberStatistics_Expected in Expected)
            {
                var SubProductGroupingNumberStatistics = DashboardStoreInformation_ProductGroupingNumberStatisticsDTOs
                    .Where(x => x.BrandId == ProductGroupingNumberStatistics_Expected.BrandId).FirstOrDefault();
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.BrandName, SubProductGroupingNumberStatistics.BrandName);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Total, SubProductGroupingNumberStatistics.Total);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Value3, SubProductGroupingNumberStatistics.Value3);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Value4, SubProductGroupingNumberStatistics.Value4);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Value5, SubProductGroupingNumberStatistics.Value5);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Value6, SubProductGroupingNumberStatistics.Value6);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Value7, SubProductGroupingNumberStatistics.Value7);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Value8, SubProductGroupingNumberStatistics.Value8);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Rate3, SubProductGroupingNumberStatistics.Rate3);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Rate4, SubProductGroupingNumberStatistics.Rate4);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Rate5, SubProductGroupingNumberStatistics.Rate5);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Rate6, SubProductGroupingNumberStatistics.Rate6);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Rate7, SubProductGroupingNumberStatistics.Rate7);
                Assert.AreEqual(ProductGroupingNumberStatistics_Expected.Rate8, SubProductGroupingNumberStatistics.Rate8);
            }
        }

        private async Task Then_EstimatedRevenueStatistics_Result(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            List<DashboardStoreInformation_EstimatedRevenueStatisticsDTO> Expected = JsonConvert.DeserializeObject<List<DashboardStoreInformation_EstimatedRevenueStatisticsDTO>>(content);

            foreach (var EstimatedRevenueStatisticsDTO_Expected in Expected)
            {
                var SubEstimatedRevenueStatisticsDTO = DashboardStoreInformation_EstimatedRevenueStatisticsDTOs
                    .Where(x => x.EstimatedRevenueId == EstimatedRevenueStatisticsDTO_Expected.EstimatedRevenueId).FirstOrDefault();
                Assert.AreEqual(EstimatedRevenueStatisticsDTO_Expected.EstimatedRevenueName, SubEstimatedRevenueStatisticsDTO.EstimatedRevenueName);
                Assert.AreEqual(EstimatedRevenueStatisticsDTO_Expected.Value, SubEstimatedRevenueStatisticsDTO.Value);
                Assert.AreEqual(EstimatedRevenueStatisticsDTO_Expected.Total, SubEstimatedRevenueStatisticsDTO.Total);
                Assert.AreEqual(EstimatedRevenueStatisticsDTO_Expected.Rate, SubEstimatedRevenueStatisticsDTO.Rate);
            }
        }
        private async Task Then_TopBrand_Result(string path)
        {
            string content = System.IO.File.ReadAllText(path);
            List<DashboardStoreInformation_TopBrandDTO> Expected = JsonConvert.DeserializeObject<List<DashboardStoreInformation_TopBrandDTO>>(content);

            foreach (var TopBrandDTO_Expected in Expected)
            {
                var SubTopBrandDTO = DashboardStoreInformation_TopBrandDTOs
                    .Where(x => x.BrandId == TopBrandDTO_Expected.BrandId).FirstOrDefault();
                Assert.AreEqual(TopBrandDTO_Expected.BrandName, SubTopBrandDTO.BrandName);
                Assert.AreEqual(TopBrandDTO_Expected.Value, SubTopBrandDTO.Value);
                Assert.AreEqual(TopBrandDTO_Expected.Total, SubTopBrandDTO.Total);
                Assert.AreEqual(TopBrandDTO_Expected.Rate, SubTopBrandDTO.Rate);
            }
        }
    }
}
