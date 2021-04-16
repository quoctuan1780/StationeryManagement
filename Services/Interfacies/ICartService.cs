using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface ICartService
    {
        Task<IList<CartItem>> GetCartsByUserIdAsync(string userId);

        Task<CartItem> AddToCartAsync(CartItem cartItem);

        Task<int> DeleteCartItemByIdAsync(int cartItemId);

        Task<CartItem> GetCartItemByProuctDetailIdAsync(int productDetailId);

        Task<CartItem> UpdateCartItemAsync(CartItem cartItem);

        Task<int> DeleteCartItemAsync(int productDetailId, string userId);

        decimal GetCartTotalByUserId(string userId);

        Task<int> UpdateQuantityOfCartItemAsync(int productDetailId, int quantity, string userId);

        Task<int> RemoveCartItemByUserId(string userId);
    }
}
