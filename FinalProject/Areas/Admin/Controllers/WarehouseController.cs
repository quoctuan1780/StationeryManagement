using Common;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(Constant.AREA_ADMIN)]
    //[Authorize(Roles = RoleConstant.ROLE_WAREHOUSE_MANAGER)]
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

        public string GetRecomandation()
        {

           var result = _recommendationService.GetRecommandtion(3,0.75);
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

        public async Task<string> GetBestSeller(int quantity)
        {
            var result = await _productService.BestSellerInMonthAsync(quantity);

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

        public IActionResult CreateReceipt()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> CreateReceiptRequest()
        {
            var products = await _productService.GetProductWithDetailsAsync();
            var listProduct = new List<SelectListItem>();

            foreach(var product in products)
            {
                listProduct.Add(new SelectListItem
                {
                    Value = product.ProductId.ToString(),
                    Text = product.Product.ProductName + " " + product.Color + " Xuất xứ" + product.Origin
                }) ;
            }

            ViewBag.Products = listProduct;

            return View();
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
                    List<ReceiptRequestDetail> receiptRequestDetails = new List<ReceiptRequestDetail>();
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

        [HttpPost]
        public IActionResult CreateReceipt(ReceiptViewModel model)
        {  
            if (ModelState.IsValid)
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
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
                        for (int i = 0; i < model.ProductId.Count;i++)
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
                        ViewBag.MessageError = MessageConstant.MESSAGE_ERROR_ADD_PRODUCT;
                    }
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
