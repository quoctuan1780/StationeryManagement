using static Common.Constant;
using static Common.RoleConstant;
using static Common.SignalRConstant;
using static FinalProject.Areas.Admin.Helpers.ImageHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using FinalProject.Areas.Admin.ViewModels;
using System.Transactions;
using Entities.Models;
using System.Linq;
using System;
using Microsoft.AspNetCore.SignalR;
using Services.Hubs;
using System.Collections.Generic;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class SaleController : Controller
    {
        private readonly IProductService _productService;
        private readonly ISaleDetailService _saleDetailService;
        private readonly ISaleService _saleService;
        private readonly IHubContext<SignalServer> _hubContext;
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;

        public SaleController(IProductService productService, ISaleService saleService, ISaleDetailService saleDetailService, INotificationService notificationService, IHubContext<SignalServer> hubContext, IAccountService accountService)
        {
            _productService = productService;
            _saleDetailService = saleDetailService;
            _saleService = saleService;
            _hubContext = hubContext;
            _notificationService = notificationService;
            _accountService = accountService;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.Sales = await _saleService.GetSalesAsync();

            return View();
        }

        [HttpDelete]
        public async Task<int> DeleteSale(int? saleId)
        {
            if(saleId is null)
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var result = await _saleService.DeleteSaleByIdAsync(saleId.Value);

                if(result > 0)
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

        public async Task<IActionResult> EditSale(int? saleId)
        {
            if(saleId is null)
            {
                return PartialView(ERROR_404_PAGE_ADMIN);
            }

            ViewBag.Products = await _productService.GetProductsCanApplySaleAsync();

            var sale = await _saleService.GetSaleByIdAsync(saleId.Value);

            var model = new SaleViewModel()
            {
                SaleId = sale.SaleId,
                SaleName = sale.SaleName,
                SaleStartDate = sale.SaleStartDate,
                SaleEndDate = sale.SaleEndDate,
                SaleType = sale.SaleType,
                Description = sale.Description,
                Discount = sale.Discount,
                FromOrderPrice = sale.FromOrderPrice,
                Image = sale.Image
            };

            if(sale.SaleDetails != null && sale.SaleDetails.Any())
            {
                model.ProductIds = sale.SaleDetails.Select(x => x.ProductId).ToList();
            }

            ViewBag.ProductInSales = await _productService.GetAllProductsAsync(model.ProductIds);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSale(SaleViewModel model, string imagePreviewDeteted)
        {
            ViewBag.Products = await _productService.GetProductsCanApplySaleAsync();

            if (model.SaleType.Equals(TYPE_SALE_FOR_ORDER))
            {
                ModelState.Remove("ProductIds");
            }
            else
            {
                ModelState.Remove("FromOrderPrice");
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(imagePreviewDeteted))
                {
                    var imagesName = imagePreviewDeteted.Split(COMMA);
                    if (model.ImageFile != null && imagesName.Contains(model.ImageFile.FileName))
                    {
                        model.ImageFile = null;
                    }
                }

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var sale = new Sale()
                    {
                        SaleId = model.SaleId,
                        SaleName = model.SaleName,
                        Description = model.Description,
                        SaleStartDate = model.SaleStartDate,
                        SaleEndDate = model.SaleEndDate,
                        Discount = model.Discount,
                        SaleType = model.SaleType,
                        Image = model.Image
                    };

                    string image = EMPTY;

                    if (model.ImageFile != null)
                    {
                        image = await SaveImageAsync(model.ImageFile, 384, 384, IMAGE_SALE_LINK);

                        if (image != EMPTY)
                        {
                            sale.Image = image;
                        }
                    }

                    var result = await _saleService.UpdateSaleAsync(sale);

                    if (model.SaleType.Equals(TYPE_SALE_FOR_PRODUCT))
                    {
                        var resultUpdate = await _saleDetailService.UpdateSaleDetailsAsync(model.ProductIds, model.Discount, model.SaleId, model.SaleStartDate, model.SaleEndDate);

                        if(resultUpdate > 0)
                        {
                            await _productService.UpdateSalePriceAsync();

                            transaction.Complete();

                            TempData["MessageSuccess"] = "Cập nhật khuyến mại thành công";

                            return Redirect("/Admin/Sale/Index");
                        }
                    }
                    else
                    {
                        transaction.Complete();

                        TempData["MessageSuccess"] = "Cập nhật khuyến mại thành công";

                        return View("/Admin/Sale/Index");
                    }
                }
                catch
                {
                    ViewBag.MessageError = "Lỗi hệ thống, vui lòng thử lại sau";
                }
            }

            model.ProductIds = await _saleDetailService.GetProductIdInSaleAsync(model.SaleId);

            ViewBag.ProductInSales = await _productService.GetAllProductsAsync(model.ProductIds);

            return View(model);
        }

        public async Task<IActionResult> AddSale()
        {
            ViewBag.Products = await _productService.GetProductsCanApplySaleAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSale(SaleViewModel model, string imagePreviewDeteted)
        {
            ViewBag.Products = await _productService.GetProductsCanApplySaleAsync();

            if (model.SaleType.Equals(TYPE_SALE_FOR_ORDER))
            {
                ModelState.Remove("ProductIds");
            }
            else
            {
                ModelState.Remove("FromOrderPrice");
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(imagePreviewDeteted))
                {
                    var imagesName = imagePreviewDeteted.Split(COMMA);
                    if (model.ImageFile != null && imagesName.Contains(model.ImageFile.FileName))
                    {
                        model.ImageFile = null;
                    }
                }

                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                try
                {
                    var sale = new Sale()
                    {
                        SaleName = model.SaleName,
                        Description = model.Description,
                        SaleStartDate = model.SaleStartDate,
                        SaleEndDate = model.SaleEndDate,
                        Discount = model.Discount,
                        SaleType = model.SaleType
                    };

                    var listNotifications = new List<Notification>();
                    var users = await _accountService.GetAllCustomersAsync();

                    if (model.SaleType.Equals(TYPE_SALE_FOR_ORDER))
                    {
                        sale.FromOrderPrice = model.FromOrderPrice;

                        var result = await _saleService.AddSaleAsync(sale);

                        if (result != null)
                        {
                            transaction.Complete();

                            TempData["MessageSuccess"] = "Thêm khuyến mại thành công";

                            return Redirect("/Admin/Sale/Index");
                        }
                    }
                    else
                    {
                        var checkExistsProductSale = await _productService.CheckProductIsInAnySalesAsync(model.ProductIds);
                        if (!checkExistsProductSale)
                        {
                            string image = EMPTY;

                            if(model.ImageFile != null)
                            {
                                image = await SaveImageAsync(model.ImageFile, 384, 384, IMAGE_SALE_LINK);

                                if(image != EMPTY)
                                {
                                    sale.Image = image;
                                }
                            }
                            
                            var result = await _saleService.AddSaleAsync(sale);

                            if (result != null)
                            {
                                var resultAddSaleDetail = await _saleDetailService.AddSaleDetailsAsync(model.ProductIds, result.SaleId, model.SaleStartDate, model.SaleEndDate);

                                if (resultAddSaleDetail > 0)
                                {
                                    var resultUpdatePriceSaleProduct = await _productService.UpdatePriceSaleProductsAsync(model.ProductIds, model.Discount);

                                    if (resultUpdatePriceSaleProduct > 0)
                                    {
                                        await _productService.UpdateSalePriceAsync();

                                        var link = Url.ActionLink("SaleProduct", "Home", new { Area = "" }, Request.Scheme);
                                        var notificationType = await _notificationService.GetNotifycationByNameAsync(NOTIFICATION_SALE);

                                        if (users != null)
                                        {
                                            foreach (var item in users)
                                            {
                                                var notification = new Notification()
                                                {
                                                    CreatedDate = DateTime.Now,
                                                    Link = link,
                                                    NotificationTypeId = notificationType.NotificationTypeId,
                                                    Status = STATUS_NOT_SEEN_NOTIFICATION,
                                                    UserId = item.Id,
                                                    RecordId = sale.SaleId,
                                                    RoleSeen = ROLE_CUSTOMER,
                                                    Content = "Quản trị viên đã áp dụng khuyến mãi cho một vài sản phẩm"
                                                };

                                                listNotifications.Add(notification);
                                            }
                                        }

                                        await _notificationService.AddNotificationAsync(notifications: listNotifications);

                                        await _hubContext.Clients.Group(SIGNAL_GROUP_CUSTOMER).SendAsync(SIGNAL_NOTIFICATION_NEW_SALE_CUSTOMER);

                                        transaction.Complete();

                                        TempData["MessageSuccess"] = "Thêm khuyến mại thành công";

                                        return Redirect("/Admin/Sale/Index");
                                    }
                                }
                            }
                        }
                        else
                        {
                            ViewBag.MessageError = "Lỗi! Một vài sản phẩm đang được khuyễn mãi";

                            return View(model);
                        }
                    }
                }
                catch
                {
                    ViewBag.MessageError = "Lỗi hệ thống, vui lòng thử lại sau";
                }
            }

            return View(model);
        }
    }
}
