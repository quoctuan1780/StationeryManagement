using AspNetCore.Reporting;
using Entities.Models;
using FinalProject.Areas.Warehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Services.Hubs;
using Services.Interfacies;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;
using static Common.SignalRConstant;

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
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductDetailService _productDetailService;
        private readonly IHubContext<SignalServer> _hubContext;

        public HomeController(IProductService productService, IReceiptService receiptService,IAccountService accountService, 
            UserManager<User> userManager, IOrderService orderService, IWebHostEnvironment webHostEnvironment,IProductDetailService productDetailService, IHubContext<SignalServer> hubContext)
        {
            _receiptService = receiptService;
            _accountService = accountService;
            _userManager = userManager;
            _productService = productService;
            _orderService = orderService;
            _webHostEnvironment = webHostEnvironment;
            _productDetailService = productDetailService;
            _hubContext = hubContext;
        }

        public IActionResult Dashboard()
        {
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
        public async Task<IActionResult> PrintReceipt(int id)
        {
            string mintype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.WebRootPath}\\Report\\Report2.rdlc";
            var Receipt = await _receiptService.GetReceiptAsync(id);
            List<int> listIdProduct = new List<int>();
            foreach(var item in Receipt.ImportWarehouseDetails)
            {
                listIdProduct.Add(item.ProductDetailId);
            }
            var products = _productService.GetProductForReportExportAsync(listIdProduct);
            var details = _productDetailService.GetColorReportAsync(listIdProduct);
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1",details);
            localReport.AddDataSource("DataSet2", Receipt);
            localReport.AddDataSource("DataSet3", products);
            var result = localReport.Execute(RenderType.Pdf, extension, null,mintype);
            return File(result.MainStream, "application/pdf");
        }

        public async Task<IActionResult> ViewListRequestReceipt()
        {
            ViewBag.ListRequests = await _receiptService.GetReceiptRequestsAsync();
            return View();
        }

        [HttpDelete]
        public async Task<int> DeleteRequest(int? requestId)
        {
            if(requestId is null)
            {
                return ERROR_CODE_NULL;
            }

            var result = await _receiptService.DeleteReceiptRequestAsync(requestId.Value);

            if(result > 0)
            {
                await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_RECEPT);

                return CODE_SUCCESS;
            }


            return ERROR_CODE_SYSTEM;
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

        [HttpGet]
        public async Task<IActionResult> CreateReceiptRequest()
        {
            ViewBag.Products = await _productService.GetProductWithDetailsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReceiptRequest(ReceiptRequestViewModel model)
        {
            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var receiptRequest = new ReceiptRequest
                {
                    CreateDate = model.CreateDate,
                    Status = RECEIPT_REQUEST_STATUS_WAITING,
                    UserId = model.UserId,
                };

                var resultAddRecept = await _receiptService.AddReceiptRequestAsync(receiptRequest);

                if (resultAddRecept)
                {
                    var list = new List<ReceiptRequestDetail>();
                    for (int i = 0; i < model.ProductDetailId.Count; i++)
                    {
                        var requestDetail = new ReceiptRequestDetail()
                        {
                            ProductDetailId = model.ProductDetailId[i],
                            Quantity = model.Quantity[i],
                            ReceiptRequestId = receiptRequest.ReceiptRequestId,
                            Status = RECEIPT_REQUEST_STATUS_WAITING,

                        };
                        list.Add(requestDetail);
                    }
                    var result = await _receiptService.AddReceiptRequestDetailAsync(list);

                    if (result > 0) 
                    {
                        transaction.Complete();

                        await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_RECEPT);

                        return Redirect("/Warehouse/Home/ViewListRequestReceipt");
                    }
                }
            }
            catch
            {
                ViewBag.Message = "Thêm phiếu nhập không thành công";
            }

            ViewBag.Products = await _productService.GetProductWithDetailsAsync();

            return View(model);
        }

        #region Hub service
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
        #endregion
    }
}
