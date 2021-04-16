using Common;
using Entities.Data;
using Entities.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class MoMoService : IMoMoService
    {
        private ShopDbContext _context;
        private IConfiguration _configuration;

        public MoMoService(ShopDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public  async Task<int> AddMoMoPaymentAsync(int orderId, string moMoOrderId, string payType, string responseTime)
        {
            var checkDatetime = DateTime.TryParse(responseTime, out DateTime responseTimeParse);
            if (checkDatetime) {
                var moMoPayment = new MoMoPayment()
                {
                    MoMoOrderId = moMoOrderId,
                    PayType = payType,
                    ResponseTime = responseTimeParse,
                    OrderId = orderId
                };

                await _context.AddAsync(moMoPayment);

                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public string MoMoCheckout(string total, string orderInfo, string email, string hostName)
        {
            
            string endPoint = _configuration[Constant.MOMO_LINK];

            string partnerCode = _configuration[Constant.MOMO_PARTNER_CODE];
            string accessKey = _configuration[Constant.MOMO_ACCESS_KEY];
            string serectkey = _configuration[Constant.MOMO_SECRET_KEY];
            string returnUrl = $"{hostName}/Order/MoMoSuccess";
            string notifyurl = $"{hostName}/Order/MoMoFail";
            string amount = total;
            string orderId = Guid.NewGuid().ToString();
            string requestId = Guid.NewGuid().ToString();
            string extraData = email;

            string rawHash = Constant.MOMO_SHA_PARTNER_CODE +
                partnerCode + Constant.MOMO_SHA_ACCESS_KEY +
                accessKey + Constant.MOMO_SHA_REQUEST_ID +
                requestId + Constant.MOMO_SHA_AMOUNT +
                amount + Constant.MOMO_SHA_ORDER_ID +
                orderId + Constant.MOMO_SHA_ORDER_INFO +
                orderInfo + Constant.MOMO_SHA_RETURN_URL +
                returnUrl + Constant.MOMO_SHA_NOTIFY_URL +
                notifyurl + Constant.MOMO_SHA_EXTRA_DATA +
                extraData;

            //sign signature SHA256
            string signature = SignSHA256(rawHash, serectkey);

            //build body json request
            var message = new JObject
            {
                { Constant.MOMO_JSON_PARTNER_CODE, partnerCode },
                { Constant.MOMO_JSON_ACCESS_KEY, accessKey },
                { Constant.MOMO_JSON_REQUEST_ID, requestId },
                { Constant.MOMO_JSON_AMOUNT, amount },
                { Constant.MOMO_JSON_ORDER_ID, orderId },
                { Constant.MOMO_JSON_ORDER_INFO, orderInfo },
                { Constant.MOMO_JSON_RETURN_URL, returnUrl },
                { Constant.MOMO_JSON_NOTIFY_URL, notifyurl },
                { Constant.MOMO_JSON_EXTRA_DATA, extraData },
                { Constant.MOMO_JSON_REQUEST_TYPE, Constant.MOMO_JSON_CAPTURE_MOMO_WALLET },
                { Constant.MOMO_JSON_SIGNATURE, signature }
            };
            //Before sign HMAC SHA256 signature

            string responseFromMomo = SendPaymentRequest(endPoint, message.ToString());

            return responseFromMomo;
        }

        public string SendPaymentRequest(string endpoint, string postJsonString)
        {
            try
            {
                var httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                var postData = postJsonString;

                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                var stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {

                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }


                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);

            }
            catch (WebException e)
            {
                return e.Message;
            }
        }

        public string SignSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            string hex = BitConverter.ToString(hashmessage);
            hex = hex.Replace("-", "").ToLower();
            return hex;
        }
    }
}
