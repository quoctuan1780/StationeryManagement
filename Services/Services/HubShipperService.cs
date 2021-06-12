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
    public class HubShipperService : IHubShipperService
    {
        private readonly ShopDbContext _context;

        public HubShipperService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetOrderDeliveredAsync(string userId)
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_RECEIVED_GOODS) && x.ShipperId == userId).ToListAsync();

            return result.Count;
        }

        public async Task<int> GetOrderDeliveringAsync(string userId)
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_ON_DELIVERY_GOODS) && x.ShipperId == userId).ToListAsync();

            return result.Count;
        }

        public async Task<string> GetOrderDeliveringOrDeliveriedAsync(string userId)
        {
            var orderDelivering = await _context.Orders.Where(x => x.ShipperId == userId && x.Status.Equals(STATUS_ON_DELIVERY_GOODS)).ToListAsync();

            var orderDelivered = await _context.Orders.Where(x => x.ShipperId == userId && x.Status.Equals(STATUS_RECEIVED_GOODS)).ToListAsync();

            double resultDelivering = 0;

            double resultDelivered = 0;

            if(orderDelivering.Count == 0)
            {
                resultDelivered = 100;
            }
            else if(orderDelivered.Count == 0)
            {
                resultDelivering = 100;
            }
            else
            {
                resultDelivering = Math.Round((double)orderDelivering.Count / (orderDelivered.Count + orderDelivering.Count) * 100, 2);
                resultDelivered = 100.00 - resultDelivering;
            }

            #region Convert to Json
            dynamic jsonDelivering = new JObject();
            jsonDelivering.y = resultDelivering;
            jsonDelivering.label = "Đơn hàng đang giao";
            dynamic jsonDelivered = new JObject();
            jsonDelivered.y = resultDelivered;
            jsonDelivered.label = "Đơn hàng đã giao";
            #endregion

            var jObject = new List<JObject>
            {
                jsonDelivering,
                jsonDelivered
            };

            return JsonConvert.SerializeObject(jObject);
        }

        public async Task<int> GetOrderWaitToConfirmDeliveryAsync(string userId)
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS) && x.ShipperId == userId).ToListAsync();

            return result.Count;
        }

        public async Task<int> GetOrderWaitToPickAsync()
        {
            var result = await _context.Orders.Where(x => x.Status.Equals(STATUS_WAITING_PICK_GOODS) && x.ShipperId == null).ToListAsync();

            return result.Count;
        }
    }
}
