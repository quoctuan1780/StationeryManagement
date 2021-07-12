using static Common.Constant;
using static Common.SignalRConstant;
using static Common.RoleConstant;
using System;
using System.Linq;
using Services.Hubs;
using Newtonsoft.Json;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class WarehouseController : Controller
    {
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IProductService _productService;
        private readonly IReceiptService _receiptService;
        private readonly IRecommendationService _recommendationService;
        private static DateTime FromDate;
        private static DateTime ToDate;
        private static int Quantity;

        public WarehouseController(IProductService productService, IReceiptService receiptService, 
            IRecommendationService recommendationService, IHubContext<SignalServer> hubContext)
        {
            _hubContext = hubContext;
            _productService = productService;
            _receiptService = receiptService;
            _recommendationService = recommendationService;
        }
        public async Task<IActionResult> ViewRecommendation()
        {
            ViewBag.ProductOutOfStock = await _productService.GetProductDetailsRunOutOfStockAsync();
            return View();
        }

        public async Task<string> GetRecommandation()
        {
            var listId = await _productService.ListBestSellerProduct(FromDate, ToDate, Quantity);
            var result = await _recommendationService.GetRecommandtion(listId);
            var recommandation = new List<JObject>();

            if(result is null || !result.Any())
            {
                return NULL;
            }

            foreach (var item in result)
            {
                var obj = new JObject
                {
                    { "productDetailId", item.ProductDetailId },
                    { "productName", item.Product.ProductName },
                    { "color", item.Color },
                    { "totalQuantity", item.Quantity },
                    { "quantityOrdered", item.QuantityOrdered },
                    { "RemainingQuantity", item.RemainingQuantity }
                };

                recommandation.Add(obj);
            }

            return JsonConvert.SerializeObject(recommandation);
        }

        public async Task<string> GetBestSeller(DateTime fromDate, DateTime toDate, int quantity)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Quantity = quantity;
            var result = await _productService.BestSellerInMonthAsync(fromDate, toDate, quantity);
            if(result is null || result.Equals("[]"))
            {
                return NULL;
            }

            return result;
        }

               

        public async Task<IActionResult> RejectReceipt(int? id)
        {
            if (id is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }
            if (await _receiptService.RejectReceiptRequestAsync(id.Value) > 0)
                ViewBag.Message = "Từ chối thành công!";
            var result = await _receiptService.RejectReceiptRequestAsync(id.Value);

            if (result > 0)
            {
                ViewBag.Message = "Xóa thành công!";
            }

            return Redirect("/Admin/Warehouse/ListReceiptRequest");
        }

        
        public async Task<IActionResult> ApproveReceipt(int? id)
        {
            if (id is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _receiptService.ApproveReceiptRequestAsync(id.Value);
                if (result > 0)
                {
                    await _hubContext.Clients.Group(SIGNAL_GROUP_WAREHOUSE).SendAsync(SIGNAL_COUNT_RECEPT_REQUEST_ACCEPT);
                    result = await _receiptService.AddReceiptAsync(id.Value);
                    if(result > 0)
                    {
                        transaction.Complete();

                        ViewBag.Message = "Đã duyệt!";
                    }
                }
                
            }
            catch
            {
            }

            return Redirect("/Admin/Warehouse/ListReceiptRequest");
        }
      
        public async Task<string> GetColor(int productID)
        {
            var listColor = await _productService.GetColorByIdAsync(productID);
            return JsonConvert.SerializeObject(listColor);
        }

        public async Task<IActionResult> ListReceiptRequest()
        {
            ViewBag.ListReceipts = await _receiptService.GetReceiptRequestsAsync();
            return View();
        }
        public async Task<IActionResult> ViewRequestReceipt(int? id)
        {
            if(id is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            ViewBag.Request = await _receiptService.GetReceiptRequestAsync(id.Value);

            return View();
        }
    }
}
