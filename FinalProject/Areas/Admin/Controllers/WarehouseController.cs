using static Common.Constant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using static Common.RoleConstant;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class WarehouseController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProviderService _providerService;
        private readonly IReceiptService _receiptService;
        private readonly IRecommendationService _recommendationService;

        public WarehouseController(IProductService productService, IProviderService providerService, IReceiptService receiptService, 
            IRecommendationService recommendationService)
        {
            _productService = productService;
            _providerService = providerService;
            _receiptService = receiptService;
            _recommendationService = recommendationService;
        }
        public async Task<IActionResult> ViewRecommendation()
        {
            ViewBag.ProductOutOfStock = await _productService.GetProductDetailsRunOutOfStockAsync();
            return View();
        }

        public string GetRecommandation()
        {

           var result = _recommendationService.GetRecommandtion(2,0.5);
            var recommandation = new List<JObject>();

            foreach (var item in result)
            {
                var obj = new JObject
                {
                    { "productDetailId", item.ProductDetailId },
                    { "productName", item.Product.ProductName },
                    { "color", item.Color },
                    { "length", item.Length},
                    { "height", item.Height },
                    { "width", item.Width },
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
            var result = await _productService.BestSellerInMonthAsync(fromDate,toDate,quantity);

            var bestSellerList = new List<JObject>();

            foreach (var item in result)
            {
                var obj = new JObject
                {
                    { "productDetailId", item.ProductDetailId },
                    { "productName", item.Product.ProductName },
                    { "color", item.Color },
                    { "length", item.Length},
                    { "height", item.Height },
                    { "width", item.Width },
                    { "totalQuantity", item.Quantity },
                    { "quantityOrdered", item.QuantityOrdered },
                    { "RemainingQuantity", item.RemainingQuantity }
                };

                bestSellerList.Add(obj);
            }

            return JsonConvert.SerializeObject(bestSellerList);
        }


        public IActionResult Index()
        {
            return View();
        }
       

        public async Task<IActionResult> RejectReceipt(int id)
        {
            if (await _receiptService.RejectReceiptRequestAsync(id) > 0)
                ViewBag.Message = "Xóa thành công!";
            return Redirect("~Area/Admin/Warehouse/ListReceiptRequest");
        }

        
        public async Task<IActionResult> ApproveReceipt(int id)
        {
            if (await _receiptService.ApproveReceiptRequestAsync(id) > 0)
            {
                await _receiptService.AddReceiptAsync(id);
                ViewBag.Message = "Đã duyệt!";
            }
                
            return Redirect("~Area/Admin/Warehouse/ListReceiptRequest");
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


        [HttpGet]
        public JsonResult GetProvider(int productId)
        {
            return Json(_providerService.GetProvidersByProduct(productId));
        }
    }
}
