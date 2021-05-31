using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private readonly IWorkflowHistoryService _workflowHistoryService;

        public OrderController(IOrderService orderService, IAccountService accountService, IWorkflowHistoryService workflowHistoryService)
        {
            _orderService = orderService;
            _accountService = accountService;
            _workflowHistoryService = workflowHistoryService;
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.Orders = await _orderService.GetOrdersAsync();
            return View();
        }

        public async Task<IActionResult> OrderWaitExportWarehouse()
        {
            ViewBag.Orders = await _orderService.GetOrdersWaitExportWarehouseAsync();
            return View();
        }

        public async Task<IActionResult> OrderWaitDelivery()
        {
            ViewBag.Orders = await _orderService.GetOrdersWaitDeliveryAsync();
            return View();
        }

        public async Task<IActionResult> OrderDetail(int? orderId)
        {
            if(orderId is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            return View();
        }

        [HttpPut]
        [Authorize(Roles = ROLE_ADMIN)]
        public async Task<int> AdminComfirmOrder(int? orderId)
        {
            if(orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.AdminConfirmOrderAsync(orderId.Value);

                if(result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_WAITING_CONFIRM,
                        NextStatus = STATUS_PREPARING_GOODS,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        RecordId = orderId.Value.ToString(),
                    };

                    var resultAddworkflow = await _workflowHistoryService.AddWorkflowHistoryAsync(workFlow);

                    if(!(resultAddworkflow is null))
                    {
                        transaction.Complete();

                        return CODE_SUCCESS;
                    }
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }

        [HttpPut]
        [Authorize(Roles = ROLE_ADMIN_WAREHOUSE_MANAGER)]
        public async Task<int> WarehouseManagementComfirmOrder(int? orderId)
        {
            if (orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.WarehouseManagementConfirmOrderAsync(orderId.Value);

                if (result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_PREPARING_GOODS,
                        NextStatus = STATUS_WAITING_PICK_GOODS,
                        FullName = user.FullName,
                        UserEmail = user.Email,
                        RecordId = orderId.Value.ToString(),
                    };

                    var resultAddworkflow = await _workflowHistoryService.AddWorkflowHistoryAsync(workFlow);

                    if (!(resultAddworkflow is null))
                    {
                        transaction.Complete();

                        return CODE_SUCCESS;
                    }
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }
    }
}
