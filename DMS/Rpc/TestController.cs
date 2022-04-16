using DMS.Models;
using DMS.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DMS.Rpc
{
    public class TestController : ControllerBase
    {
        private IServiceProvider ServiceProvider;
        private DataContext DataContext;
        public TestController(IServiceProvider ServiceProvider, DataContext DataContext)
        {
            this.DataContext = DataContext;
            this.ServiceProvider = ServiceProvider;
        }

        [Route("rpc/test/test"), HttpGet]
        public async Task Test()
        {
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    Parallel.For(1, 200, async x =>
                     {
                         try
                         {
                             using (var scope = ServiceProvider.CreateScope())
                             {
                                 var sp = scope.ServiceProvider;
                                 IUOW UOW = sp.GetService<IUOW>();
                                 List<string> result = await UOW.PermissionRepository.ListPath(2);
                                 UOW.Dispose();
                                 Console.WriteLine($"{x}:{result.Count}");
                             }
                         }
                         catch (Exception ex)
                         {
                             Console.WriteLine(ex.Message);
                         }
                     });
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
            }
        }

        //[Route("rpc/test/test-query"), HttpGet]
        //public async Task TestQuery()
        //{
        //    try
        //    {
        //        for (long i = 0; i < 100; i++)
        //        {
        //            int milliseconds = 1000;
        //            await Task.Delay(milliseconds);
        //            TestDAO TestDAO = new TestDAO();
        //            TestDAO.Count = i;
        //            await DataContext.BulkMergeAsync(new List<TestDAO> { TestDAO });

        //        }
        //    }
        //    catch (Exception ex2)
        //    {
        //        Console.WriteLine(ex2.Message);
        //    }
        //}
    }
}
