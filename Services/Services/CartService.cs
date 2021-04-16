using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Schema;
using Services.Interfacies;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Services
{
    public class CartService : ICartService
    {
        private readonly ShopDbContext _context;

        public CartService(ShopDbContext context)
        {
            _context = context;
        }

        public async Task<CartItem> AddToCartAsync(CartItem cartItem)
        {
            await _context.AddAsync(cartItem);

            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task<int> DeleteCartItemAsync(int productDetailId, string userId)
        {
            var cartItem = await _context.CartItems.Where(x => x.ProductDetailId == productDetailId && x.UserId == userId).FirstOrDefaultAsync();

            _context.Remove(cartItem);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteCartItemByIdAsync(int productDetailId)
        {
            var result = await _context.CartItems.Where(x => x.ProductDetailId == productDetailId).FirstOrDefaultAsync();

            _context.Remove(result);

            return await _context.SaveChangesAsync();
        }

        public async Task<CartItem> GetCartItemByProuctDetailIdAsync(int productDetailId)
        {
            return await _context.CartItems.Where(x => x.ProductDetailId == productDetailId).FirstOrDefaultAsync();
        }

        public async Task<IList<CartItem>> GetCartsByUserIdAsync(string userId)
        {
            return await _context.CartItems.Where(x => x.UserId == userId)
                        .Include(x => x.User)
                        .Include(x => x.ProductDetail)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.Category)
                        .Include(x => x.ProductDetail)
                        .ThenInclude(x => x.Product)
                        .ThenInclude(x => x.ProductImages)
                        .ToListAsync();
        }

        public decimal GetCartTotalByUserId(string userId)
        {
            var result = _context.CartItems.Where(x => x.UserId == userId);

            if (!result.Any()) return 0;

            return result.Sum(x => x.Quantity * x.Price);
        }

        public async Task<int> RemoveCartItemByUserId(string userId)
        {
            var carts = _context.CartItems.Where(x => x.UserId == userId);

            _context.RemoveRange(carts);

            return await _context.SaveChangesAsync();
        }

        public async Task<CartItem> UpdateCartItemAsync(CartItem cartItem)
        {
            _context.Update(cartItem);

            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task<int> UpdateQuantityOfCartItemAsync(int productDetailId, int quantity, string userId)
        {
            var result = 
                _context.CartItems.Where(x => x.ProductDetailId == productDetailId && x.UserId == userId).FirstOrDefault();

            result.Quantity = quantity;

            _context.Update(result);

            return await _context.SaveChangesAsync();
        }
    }
}
