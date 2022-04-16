using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Models;
using DMS.Rpc;
using DMS.Services.MImage;
using LightBDD.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Tests
{
    public class MockRabbitManager : IRabbitManager
    {
        public void PublishList<T>(List<T> message, string routeKey) where T : DataEntity
        {
        }
        public void PublishSingle<T>(T message, string routeKey) where T : DataEntity
        {
        }
    }
    public class MockLogging : ILogging
    {
        public void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "")
        {
        }
        public void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "")
        {
        }
    }
    public class MockImageService : IImageService
    {
        public MockImageService()
        {
        }
        public Task<int> Count(ImageFilter ImageFilter)
        {
            throw new NotImplementedException();
        }
        public async Task<Image> Create(Image Image, string path, string thumbnailPath, int width, int height)
        {
            return null;
        }
        public async Task<Image> Create(Image Image, string path)
        {
            return null;
        }
        public async Task<Image> Create(Image Image)
        {
            return null;
        }
        public async Task<Image> Delete(Image Image)
        {
            return null;
        }
        public async Task<Image> Get(long Id)
        {
            return null;
        }
        public async Task<List<Image>> Import(List<Image> Images)
        {
            return null;
        }
        public async Task<List<Image>> List(ImageFilter ImageFilter)
        {
            return null;
        }
        public async Task<ImageFilter> ToFilter(ImageFilter ImageFilter)
        {
            return null;
        }
        public async Task<Image> Update(Image Image)
        {
            return null;
        }
        public async Task<List<Image>> BulkDelete(List<Image> Images)
        {
            return null;
        }
    }
}
