using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class RecommandationService : IRecommendationService
    {
        private readonly IProductService _productService;
        private readonly ShopDbContext _context;


        public RecommandationService(ShopDbContext shopDbContext, IProductService productService)
        {
            _productService = productService;
            _context = shopDbContext;

        }

        public async Task<List<ProductDetail>> GetListProductDetailForCreateRRAsync(DateTime fromDate, DateTime toDate, int quantity)
        {
            var listUnion = await _productService.GetProductDetailsRunOutOfStockAsync();

            var listBestSeller = await _productService.GetBestSellerInMonthAsync(fromDate, toDate, quantity);

            var listId = await _productService.ListBestSellerProduct(fromDate, toDate, quantity);

            var listRecommandation = await GetRecommandtion(listId);

            foreach (var item in listBestSeller)
            {
                listUnion.Add(item);
            }

            foreach (var item in listRecommandation)
            {
                listUnion.Add(item);
            }

            listUnion = listUnion.Distinct().ToList();

            return listUnion;
        }

        public async Task<List<ProductDetail>> GetRecommandtion(List<int> listInput)
        {
            var listPro = new List<ProductDetail>();
            var rec = await _context.Recommendations.Include(x => x.RecommendationDetails).OrderBy(x => x.CreateDate).FirstOrDefaultAsync();
            if (rec != null)
            {

                var details = await _context.RecommendationDetails.Where(x => x.RecommendationId == rec.RecommendtionId).ToListAsync();

                var listProductId = new List<string>();
                List<string> listConvert = listInput.ConvertAll<string>(x => x.ToString());
                var listRecommendationDetail = new List<RecommendationDetail>();

                foreach (var num in listInput)
                {

                    var RecommendationDetail = new List<RecommendationDetail>();
                    RecommendationDetail = details.Where(x => listConvert.Contains(x.Input)).ToList();
                    listRecommendationDetail.AddRange(RecommendationDetail);

                }
                foreach (var item in listRecommendationDetail)
                {
                    listProductId.Add(item.Output);
                }

                listPro = _context.ProductDetails.Include(x => x.Product).Where(p => listProductId.Contains(p.ProductDetailId.ToString())).ToList();


            }

            return listPro;
        }

        public async Task<List<Product>> GetSuggestedProduct(List<int> listId)
        {
            var support = await _context.ProductDetails.ToListAsync();
            int minsupp = (int)Math.Round(support.Count * 0.5);
            var productDetails = await GetRecommandtion(listId);
            var productIds = new List<int>();

            foreach (var item in productDetails)
            {
                productIds.Add(item.ProductId);
            }

            productIds = productIds.Distinct().ToList();

            var listSuggested = await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Include(x => x.RatingDetails).Where(x => productIds.Contains(x.ProductId)).ToListAsync();

            if (listSuggested != null)
            {
                var productDetail = await _context.ProductDetails.Include(x => x.Product).Where(x => x.ProductDetailId == listId[0]).FirstOrDefaultAsync();

                return await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Include(x => x.RatingDetails).Where(x => x.CategoryId == productDetail.Product.CategoryId).ToListAsync();
            }

            return listSuggested;
        }

    }
}
