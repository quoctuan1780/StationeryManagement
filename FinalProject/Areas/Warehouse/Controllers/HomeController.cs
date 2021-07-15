using AspNetCore.Reporting;
using Entities.Models;
using FinalProject.Areas.Warehouse.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Services.Hubs;
using Services.Interfacies;
using System;
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
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductDetailService _productDetailService;
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IWorkflowHistoryService _workflowHistoryService;
        private readonly INotificationService _notificationService;
        private readonly IAprioriBackground _aprioriBackground;

        public HomeController(IProductService productService, IReceiptService receiptService, IAccountService accountService, IOrderService orderService, IHubContext<SignalServer> hubContext,
            IWorkflowHistoryService workflowHistoryService, INotificationService notificationService, IWebHostEnvironment webHostEnvironment, IProductDetailService productDetailService, IAprioriBackground aprioriBackground)
        {
            _receiptService = receiptService;
            _accountService = accountService;
            _productService = productService;
            _orderService = orderService;
            _webHostEnvironment = webHostEnvironment;
            _productDetailService = productDetailService;
            _hubContext = hubContext;
            _workflowHistoryService = workflowHistoryService;
            _notificationService = notificationService;

            _aprioriBackground = aprioriBackground;
        }

        public IActionResult Dashboard()
        {
            return View();
        }

        public async Task Test()
        {
            await _aprioriBackground.RecommandationBackground();
        }

        public async Task<IActionResult> UpdateReceipt(int id)
        {
            ViewBag.Receipt = await _receiptService.GetReceiptAsync(id);

            ViewBag.User = await _accountService.GetUserAsync(User);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateReceipt(int id, List<int> AddQuantity, List<int> productDetailIds)
        {
            ViewBag.User = await _accountService.GetUserAsync(User);

            if (AddQuantity is null)
            {
                ViewBag.MessageError = "Lỗi hệ thống, vui lòng thử lại sau";
                return View();
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _receiptService.GetReceiptAfterUpdate(id, AddQuantity, productDetailIds);

                if (result != null)
                {
                    transaction.Complete();

                    ViewBag.Receipt = result;

                    ViewBag.MessageSuccess = "Cập nhật số lượng sản phẩm thành công";

                    return View();
                }
            }
            catch
            {
                ViewBag.MessageError = "Lỗi hệ thống, vui lòng thử lại sau";
            }

            ViewBag.Receipt = await _receiptService.GetReceiptAsync(id);

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
            var path = $"{_webHostEnvironment.WebRootPath}\\Report\\Report2.rdlc";
            var Receipt = await _receiptService.GetReceiptAsync(id);
            var listIdProduct = new List<int>();
            foreach (var item in Receipt.ImportWarehouseDetails)
            {
                listIdProduct.Add(item.ProductDetailId);
            }
            var products = _productService.GetProductForReportExportAsync(listIdProduct);
            var details = _productDetailService.GetColorReportAsync(listIdProduct);
            var localReport = new LocalReport(path);
            localReport.AddDataSource("DataSet1", details);
            localReport.AddDataSource("DataSet2", Receipt);
            localReport.AddDataSource("DataSet3", products);
            var result = localReport.Execute(RenderType.Pdf, extension, null, mintype);
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
            if (requestId is null)
            {
                return ERROR_CODE_NULL;
            }

            var result = await _receiptService.DeleteReceiptRequestAsync(requestId.Value);

            if (result > 0)
            {
                await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_RECEPT);

                return CODE_SUCCESS;
            }


            return ERROR_CODE_SYSTEM;
        }

        public async Task<IActionResult> ViewRequestReceipt(int? id)
        {
            if (id is null)
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

            ViewBag.User = await _accountService.GetUserAsync(User);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateReceiptRequest(ReceiptRequestViewModel model)
        {
            ViewBag.Products = await _productService.GetProductWithDetailsAsync();

            var user = await _accountService.GetUserAsync(User);

            ViewBag.User = user;

            ModelState.Remove("Quantity");
            ModelState.Remove("Prices");

            if (ModelState.IsValid)
            {
                if (model.ProductDetailId.Count != model.Quantity.Count || model.ProductDetailId.Count != model.Prices.Count)
                {
                    return View(model);
                }

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

                try
                {
                    var receiptRequest = new ReceiptRequest
                    {
                        CreateDate = model.CreateDate,
                        Status = RECEIPT_REQUEST_STATUS_WAITING,
                        UserId = model.UserId
                    };

                    var workflow = new WorkflowHistory()
                    {
                        CreatedBy = model.UserId,
                        CreatedDate = DateTime.Now,
                        FullName = user.FullName,
                        CurrentStatus = RECEIPT_REQUEST_STATUS_WAITING,
                        Type = TYPE_IMPORT_WAREHOUSE,
                        UserEmail = user.Email,
                        UserRole = ROLE_ADMIN,
                        NextStatus = RECEIPT_REQUEST_STATUS_APPROVED
                    };

                    var resultAddRecept = await _receiptService.AddReceiptRequestAsync(receiptRequest);

                    if (resultAddRecept)
                    {
                        workflow.RecordId = receiptRequest.ReceiptRequestId.ToString();

                        await _workflowHistoryService.AddWorkflowHistoryAsync(workflow);

                        var receiptRequestDetails = new List<ReceiptRequestDetail>();

                        for (int i = 0; i < model.ProductDetailId.Count; i++)
                        {
                            var requestDetail = new ReceiptRequestDetail()
                            {
                                ProductDetailId = model.ProductDetailId[i],
                                Quantity = model.Quantity[i],
                                ReceiptRequestId = receiptRequest.ReceiptRequestId,
                                Status = RECEIPT_REQUEST_STATUS_WAITING,
                                Price = model.Prices[i]
                            };
                            receiptRequestDetails.Add(requestDetail);
                        }
                        var result = await _receiptService.AddReceiptRequestDetailAsync(receiptRequestDetails);

                        if (result > 0)
                        {
                            var link = Url.ActionLink("ViewRequestReceipt", "Warehouse",
                                            new { Area = AREA_ADMIN, Id = receiptRequest.ReceiptRequestId },
                                            Request.Scheme);

                            var notificationType = await _notificationService.GetNotifycationByNameAsync(NOTIFICATION_REQUEST_RECEIPT_IMPORT_WAREHOUSE);

                            var notifications = new List<Notification>();

                            var admins = await _accountService.GetAllAdminsAsync();

                            var content = "Quản kho " + user.FullName + " đã tạo một yêu cầu nhập hàng mã đơn là #" + receiptRequest.ReceiptRequestId;
                            if (admins != null)
                            {
                                foreach (var item in admins)
                                {
                                    var nofification = new Notification()
                                    {
                                        CreatedDate = DateTime.Now,
                                        Link = link,
                                        NotificationTypeId = notificationType.NotificationTypeId,
                                        Status = STATUS_NOT_SEEN_NOTIFICATION,
                                        UserId = item.Id,
                                        RecordId = receiptRequest.ReceiptRequestId,
                                        RoleSeen = ROLE_ADMIN,
                                        Content = content
                                    };

                                    notifications.Add(nofification);
                                }
                            }

                            await _notificationService.AddNotificationAsync(notifications);

                            await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_RECEPT);

                            await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_NOTIFICATION_NEW_RECEIPT_REQUEST, link, DateTime.Now.ToShortDateString(), content);

                            transaction.Complete();

                            return Redirect("/Warehouse/Home/ViewListRequestReceipt");
                        }
                    }
                }
                catch
                {
                    ViewBag.Message = "Thêm phiếu nhập không thành công";
                }
            }

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
