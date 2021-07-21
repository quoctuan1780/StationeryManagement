using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IRecommendationService
    {
        public Task<List<ProductDetail>> GetRecommandtion(List<int> listInput);

        public Task<List<ProductDetail>> GetListProductDetailForCreateRRAsync(DateTime fromDate, DateTime toDate, int quantity);

        public Task<List<Product>> GetSuggestedProduct(List<int> listId);

        Task<int> AddOrderDetail();
    }
}
