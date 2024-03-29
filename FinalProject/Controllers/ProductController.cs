﻿using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Collections.Generic;
using System.Threading.Tasks;
using X.PagedList;
using static Common.Constant;

namespace FinalProject.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICommentService _commentService;
        private readonly IRatingService _rateService;
        private readonly IUserConnectionService _userConnectionService;
        private readonly IRecommendationService _recommendationService;

        public ProductController(IProductService productService, ICommentService commentService, IRecommendationService recommendationService, IRatingService rateService, IUserConnectionService userConnectionService)
        {
            _productService = productService;
            _commentService = commentService;
            _recommendationService = recommendationService;
            _rateService = rateService;
            _userConnectionService = userConnectionService;
        }
        public async Task<IActionResult> Detail(int? id, int? page = 1)
        {
            if(id is null)
            {
                return PartialView(ERROR_404_PAGE);
            }

            var userConnection = Request.HttpContext.Connection.RemoteIpAddress;

            await _userConnectionService.AddUserConnectionAsync(userConnection.ToString(), id.Value);

            ViewBag.Product = await _productService.GetProductByIdAsync(id.Value);

            ViewBag.Comments = await _commentService.GetAllCommentsByProductIdAsync(id.Value);

            ViewBag.Ratings = await _rateService.GetRatingsAsync();

            ViewBag.RatingsDetail = await _rateService.GetRatingsDetailAsync(id.Value);

            var listProductDetailId = await _productService.GetProductDetailByProDuctIdAsync(id.Value);

            var productSuggests = await _recommendationService.GetSuggestedProduct(listProductDetailId);

            var model = productSuggests.ToPagedList(page.Value, 12);

            return View(model);
        }

        public string GetProductSkip(int? skip)
        {
            if(skip is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            try
            {
                return _productService.GetProductSkip(skip.Value);
            }
            catch
            {

            }

            return ERROR_CODE_SYSTEM.ToString();
        }
    }
}
