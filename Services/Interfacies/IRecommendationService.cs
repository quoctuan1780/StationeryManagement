using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IRecommendationService
    {
        public Task<string[][]> PrepareData();
        public Task<IList<ProductDetail>> GetRecommandtion(int support, double confident, List<int> listInput);

        public AssociationRule<string>[] Rule(string[][] dataset, int suppport, double confident);

        public Task<List<Product>> GetSuggestedProduct(List<int> listId);
        int AddRecommandation(AssociationRule<string>[] rules);

    }
}
