using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Hubs;
using Services.Interfacies;
using System;
using static Common.Constant;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area("Warehouse")]
    public class RecommandationController : Controller
    {
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IProductService _productService;
        private readonly IReceiptService _receiptService;
        private readonly IRecommendationService _recommendationService;
        private readonly UserManager<User> _userManager;
        static DateTime FromDate;
        static DateTime ToDate;
        static int Quantity;

        public RecommandationController(IProductService productService, IReceiptService receiptService,
            IRecommendationService recommendationService, IHubContext<SignalServer> hubContext, UserManager<User> userManager)
        {
            _hubContext = hubContext;
            _productService = productService;
            _receiptService = receiptService;
            _recommendationService = recommendationService;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ViewRecommandation()
        {
            ViewBag.ProductOutOfStock = await _productService.GetProductDetailsRunOutOfStockAsync();
            return View();
        }

        public async Task<string> GetRecommandation()
        {
            var listId = await _productService.ListBestSellerProduct(FromDate, ToDate, Quantity);
            var result = await _recommendationService.GetRecommandtion(4, 0.81,listId);
            var recommandation = new List<JObject>();

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
            return await _productService.BestSellerInMonthAsync(fromDate, toDate, quantity);

        }
        public async Task<IActionResult> AutoCreateReceiptRequest()
        {
            ViewBag.ProductOutOfStock = await _productService.GetProductDetailsRunOutOfStockAsync();
            ViewBag.BestSeller = await _productService.BestSellerInMonthAsync(FromDate, ToDate, Quantity);
            var listId = await _productService.ListBestSellerProduct(FromDate, ToDate, Quantity);
            ViewBag.Recommandation = await _recommendationService.GetRecommandtion(4, 0.81,listId);
           
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AutoCreateReceiptRequest(IList<int> selected, IList<int> quantity)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                ReceiptRequest rr = new()
                {
                    CreateDate = DateTime.Now,
                    UserId = _userManager.GetUserId(User),
                    Status = RECEIPT_REQUEST_STATUS_WAITING
                };
                if (await _receiptService.AddReceiptRequestAsync(rr))
                {
                    List<ReceiptRequestDetail> list = new();
                    for (int i = 0; i < quantity.Count; i++)
                    {
                        var detail = new ReceiptRequestDetail()
                        {
                            ReceiptRequestId = rr.ReceiptRequestId,
                            ProductDetailId = selected[i],
                            Quantity = quantity[i],
                            Status = RECEIPT_STATUS_WAITING
                        };
                        list.Add(detail);
                    }
                    if (await _receiptService.AddReceiptRequestDetailAsync(list) > 0)
                    {
                        transaction.Complete();
                        return Redirect("/Warehouse/Home/ViewListRequestReceipt");
                    }

                }
            }
            catch (Exception)
            {

                ViewBag.Message("Không thể thêm phiếu yêu cầu nhập hàng!");
                return View();
            }

            return View();
        }
    }
}
