using static Common.Constant;
using static Common.SignalRConstant;
using static Common.RoleConstant;
using System;
using System.Linq;
using Services.Hubs;
using Newtonsoft.Json;
using System.Transactions;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Entities.Models;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class WarehouseController : Controller
    {
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IProductService _productService;
        private readonly IReceiptService _receiptService;
        private readonly IRecommendationService _recommendationService;
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowHistoryService _workflowHistoryService;
        private static DateTime FromDate;
        private static DateTime ToDate;
        private static int Quantity;

        public WarehouseController(IProductService productService, IReceiptService receiptService, 
            IRecommendationService recommendationService, IHubContext<SignalServer> hubContext, IAccountService accountService, INotificationService notificationService, IWorkflowHistoryService workflowHistoryService)
        {
            _hubContext = hubContext;
            _productService = productService;
            _receiptService = receiptService;
            _recommendationService = recommendationService;
            _accountService = accountService;
            _notificationService = notificationService;
            _workflowHistoryService = workflowHistoryService;
        }
        public async Task<IActionResult> ViewRecommendation()
        {
            ViewBag.ProductOutOfStock = await _productService.GetProductDetailsRunOutOfStockAsync();
            return View();
        }

        public async Task<string> GetRecommandation()
        {
            var listId = await _productService.ListBestSellerProduct(FromDate, ToDate, Quantity);
            var result = await _recommendationService.GetRecommandtion(listId);
            var recommandation = new List<JObject>();

            if(result is null || !result.Any())
            {
                return NULL;
            }

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
            var result = await _productService.BestSellerInMonthAsync(fromDate, toDate, quantity);
            if(result is null || result.Equals("[]"))
            {
                return NULL;
            }

            return result;
        }

               

        public async Task<IActionResult> RejectReceipt(int? id)
        {
            if (id is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _receiptService.RejectReceiptRequestAsync(id.Value);

                if (result > 0)
                {
                    var receiptRequest = await _receiptService.GetReceiptRequestAsync(id.Value);

                    var workflow = new WorkflowHistory()
                    {
                        CreatedBy = receiptRequest.UserId,
                        CreatedDate = DateTime.Now,
                        FullName = receiptRequest.User.FullName,
                        CurrentStatus = RECEIPT_REQUEST_STATUS_WAITING,
                        Type = TYPE_IMPORT_WAREHOUSE,
                        UserEmail = receiptRequest.User.Email,
                        UserRole = ROLE_ADMIN,
                        NextStatus = RECEIPT_REQUEST_REJECT,
                        RecordId = id.Value.ToString()
                    };

                    await _workflowHistoryService.AddWorkflowHistoryAsync(workflow);

                    var link = Url.ActionLink("ViewRequestReceipt", "Home",
                                            new { Area = AREA_WAREHOUSE, Id = id.Value },
                                            Request.Scheme);

                    var notificationType = await _notificationService.GetNotifycationByNameAsync(NOTIFICATION_REJECT_RECEIPT_IMPORT_WAREHOUSE);

                    var content = "Quản trị viên từ chối đơn nhập hàng có mã đơn là #" + id.Value;

                    var notification = new Notification()
                    {
                        CreatedDate = DateTime.Now,
                        Link = link,
                        NotificationTypeId = notificationType.NotificationTypeId,
                        Status = STATUS_NOT_SEEN_NOTIFICATION,
                        UserId = receiptRequest.UserId,
                        RecordId = id.Value,
                        RoleSeen = ROLE_WAREHOUSE_MANAGER,
                        Content = content,
                    };

                    await _notificationService.AddNotificationAsync(notification);

                    await _hubContext.Clients.User(receiptRequest.UserId).SendAsync(SIGNAL_NOTIFICATION_REJECT_RECEIPT_REQUEST, link, DateTime.Now.ToShortDateString(), content);

                    transaction.Complete();

                    TempData["MessageSuccess"] = "Từ chối đơn nhập hàng thành công";
                }
            }
            catch
            {
                TempData["MessageError"] = "Lỗi hệ thống! từ chối đơn nhập hàng không thàng công";
            }
            
            return Redirect("/Admin/Warehouse/ListReceiptRequest");
        }

        
        public async Task<IActionResult> ApproveReceipt(int? id)
        {
            if (id is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _receiptService.ApproveReceiptRequestAsync(id.Value);

                if (result > 0)
                {
                    var receiptRequest = await _receiptService.GetReceiptRequestAsync(id.Value);

                    var workflow = new WorkflowHistory()
                    {
                        CreatedBy = receiptRequest.UserId,
                        CreatedDate = DateTime.Now,
                        FullName = receiptRequest.User.FullName,
                        CurrentStatus = RECEIPT_REQUEST_STATUS_WAITING,
                        Type = TYPE_IMPORT_WAREHOUSE,
                        UserEmail = receiptRequest.User.Email,
                        UserRole = ROLE_ADMIN,
                        NextStatus = RECEIPT_REQUEST_STATUS_APPROVED,
                        RecordId = id.Value.ToString()
                    };

                    await _workflowHistoryService.AddWorkflowHistoryAsync(workflow);

                    result = await _receiptService.AddReceiptAsync(id.Value);

                    if(result > 0)
                    {
                        var link = Url.ActionLink("ViewRequestReceipt", "Home",
                                            new { Area = AREA_WAREHOUSE, Id = id.Value },
                                            Request.Scheme);

                        var notificationType = await _notificationService.GetNotifycationByNameAsync(NOTIFICATION_REQUEST_RECEIPT_IMPORT_WAREHOUSE);

                        var content = "Quản trị viên đã duyệt đơn nhập hàng có mã đơn là #" + id.Value;

                        var notification = new Notification()
                        {
                            CreatedDate = DateTime.Now,
                            Link = link,
                            NotificationTypeId = notificationType.NotificationTypeId,
                            Status = STATUS_NOT_SEEN_NOTIFICATION,
                            UserId = receiptRequest.UserId,
                            RecordId = id.Value,
                            RoleSeen = ROLE_WAREHOUSE_MANAGER,
                            Content = content
                        };
                           

                        await _notificationService.AddNotificationAsync(notification);


                        await _hubContext.Clients.Group(SIGNAL_GROUP_WAREHOUSE).SendAsync(SIGNAL_COUNT_RECEPT_REQUEST_ACCEPT);

                        await _hubContext.Clients.User(receiptRequest.UserId).SendAsync(SIGNAL_NOTIFICATION_APPROVED_RECEIPT_REQUEST, link, DateTime.Now.ToShortDateString(), content);

                        transaction.Complete();

                        TempData["MessageSuccess"] = "Duyệt đơn yêu cầu nhập hàng thành công";
                    }
                }
                
            }
            catch
            {
                TempData["MessageError"] = "Lỗi hệ thống! duyệt đơn nhập hàng không thàng công";
            }

            return Redirect("/Admin/Warehouse/ListReceiptRequest");
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
        public async Task<IActionResult> ViewRequestReceipt(int? id)
        {
            if(id is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            ViewBag.Request = await _receiptService.GetReceiptRequestAsync(id.Value);

            return View();
        }
    }
}
