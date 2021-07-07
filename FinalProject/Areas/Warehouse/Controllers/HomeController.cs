using Entities.Models;
using FinalProject.Areas.Warehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
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
        private readonly IOrderService _orderService;

        public HomeController(IProductService productService, IReceiptService receiptService,IAccountService accountService, 
            UserManager<User> userManager, IOrderService orderService)
        {
            _receiptService = receiptService;
            _accountService = accountService;
            _userManager = userManager;
            _productService = productService;
            _orderService = orderService;
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
                    Value = product.ProductDetailId.ToString(),
                    Text = product.Product.ProductName + product.Color
                }) ;
            }

            ViewBag.Products = listProduct;

            return View();
        }

        public async Task<IActionResult> UpdateReceipt(int id)
        {
            ViewBag.Receipt = await _receiptService.GetReceiptAsync(id);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateReceipt(int id, List<int> AddQuantity)
        {
            ViewBag.Receipt = await _receiptService.GetReceiptAfterUpdate(id, AddQuantity);
            return View();
        }
        public async Task<IActionResult> ListReceipt()
        {
            ViewBag.Receipts = await _receiptService.GetReceiptsAsync();
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
            return await _receiptService.DeleteReceiptRequestAsync(requestId);
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
                    List<ReceiptRequestDetail> list = new();
                    for(int i = 0; i < model.ProductDetailId.Count; i++)
                    {
                        var requestDetail = new ReceiptRequestDetail()
                        {
                            ProductDetailId = model.ProductDetailId[i],
                            Quantity = model.Quantity[i],
                            ReceiptRequestId = receiptRequest.ReceiptRequestId,
                            Status = RECEIPT_REQUEST_STATUS_WAITING,
                            
                        };
                        list.Add(requestDetail);
                      count += await  _receiptService.AddReceiptRequestDetailAsync(list);
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
        public async Task<IActionResult> GetAcceptedRequest()
        {
            return Ok(await _receiptService.CountAcceptedRequestReceiptAsync());
        }
        public async Task<IActionResult> GetAcceptedOrsers()
        {
            return Ok(await _orderService.CountNewAcceptedOrdersAsync());
        }      
        public async Task<IActionResult> GetChartSales()
        {
            return Ok(await _orderService.GetTotalSalesPerMonthsAsync());
        }

        public async Task<IActionResult> GetChartPurchase()
        {
            return Ok(await _orderService.GetTotalPurchasePerMonthsAsync());
        }
        public async Task<IActionResult> GetProcessReceipt()
        {
            var result = await _receiptService.GetListProcessReceiptAsync();
            return Ok(JsonConvert.SerializeObject(result));
        }
        public async Task<IActionResult> CountWaitingForPicking()
        {
            return Ok(await _orderService.CountOrdersWaitToPickAsync());
        }
        public async Task<IActionResult> CountWaitingForDelivering()
        {
            return Ok(await _orderService.CountOrderWaitToDeliveryAsync());
        }
        public async Task<IActionResult> CountDeliveringOrder()
        {
            return Ok(await _orderService.CountOrdersDeliveringAsync());
        }
        public async Task<IActionResult> CountDelivered()
        {
            return Ok(await _orderService.CountOrdersDeliveredAsync());
        }
        public async Task<IActionResult> ProcessReceipt()
        {
            return Ok(await _receiptService.GetNumberOfProcessingReceiptAsync());
        }
        public async Task<IActionResult> DeliveryChart()
        {
            return Ok(await _orderService.ListPercentDeliveryAsync());
        }
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
