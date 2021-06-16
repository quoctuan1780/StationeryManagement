﻿using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IList<ProductDetail>> BestSellerInMonthAsync(DateTime fromDate, DateTime toDdate, int quantity)
        {
            var result = await _context.OrderDetails
                    .Include(x => x.Order)
                    .Where(x => x.Order.OrderDate.Month >= fromDate.Month && x.Order.OrderDate.Month <= toDdate.Month)
                    .Where(x => x.Order.OrderDate.Year >= fromDate.Year && x.Order.OrderDate.Year <= toDdate.Year)
                    .GroupBy(x => x.ProductDetailId)
                    .OrderByDescending(x => x.Key)
                    .Take(quantity)
                    .Select(x => x.Key)
                    .ToListAsync();

            return await _context.ProductDetails.Include(x => x.Product).Where(x => result.Contains(x.ProductDetailId)).ToListAsync();
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
                            .FirstOrDefaultAsync();
        }

        //check deteted item
        public async Task<IList<ProductDetail>> GetProductDetailsRunOutOfStockAsync()
        {
            var result = await _context.ProductDetails.Include(x => x.Product).Where(x => x.RemainingQuantity < 10).ToListAsync();

            dynamic a = 0;

            return result;
        }


        //check deteted item
        public async Task<IList<ProductDetail>> GetProductWithDetailsAsync()
        {
            return await _context.ProductDetails.Include(x => x.Product).ToListAsync();
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
