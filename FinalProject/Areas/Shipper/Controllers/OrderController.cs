using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using static Common.Constant;
using static Common.RoleConstant;

namespace FinalProject.Areas.Shipper.Controllers
{
    [Area(AREA_SHIPPER)]
    [Authorize(Roles = ROLE_SHIPPER, AuthenticationSchemes = ROLE_SHIPPER)]
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
        public IActionResult OrderWaitPick()
        {
            ViewBag.Orders = _orderService.GetOrdersWaitToPick();

            return View();
        }

        [HttpPut]
        public async Task<int> ConfirmOrder(IList<int> ordersPicked)
        {
            if(ordersPicked is null)
            {
                return ERROR_CODE_NULL;
            }
            try
            {
                var user = await _accountService.GetUserAsync(User);
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var result = await _orderService.ShipperConfirmPickOrdersAsync(ordersPicked, user.Id);
                if (result > 0)
                {
                    transaction.Complete();

                    return CODE_SUCCESS;
                }
            }
            catch
            {
            }

            return ERROR_CODE_SYSTEM;
        }

        public async Task<IActionResult> OrderWaitDelivery()
        {
            var user = await _accountService.GetUserAsync(User);

            ViewBag.Orders = await _orderService.GetOrdersWaitDeliveryAsync(user.Id);

            return View();
        }

        public async Task<IActionResult> OrderDelivery()
        {
            var user = await _accountService.GetUserAsync(User);

            ViewBag.Orders = await _orderService.GetOrdersDeliveryAsync(user.Id);

            return View();
        }

        public async Task<IActionResult> OrderDetail(int? orderId)
        {
            if (orderId is null)
            {
                return PartialView(ERROR_404_PAGE_SHIPPER);
            }

            ViewBag.Order = await _orderService.GetOrderByIdAsync(orderId.Value);

            return View();
        }

        [HttpPut]
        public async Task<int> ShipperComfirmOrder(int? orderId)
        {
            if (orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.ShipperConfirmOrderAsync(orderId.Value);

                if (result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_WAITING_PICK_GOODS,
                        NextStatus = STATUS_ON_DELIVERY_GOODS,
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

        [HttpPut]
        public async Task<int> ShipperComfirmDelivery(int? orderId)
        {
            if (orderId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var user = await _accountService.GetUserAsync(User);

                var result = await _orderService.ShipperConfirmDeliveryAsync(orderId.Value);

                if (result > 0)
                {
                    var workFlow = new WorkflowHistory()
                    {
                        CreatedDate = DateTime.Now,
                        CreatedBy = user.Id,
                        CurrentStatus = STATUS_ON_DELIVERY_GOODS,
                        NextStatus = STATUS_RECEIVED_GOODS,
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
