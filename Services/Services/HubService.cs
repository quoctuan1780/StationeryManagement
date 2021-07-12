using Entities.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Constant;

namespace Services.Services
{
    public class HubService : IHubService
    {
        private readonly ShopDbContext _context;

        public HubService(ShopDbContext context)
        {
            _context = context;
        }
        public async Task<int> GetOrdersAsync()
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_CONFIRM)).ToListAsync();

            return result.Count;
        }

        public async Task<int> GetWarehouseAsync()
        {
            var result = await _context.ReceiptRequests.Where(x => x.Status.Equals(RECEIPT_REQUEST_STATUS_WAITING)).ToListAsync();

            return result.Count;
        }

        public async Task<string> GetAccountAsync()
        {
            var result = new JObject();

            var preMonth = DateTime.Now.Month;
            var preYear = DateTime.Now.Year;

            if (preMonth - 1 == 0)
            {
                preMonth = 12;
                preYear -= 1;
            }
            else
            {
                preMonth -= 1;
            }

            var accountNow = await _context.Users.Where(x => x.CreateDate.Month == DateTime.Now.Month && x.CreateDate.Year == DateTime.Now.Year).ToListAsync();

            var accountPre = await _context.Users.Where(x => x.CreateDate.Month == preMonth && x.CreateDate.Year == preYear).ToListAsync();

            result.Add("month1", accountPre.Count);
            result.Add("month2", accountNow.Count);

            return result.ToString();
        }

        public async Task<string> GetRevenueAsync()
        {
            var result = new JObject();

            var preMonth = DateTime.Now.Month;
            var preYear = DateTime.Now.Year;

            if (preMonth - 1 == 0)
            {
                preMonth = 12;
                preYear -= 1;
            }
            else
            {
                preMonth -= 1;
            }

            var currentRevenue = await _context.Bills.Where(x => x.CreateDate.Month == DateTime.Now.Month && x.CreateDate.Year == DateTime.Now.Year ).SumAsync(x => x.Total);

            var preRevenue = await _context.Orders.Where(x => x.OrderDate.Month == preMonth && x.OrderDate.Year == preYear ).SumAsync(x => x.Total);

            result.Add("CurrentRevenue", currentRevenue);
            result.Add("PreRevenue", preRevenue);

            return result.ToString();
        }

        public async Task<string> GetTopProductAsync()
        {
            var result = (from od in _context.BillDetails
                         join o in _context.Bills on od.BillId equals o.BillId
                         where o.CreateDate.Month == DateTime.Now.Month && o.CreateDate.Year == DateTime.Now.Year
                         group od by od.ProductDetailId into g
                         select new
                         {
                             ProductDetailsId = g.Key,
                             Total = g.Sum(x => x.Quantity * x.Price)
                         }
                         into d
                         orderby d.Total descending
                         select d).Take(10);

            var dataChart = new List<JObject>();

            if(result.Any())
            {
                var products = await _context.ProductDetails.Where(x => result.Select(x => x.ProductDetailsId).Contains(x.ProductDetailId)).Include(x => x.Product).ToListAsync();

                var sumTopProduct = result.Sum(x => x.Total);

                foreach(var item in result)
                {
                    var label = products.Find(x => x.ProductDetailId == item.ProductDetailsId);
                    dynamic json = new JObject();
                    json.y = Math.Round(item.Total / sumTopProduct * 100, 2);
                    json.label = label.Product.ProductName + " " + label.Color;
                    dataChart.Add(json);
                }
            }

            return JsonConvert.SerializeObject(dataChart);
        }

        public async Task<string> GetTopCustomerAsync()
        {
            var result = (from c in _context.Users
                          join o in _context.Bills on c.Id equals o.UserId
                          where o.CreateDate.Month == DateTime.Now.Month && o.CreateDate.Year == DateTime.Now.Year
                          group o by new { o.UserId, c.FullName } into g
                          select new
                          {
                              UserId = g.Key.UserId,
                              FullName = g.Key.FullName,
                              Total = g.Sum(x => x.Total)
                          }
                          into d
                          orderby d.Total descending
                          select d).Take(10);

            var dataChart = new List<JObject>();

            if (result.Any())
            {
                var sumOfTopCustomer = await result.SumAsync(x => x.Total);
                foreach (var item in result)
                {
                    dynamic json = new JObject();
                    json.y = Math.Round(item.Total / sumOfTopCustomer * 100, 2);
                    json.label = item.FullName;
                    dataChart.Add(json);
                }
            }

            return JsonConvert.SerializeObject(dataChart);
        }

        public async Task<string> GetRevenueCurrentMonthAsync()
        {
            var result = await _context.Bills.Where(x => x.CreateDate.Month == DateTime.Now.Month && x.CreateDate.Year == DateTime.Now.Year).GroupBy(x => x.CreateDate.Day).Select(x => new { Day = x.Key, Total = x.Sum(y => y.Total) }).ToListAsync();

            var dataChart = new List<JObject>();

            if (result.Any())
            {
                foreach (var item in result)
                {
                    dynamic json = new JObject();
                    json.x = string.Concat(DateTime.Now.Year, SLASH, DateTime.Now.Month, SLASH, item.Day);
                    json.y = Math.Round(item.Total);
                    dataChart.Add(json);
                }
            }

            return JsonConvert.SerializeObject(dataChart);
        }

        public async Task<int> GetOrderWaitToReject()
        {
            var result = await _context.Orders
                .Where(x => x.Status.Equals(STATUS_PENDING_ADMIN_CANCED_ORDER)).ToListAsync();

            return result.Count;
        }
    }
}
