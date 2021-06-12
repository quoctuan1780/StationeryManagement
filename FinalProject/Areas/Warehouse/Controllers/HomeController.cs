using Entities.Models;
using FinalProject.Areas.Warehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly IAccountService _accountService;
        private readonly UserManager<User> _userManager;
        private readonly IProductService _productService;

        public HomeController(IProductService productService, IReceiptService receiptService,IAccountService accountService, UserManager<User> userManager)
        {
            _receiptService = receiptService;
            _accountService = accountService;
            _userManager = userManager;
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

        public async Task<IActionResult> ViewListRequestReceipt()
        {
            ViewBag.ListRequests = await _receiptService.GetReceiptRequestsAsync();
            return View();
        }

        [HttpDelete]
        public async Task<int> DeleteRequest(int requestId)
        {
            return await _receiptService.DeleteReceiptRequest(requestId);
        }

        public async Task<IActionResult> ViewRequestReceipt(int id)
        {
            ViewBag.Request = await _receiptService.GetReceiptRequestAsync(id);
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
                    UserId = _accountService.GetUserId(User)
                };


                if (await _receiptService.AddReceiptRequestAsync(receiptRequest))
                {
                    int count = 0;
                    for(int i = 0; i < model.ProductDetailId.Count; i++)
                    {
                        var requestDetail = new ReceiptRequestDetail()
                        {
                            ProductDetailId = model.ProductDetailId[i],
                            Quantity = model.Quantity[i],
                            ReceiptRequestId = receiptRequest.ReceiptRequestId,
                            Status = RECEIPT_REQUEST_STATUS_WAITING,

                        };
                      count += await  _receiptService.AddReceiptRequestDetailAsync(requestDetail);
                    }
                    if(count == model.ProductDetailId.Count)
                        transaction.Complete();
                    Redirect("/Warehouse/Home/ListReceipRequest");
                }
                else
                {
                    ViewBag.Message = "Thêm phiếu nhập lỗi";

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
