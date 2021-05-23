using Entities.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface IRecommendationService
    {
        public string[][] PrepareData();
        public List<ProductDetail> GetRecommandtion(int support, double confident);

        public AssociationRule<string>[] Rule(string[][] dataset, int suppport, double confident);
       

    }
}
