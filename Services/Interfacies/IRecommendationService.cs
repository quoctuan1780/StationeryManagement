using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IRecommendationService
    {
        
        public Task<IList<ProductDetail>> GetRecommandtion(List<int> listInput);

        

        public Task<List<Product>> GetSuggestedProduct(List<int> listId);
        

    }
}
