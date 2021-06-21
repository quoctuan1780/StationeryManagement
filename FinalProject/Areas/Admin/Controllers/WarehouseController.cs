using static Common.Constant;
using static Common.SignalRConstant;
using static Common.RoleConstant;
using static Common.MessageConstant;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Services.Hubs;
using System.Transactions;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class WarehouseController : Controller
    {
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IProductService _productService;
        private readonly IProviderService _providerService;
        private readonly IReceiptService _receiptService;
        private readonly IRecommendationService _recommendationService;

        public WarehouseController(IProductService productService, IProviderService providerService, IReceiptService receiptService, 
            IRecommendationService recommendationService, IHubContext<SignalServer> hubContext)
        {
            _hubContext = hubContext;
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

        public async Task<string> GetRecommandation()
        {

           var result = await _recommendationService.GetRecommandtion(2,0.5);
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
            return await _productService.BestSellerInMonthAsync(fromDate,toDate,quantity);
        }


        public IActionResult Index()
        {
            return View();
        }
       

        public async Task<IActionResult> RejectReceipt(int id)
        {
            if (await _receiptService.RejectReceiptRequestAsync(id) > 0)
                ViewBag.Message = "Xóa thành công!";
            return Redirect("/Admin/Warehouse/ListReceiptRequest");
        }

        
        public async Task<IActionResult> ApproveReceipt(int id)
        {
            if (await _receiptService.ApproveReceiptRequestAsync(id) > 0)
            {
                await _hubContext.Clients.Group(SIGNAL_GROUP_WAREHOUSE).SendAsync("AcceptOrders");
                await _receiptService.AddReceiptAsync(id);
                ViewBag.Message = "Đã duyệt!";
            }
                
            return Redirect("/Admin/Warehouse/ListReceiptRequest");
        }
      

        [HttpPost]
        public async Task<IActionResult> CreateReceiptRequest(ReceiptRequestViewModel model)
        {
            using(var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled)) { 
            var receiptRequest = new ReceiptRequest
            {
                CreateDate = model.CreateDate,
                Status = model.Status,
                //UserId = model.UserId
            };

                if (await _receiptService.AddReceiptRequestAsync(receiptRequest))
                {
                    ReceiptRequestDetail receiptRequestDetail;
                    var receiptRequestDetails = new List<ReceiptRequestDetail>();
                    for (var i = 0; i < model.Quantity.Count; i++)
                    {
                        receiptRequestDetail = new ReceiptRequestDetail
                        {
                            ReceiptRequestId = receiptRequest.ReceiptRequestId,
                            ProductDetailId = model.ProductDetailId[i],
                            Quantity = model.Quantity[i],
                            Status = "Chờ xử lý"

                        };
                        receiptRequestDetails.Add(receiptRequestDetail);
                    }

                    if (await _receiptService.AddReceiptDetailRequestsAsync(receiptRequestDetails))
                    {
                        transaction.Complete();
                        Redirect("/Admin/Warehouse/Index");

                    }
                    else
                    {
                        ViewBag.Message = "Thêm phiếu nhập lỗi";
                       
                    }
                }
            }
            return View(model);

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

        [HttpPost]
        public IActionResult CreateReceipt(ReceiptViewModel model)
        {
            if (ModelState.IsValid)
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var receipt = new ImportWarehouse()
                    {
                        CreateDate = DateTime.Now,

                        // get from previous view
                        //ReceiptRequestId = model.,
                        //UserId = UserManager.getCuurentUser();
                        Status = "Mới",
                        Total = model.Total
                    };
                    for (int i = 0; i < model.ProductId.Count; i++)
                    {
                        var receiptDetail = new ImportWarehouseDetail()
                        {
                            //ImportWarehouseId = 
                        };

                    }
                    /*if(_receiptService.AddReceipt(receipt){

                    };*/

                }
                catch
                {
                    ViewBag.MessageError = MESSAGE_ERROR_ADD_PRODUCT;
                }

            }
            return View(model);
        }
        [HttpGet]
        public JsonResult GetProvider(int productId)
        {
            return Json(_providerService.GetProvidersByProduct(productId));
        }
    }
}
