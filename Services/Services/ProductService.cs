using Entities.Data;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopDbContext _context;

        public ProductService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.AddAsync(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<string> BestSellerInMonthAsync(DateTime fromDate, DateTime toDate, int quantity)
        {
            var result = await _context.OrderDetails
                    .Include(x => x.Order)
                    .Where(x => x.Order.OrderDate >= fromDate)
                    .Where(x => x.Order.OrderDate <= toDate)
                    .GroupBy(x => x.ProductDetailId)
                    .OrderByDescending(x => x.Key)
                    .Take(quantity)
                    .Select(x => x.Key)
                    .ToListAsync();
            
            var tolist =  await _context.ProductDetails.Include(x => x.Product).Where(x => result.Contains(x.ProductDetailId)).ToListAsync();
            var bestSellerList = new List<JObject>();

            foreach (var item in tolist)
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

                bestSellerList.Add(obj);
            }

            return JsonConvert.SerializeObject(bestSellerList);
        }

        public async Task<int> DeleteProductByIdAsync(int productId)
        {
            var product = await _context.Products.FindAsync(productId);

            if (!(product is null))
            {
                product.IsDeleted = true;

                _context.Update(product);

                return await _context.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<IList<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Where(x => x.IsDeleted == false)
                .Include(x => x.Category)
                .Include(x => x.ProductImages.Where(x => x.IsDeleted == false))
                .Take(16)
                .ToListAsync();
        }

        //check deteted item
        public async Task<IList<string>> GetColorByIdAsync(int productId)
        {
            var listDetails = await _context.ProductDetails.Where(x => x.ProductId == productId).ToListAsync();
            var listColor = new List<string>();
            foreach (var item in listDetails)
            {
                listColor.Add(item.Color);
            }
            return listColor;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.Where(x => x.ProductId == id)
                            .Include(x => x.Category)
                            .Include(x => x.ProductImages.Where(y => y.IsDeleted == false))
                            .Include(x => x.ProductDetails.Where(y => y.IsDeleted == false))
                            .Where(x => x.IsDeleted == false)
                            .FirstOrDefaultAsync();
        }

        public async Task<ProductDetail> GetProductDetailAsync(int id)
        {
            var item = await _context.ProductDetails.FindAsync(id);
            return item;
        }

        //check deteted item
        public async Task<IList<ProductDetail>> GetProductDetailsRunOutOfStockAsync()
        {
            var result = await _context.ProductDetails.Include(x => x.Product).Where(x => x.RemainingQuantity < 10).ToListAsync();

            dynamic a = 0;

            return result;
        }

        public string GetProductSkip(int skip)
        {
            var result = _context.Products.Include(x => x.ProductImages.Where(x => x.IsDeleted == false))
                .Where(x => x.IsDeleted == false)
                .Skip(skip).Take(16);

            var jsonResult = new List<JObject>();

            if(result != null && result.Any())
            {
                foreach (var item in result)
                {
                    var jObject = new JObject
                    {
                        {"ProductId", item.ProductId },
                        {"ProductName", item.ProductName },
                        {"Price", item.Price }
                    };

                    if (item.ProductImages != null && item.ProductImages.Any())
                    {
                        jObject.Add("ProductImage", item.ProductImages.FirstOrDefault().Image);
                    }
                    else
                    {
                        jObject.Add("ProductImage", "");
                    }

                    jsonResult.Add(jObject);
                }
            }

            return JsonConvert.SerializeObject(jsonResult);
        }


        //check deteted item
        public async Task<IList<ProductDetail>> GetProductWithDetailsAsync()
        {
            return await _context.ProductDetails.Include(x => x.Product).ToListAsync();
        }

        public async Task<IList<ProductDetail>> GetTop10ProductHotAsync()
        {
            var result = (from od in _context.OrderDetails
                          join
                          o in _context.Orders on od.OrderId equals o.OrderId
                          where o.OrderDate.Month == DateTime.Now.Month && o.OrderDate.Year == DateTime.Now.Year
                          group od by od.ProductDetailId into g
                          select new
                          {
                              ProductDetailId = g.Key,
                              Quantity = g.Sum(x => x.Quantity)
                          }).OrderByDescending(x => x.Quantity).Select(x => x.ProductDetailId).Take(10);

            if (result == null || !result.Any())
            {
                return null;
            }

            var productDetails = await _context.ProductDetails.Include(x => x.Product).Where(x => result.Contains(x.ProductDetailId)).ToListAsync();

            return productDetails;
        }

        public async Task<bool> IsExistsProduct(Product product)
        {
            var result = await _context.Products.Where(x => x.ProductName == product.ProductName).FirstOrDefaultAsync();

            if (result is null)
            {
                return false;
            }

            return true;
        }

        public async Task<IList<Product>> SearchByPriceAsync(int price)
        {
            return await _context.Products.Where(x => x.Price <= price).OrderBy(x => x.Price).ToListAsync();
        }

        public async Task<IList<Product>> SearchByPriceAsync(string text)
        {
            return await _context.Products.Where(x => x.ProductName.Contains(text)).OrderBy(x => x.Price)
                .Union(_context.Products.Include(x => x.Category).Where(x => x.Category.CategoryName.Contains(text))).ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            if (product is null) return null;

            _context.Products.Update(product);

            await _context.SaveChangesAsync();

            return product;
        }
    }
}
