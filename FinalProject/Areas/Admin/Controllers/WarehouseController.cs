using static Common.Constant;
using static Common.MessageConstant;
using Entities.Models;
using FinalProject.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    //[Authorize(Roles = RoleConstant.ROLE_WAREHOUSE_MANAGER)]
    public class WarehouseController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProviderService _providerService;
        private readonly IReceiptService _receiptService;

        public WarehouseController(IProductService productService, IProviderService providerService, IReceiptService receiptService)
        {
            _productService = productService;
            _providerService = providerService;
            _receiptService = receiptService;
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
                        ViewBag.MessageError = MESSAGE_ERROR_ADD_PRODUCT;
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
