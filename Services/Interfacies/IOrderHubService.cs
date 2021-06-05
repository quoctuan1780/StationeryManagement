using Entities.Models;
using System.Collections.Generic;

namespace Services.Interfacies
{
    public interface IOrderHubService
    {
        List<Order> GetOrders();
    }
}
