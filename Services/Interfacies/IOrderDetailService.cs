using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IOrderDetailService
    {
        Task<int> AddOrderDetailAsync(Order order, IList<CartItem> cartItems);
        Task<int> AddOrderDetailAsync(Order order, CartItem cartItem);
    }
}
