using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using Common;

namespace Services.Services
{
    public class FastDeliveryService : IFastDeliveryService
    {
        private readonly IConfiguration _configuration;

        public FastDeliveryService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CaculateFeeShipAsync(int fromDistrictId, int serviceId, int toDistrictId, int width, int height,
            int weight, int length, int fee)
        {
            try
            {
                var httpWReq = (HttpWebRequest)WebRequest.Create(_configuration[Constant.FAST_DELIVERY_URL_API]);

                var message = new JObject
                {
                    { Constant.FAST_DELIVERY_FROM_DISTRICT_ID, 1454},
                    { Constant.FAST_DELIVERY_SERVICE_ID, 53320 },
                    { Constant.FAST_DELIVERY_SERVICE_TYPE_ID, null },
                    { Constant.FAST_DELIVERY_TO_DISTRICT_ID, 1452 },
                    { Constant.FAST_DELIVERY_TO_WARD_CODE, "21012" },
                    { Constant.FAST_DELIVERY_ITEM_HEIGHT, 50 },
                    { Constant.FAST_DELIVERY_ITEM_LENGTH, 20 },
                    { Constant.FAST_DELIVERY_ITEM_WEIGHT, 200 },
                    { Constant.FAST_DELIVERY_ITEM_WIDTH, 20 },
                    { Constant.FAST_DELIVERY_INSURANCE_FEE, 10000 },
                    { Constant.FAST_DELIVERY_COUPON, null }
                };

                var postData = message.ToString();

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = Constant.METHOD_POST;
                httpWReq.Headers.Add(HttpConstant.HEADER_CONTENT_TYPE, HttpConstant.HEADER_CONTENT_TYPE_JSON);
                httpWReq.Headers.Add(HttpConstant.HEADER_CONTENT_TYPE, HttpConstant.HEADER_CONTENT_TYPE_TEXT);
                httpWReq.Headers.Add(Constant.FAST_DELIVERY_SHOP_ID_NAME, _configuration[Constant.FAST_DELIVERY_SHOP_ID]);
                httpWReq.Headers.Add(Constant.FAST_DELIVERY_TOKEN_NAME, _configuration[Constant.FAST_DELIVERY_TOKEN]);

                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                var stream = await httpWReq.GetRequestStreamAsync();
                await stream.WriteAsync(data.AsMemory(0, data.Length));
                stream.Close();

                var response = await httpWReq.GetResponseAsync() as HttpWebResponse;

                string jsonresponse = Constant.EMPTY;

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }

                return jsonresponse;

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }
    }
}
