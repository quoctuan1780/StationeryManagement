using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.Services
{
    public class RecommandationService : IRecommendationService
    {
        private readonly ShopDbContext _context;

        public RecommandationService(ShopDbContext shopDbContext)
        {
            _context = shopDbContext;
        }

        public int AddRecommandation(AssociationRule<string>[] rules)
        {
            using var transaction = new TransactionScope();
            try
            {
                
                _context.RecommendationDetails.RemoveRange();
                _context.Recommendations.RemoveRange();
                var ReccomdHeader = new Recommendation()
                {
                    CreateDate = DateTime.Now
                };
                _context.Recommendations.Add(ReccomdHeader);
                if (_context.SaveChanges() > 0)
                {
                    List<RecommendationDetail> listRecommandLine = new();
                    foreach (var item in rules)
                    {
                        string x = "",y = "";
                        foreach(var i in item.X)
                        {
                            x += i+ " ";
                        }
                        foreach(var i in item.Y)
                        {
                            y += i + " ";
                        }
                        var recommandline = new RecommendationDetail()
                        {
                            RecommendationId = ReccomdHeader.RecommendtionId,
                            Input = x,
                            Output = y

                        };
                        listRecommandLine.Add(recommandline);
                    }
                    _context.AddRange(listRecommandLine);
                    int count = _context.SaveChanges();
                    if (count > 0)
                    {
                        transaction.Complete();
                        return count;
                    }
                }
            }
            catch
            {
                return 0;
            }


            return 0;
        }

        public async Task<IList<ProductDetail>> GetRecommandtion(int support, double confident, List<int> listInput)
        {
            var rec =  await _context.Recommendations.Include(x => x.RecommendationDetails).OrderBy(x => x.CreateDate).ToListAsync();
            var last =rec.LastOrDefault();
            if (last != null)
            {
                if (last.CreateDate.ToShortDateString().Equals(DateTime.Now.ToShortDateString()))
                {
                    var details = await _context.RecommendationDetails.Where(x => x.RecommendationId == last.RecommendtionId).ToListAsync();

                    var listProductId = new List<string>();
                    List<string> listConvert = listInput.ConvertAll<string>(x => x.ToString());
                    var listRecommendationDetail = new List<RecommendationDetail>();
                    foreach (var num in listInput) {

                        var RecommendationDetail = new List<RecommendationDetail>();
                        RecommendationDetail = details.Where(x => listConvert.Contains(x.Input)).ToList();
                        listRecommendationDetail.AddRange(RecommendationDetail);

                    }
                    foreach(var item in listRecommendationDetail)
                    {
                        listProductId.Add(item.Output);
                    }
                    
                    var listPro = _context.ProductDetails.Where(p => listProductId.Contains(p.ProductDetailId.ToString())).ToList();
                    return listPro;
                }
            }
            string[][] dataset = await PrepareData();

            AssociationRule<string>[] results = Rule(dataset, support, confident);
            //get list product
            var listProducts = new List<ProductDetail>();
            
            if (results != null)
            {
                var listProductDetailids = new List<string>();
                foreach(var item in results)
                {
                    listProductDetailids.AddRange(item.Y);
                };

                    listProductDetailids = listProductDetailids.Distinct().ToList();

                 listProducts = _context.ProductDetails.Include(x => x.Product).Where(x => listProductDetailids.Contains(x.ProductDetailId.ToString())).ToList();
                
                if (listProducts.Any())
                {
                    AddRecommandation(results);
                }
            }
            return listProducts;
        }

        public async Task<List<Product>> GetSuggestedProduct(List<int> listId)
        {
            var listDetail = await GetRecommandtion(4, 0.81, listId);
            var ID = new List<int>();
            foreach(var item in listDetail)
            {
                ID.Add(item.ProductId);
            }
            ID = ID.Distinct().ToList();
            return await _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Where(x => ID.Contains(x.ProductId)).ToListAsync();
        }

        public async Task<string[][]> PrepareData()
        {
            var listOrder = await _context.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.ProductDetail).OrderBy(x => x.OrderId).ToListAsync();
            int currentOrderId = listOrder.FirstOrDefault().OrderId;
            var dataset = new List<string[]>();
            var list = new List<string>();


            if (listOrder is not null)
            {
                dataset.Clear();
                foreach (var item in listOrder)
                {
                    foreach (var detail in item.OrderDetails)
                    {
                        list.Add(detail.ProductDetailId.ToString());
                    }
                    if (list is not null)
                    {
                        list.Sort();
                    }
                    dataset.Add((string[])list.ToArray());
                    list.Clear();
                }
            }

            return dataset.ToArray();
        }




        public AssociationRule<string>[] Rule(string[][] dataset, int suppport, double confident)
        {
            Apriori apriori = new Apriori(threshold: suppport, confidence: confident);

            // Use the algorithm to learn a set matcher
            AssociationRuleMatcher<string> classifier = apriori.Learn(dataset);

            // Use the classifier to find orders that are similar to 
            // orders where clients have bought 2 items together:
            AssociationRule<string>[] rules = classifier.Rules;
            return rules;
        }



    }
}
