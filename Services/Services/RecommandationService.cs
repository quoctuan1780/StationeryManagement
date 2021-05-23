using Accord.Math;
using Accord.Statistics.Kernels;
using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Schema;
using Services.Interfacies;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class RecommandationService : IRecommendationService
    {
        private readonly ShopDbContext _context;

        public RecommandationService(ShopDbContext shopDbContext)
        {
            _context = shopDbContext;
        }
        public List<ProductDetail> GetRecommandtion(int support, double confident)
        {
            string[][] dataset = PrepareData();

            AssociationRule<string>[] results = Rule(dataset, support, confident);
            //get list product
            var listProduct = new List<ProductDetail>();
            var allProduct = _context.ProductDetails.Include(x => x.Product).ToList();
            if(allProduct is not null && results is not null)
            {
                foreach (var item in allProduct)
                {
                    for (var i = 0; i < results.Length; i++)
                    {
                        if (results[i].Y.Contains(item.ProductId.ToString()) && !listProduct.Exists(x => x.ProductDetailId  == item.ProductDetailId))
                        {
                            
                            listProduct.Add(item);
                        }
                    }
                }
            }
            
            return listProduct;
        }

       

        public string[][] PrepareData()
        {
            var listOrder =  _context.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.ProductDetail).OrderBy(x => x.OrderId).ToList();
            int currentOrderId = listOrder.FirstOrDefault().OrderId;
            var dataset = new List<string[]>();          
            var list = new List<string>();


            if(listOrder is not null)
            {
                dataset.Clear();
                foreach (var item in listOrder)
                {
                    foreach (var detail in item.OrderDetails)
                    {
                        list.Add(detail.ProductDetail.ProductId.ToString());
                    }
                    if(list is not null)
                    {
                        list.Sort();
                    }                       
                    dataset.Add((string[])list.ToArray());
                    list.Clear();                     
                }
            }
            
            return dataset.ToArray();
        } 
        
       

       
        public AssociationRule<string>[] Rule(string[][] dataset,int suppport, double confident)
        {
            Apriori apriori = new Apriori(threshold: suppport, confidence: confident);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<string> classifier = apriori.Learn(dataset);

            // Use the classifier to find orders that are similar to 
            // orders where clients have bought 2 items together:
            AssociationRule<string>[] rules= classifier.Rules;
            return rules;
        }

       
        
    }
}
