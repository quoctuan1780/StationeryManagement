using Accord.Math;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Common.Constant;

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
            var rec = await _context.Recommendations.Include(x => x.RecommendationDetails).OrderBy(x => x.CreateDate).FirstOrDefaultAsync();

            listInput.Sort();
            var productDetailIds = new List<int>();
            var arr = listInput.ToArray();
            var strs = new List<string>();
            FindSubsets(arr, strs);
            strs.Remove(EMPTY);
            var candidate = strs.Distinct().Select(x => x.Remove(x.Length - 1)).ToArray();

            foreach(var item in rec.RecommendationDetails)
            {
                var itemArr = item.Input.Split(SPACE).ToList();
                itemArr.Remove(EMPTY);
                var itemArrInt = itemArr.ConvertAll(x => int.Parse(x));
                itemArrInt.Sort((a, b) => a.CompareTo(b));
                var itemStr = string.Join(COMMA, itemArrInt);

                if (candidate.Any(x => x.Equals(itemStr)))
                {
                    var output = item.Output.Split(SPACE).ToList();
                    output.Remove(EMPTY);
                    productDetailIds.AddRange(output.ConvertAll(x => int.Parse(x)));
                }
            }

            if (productDetailIds.Any())
            {
                return await _context.ProductDetails.Include(x => x.Product).Where(x => productDetailIds.Distinct().Contains(x.ProductDetailId)).ToListAsync();
            }

            return new List<ProductDetail>();
        }

        private static void FindSubsets(int[] arr, List<string> strs)
        {
            int[] sub = new int[arr.Length];
            Find(arr, sub, 0, strs);
        }

        private static void Find(int[] arr, int[] sub, int p, List<string> strs)
        {
            //if the position variable p has iterated all elements   
            if (p == arr.Length)
            {
                //mechanism to print non zero elements  
                string s = string.Empty;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (sub[i] != 0)
                    {
                        s += sub[i].ToString() + ",";
                    }
                }
                strs.Add(s);
            }
            else
            {
                //For not selecting the element  
                sub[p] = 0;
                Find(arr, sub, p + 1, strs);

                //For selecting the element  
                sub[p] = arr[p];
                Find(arr, sub, p + 1, strs);
            }
        }

        public async Task<List<Product>> GetSuggestedProduct(List<int> listId)
        {
            var result = await _context.RecommendationDetails.ToListAsync();

            var productDetailIdsSuggestsStr = new List<string>();

            var productDetailIds = new List<int>();

            foreach (var item in result)
            {
                var itemArr = item.Input.Split(SPACE);

                if(listId.Any(x => itemArr.Contains(x.ToString())))
                {
                    productDetailIdsSuggestsStr.AddRange(item.Output.Split(SPACE));
                }
            }

            if (productDetailIdsSuggestsStr.Any())
            {
                foreach(var item in productDetailIdsSuggestsStr)
                {
                    var output = item.Split(SPACE).Where(x => x != EMPTY).Select(x => Convert.ToInt32(x));
                    productDetailIds.AddRange(output);
                }
            }

            if (productDetailIds.Any())
            {
                return await _context.Products
                    .AsNoTracking()
                    .Include(x => x.ProductDetails)
                    .Include(x => x.ProductImages)
                    .Include(x => x.RatingDetails)
                    .ThenInclude(x => x.Rating)
                    .Where(x => x.ProductDetails.Any(x => productDetailIds.Contains(x.ProductDetailId)))
                    .ToListAsync();
            }

            var product = await _context.Products.Include(x => x.ProductDetails).Where(x => x.ProductDetails.Select(x => x.ProductDetailId).Contains(listId.FirstOrDefault())).FirstOrDefaultAsync();

            return await _context.Products
                    .AsNoTracking()
                    .Include(x => x.ProductDetails)
                    .Include(x => x.ProductImages)
                    .Include(x => x.RatingDetails)
                    .ThenInclude(x => x.Rating)
                    .Where(x => x.CategoryId == product.CategoryId)
                    .ToListAsync();
        }

    }
}
