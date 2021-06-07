using Entities.Models;
using FinalProject.Areas.Warehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Interfacies;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
    public class HomeController : Controller
    {
        private readonly IReceiptService _receiptService;
        private readonly IProductService _productService;

        public HomeController(IProductService productService, IReceiptService receiptService)
        {
            _receiptService = receiptService;
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> CreateReceiptRequest()
        {
            var products = await _productService.GetProductWithDetailsAsync();
            var listProduct = new List<SelectListItem>();

            foreach (var product in products)
            {
                listProduct.Add(new SelectListItem
                {
                    Value = product.ProductId.ToString(),
                    Text = product.Product.ProductName + " " + product.Color + " Xuất xứ " + product.Origin
                });
            }

            ViewBag.Products = listProduct;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateReceiptRequest(ReceiptRequestViewModel model)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var receiptRequest = new ReceiptRequest
                {
                    CreateDate = model.CreateDate,
                    Status = model.Status,
                    UserId = model.UserId
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
                            Status = "Chờ duyệt"

                        };
                        receiptRequestDetails.Add(receiptRequestDetail);
                    }

                    if (await _receiptService.AddReceiptDetailRequestsAsync(receiptRequestDetails))
                    {
                        if (await _receiptService.AddReceiptAsync(receiptRequest.ReceiptRequestId) > 0)
                        {
                            transaction.Complete();
                            Redirect("/Admin/Warehouse/Index");
                        }


                    }
                    else
                    {
                        ViewBag.Message = "Thêm phiếu nhập lỗi";

                    }
                }
            }
            return View(model);

        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
