﻿using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Warehouse.Controllers
{
    [Area(AREA_WAREHOUSE)]
    [Authorize(Roles = ROLE_WAREHOUSE_MANAGER, AuthenticationSchemes = ROLE_WAREHOUSE_MANAGER)]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IAccountService _accountService;
        private IWorkflowHistoryService _workflowHistoryService;

        public OrderController(IOrderService orderService, IWorkflowHistoryService workflowHistoryService, IAccountService accountService)
        {
            _orderService = orderService;
            _accountService = accountService;
            _workflowHistoryService = workflowHistoryService;
        }

        public async Task<IActionResult> OrderDetail(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE_WAREHOUSE);
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            return View();
        }

        public async Task<IActionResult> OrderWaitExportWarehouse()
        {
            ViewBag.Orders = await _orderService.GetOrdersWaitExportWarehouseAsync();
            return View();
        }

        [HttpPut]
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
