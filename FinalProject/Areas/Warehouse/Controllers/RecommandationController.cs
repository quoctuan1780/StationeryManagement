﻿using static Common.Constant;
using static Common.RoleConstant;
using static Common.SignalRConstant;
using static FinalProject.Areas.Warehouse.Helpers.ReceiptRequestHelper;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Hubs;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using FinalProject.Areas.Warehouse.ViewModels;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
    public class RecommandationController : Controller
    {
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly IProductService _productService;
        private readonly IReceiptService _receiptService;
        private readonly IRecommendationService _recommendationService;
        private readonly UserManager<User> _userManager;
        private readonly IAccountService _accountService;
        private readonly INotificationService _notificationService;
        private readonly IWorkflowHistoryService _workflowHistoryService;
        private static DateTime FromDate;
        private static DateTime ToDate;
        private static int Quantity;

        public RecommandationController(IProductService productService, IReceiptService receiptService,
            IRecommendationService recommendationService, IHubContext<SignalServer> hubContext, UserManager<User> userManager, IAccountService accountService, INotificationService notificationService, IWorkflowHistoryService workflowHistoryService)
        {
            _hubContext = hubContext;
            _productService = productService;
            _receiptService = receiptService;
            _recommendationService = recommendationService;
            _userManager = userManager;
            _accountService = accountService;
            _notificationService = notificationService;
            _workflowHistoryService = workflowHistoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ViewRecommandation()
        {
            FromDate = DateTime.MinValue;
            ToDate = DateTime.MinValue;
            Quantity = 0;

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
            recommandation = recommandation.Distinct().ToList();
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
        public async Task<IActionResult> AutoCreateReceiptRequest()
        {
            ViewBag.ListProduct = await _recommendationService.GetListProductDetailForCreateRRAsync(FromDate, ToDate, Quantity);
            
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> AutoCreateReceiptRequest(IList<int> selected, IList<int> quantity)
        public async Task<IActionResult> AutoCreateReceiptRequest(ReceiptRequestViewModel model, string productDetalIdMultiPage, string priceMultiPage, string quantityMultiPage)
        {

            var productDetalIds = ConvertStringToListInt(productDetalIdMultiPage);
            var prices = ConvertStringToListDecimal(priceMultiPage);
            var quantities = ConvertStringToListInt(quantityMultiPage);

            model.ProductDetailId = productDetalIds;
            model.Prices = prices;
            model.Quantity = quantities;

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var receiptRequest = new ReceiptRequest()
                {
                    CreateDate = DateTime.Now,
                    UserId = user.Id,
                    Status = RECEIPT_REQUEST_STATUS_WAITING
                };

                var workflow = new WorkflowHistory()
                {
                    CreatedBy = user.Id,
                    CreatedDate = DateTime.Now,
                    FullName = user.FullName,
                    CurrentStatus = RECEIPT_REQUEST_STATUS_WAITING,
                    Type = TYPE_IMPORT_WAREHOUSE,
                    UserEmail = user.Email,
                    UserRole = ROLE_ADMIN,
                    NextStatus = RECEIPT_REQUEST_STATUS_APPROVED
                };

                if (await _receiptService.AddReceiptRequestAsync(receiptRequest))
                {
                    workflow.RecordId = receiptRequest.ReceiptRequestId.ToString();

                    await _workflowHistoryService.AddWorkflowHistoryAsync(workflow);

                    var listReceiptsDetail = new List<ReceiptRequestDetail>();

                    for (int i = 0; i < productDetalIds.Count; i++)
                    {
                        var detail = new ReceiptRequestDetail()
                        {
                            ReceiptRequestId = receiptRequest.ReceiptRequestId,
                            ProductDetailId = productDetalIds[i],
                            Quantity = quantities[i],
                            Price = prices[i],
                            Status = RECEIPT_STATUS_WAITING
                        };
                        listReceiptsDetail.Add(detail);
                    }

                    if (await _receiptService.AddReceiptRequestDetailAsync(listReceiptsDetail) > 0)
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

                        transaction.Complete();

                        await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_COUNT_NEW_RECEPT);

                        await _hubContext.Clients.Group(SIGNAL_GROUP_ADMIN).SendAsync(SIGNAL_NOTIFICATION_NEW_RECEIPT_REQUEST, link, DateTime.Now.ToShortDateString(), content);

                        return Redirect("/Warehouse/Home/ViewListRequestReceipt");
                    }

                }
            }
            catch
            {
                ViewBag.Message = "Không thể thêm phiếu yêu cầu nhập hàng!";
            }



            return View(model);
        }
    }
}
