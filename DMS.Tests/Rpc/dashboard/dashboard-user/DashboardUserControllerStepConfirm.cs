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
namespace DMS.Tests.Rpc.dashboard
{
    public partial class DashboardUserControllerFeature
    {
        private async Task Then_Result(long quanlity)
        {
            Assert.AreEqual(quanlity, Result);
        }
        private async Task Then_DecimalResult(decimal quanlity)
        {
            Assert.AreEqual(quanlity, DecimalResult);
        }
    }
}
