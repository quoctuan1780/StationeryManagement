﻿using Entities.Data;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Services.Services
{
    public class AprioriBackground : IAprioriBackground
    {
        private readonly ShopDbContext _context;

        public AprioriBackground(ShopDbContext shopDbContext)
        {
            _context = shopDbContext;
        }
        public async Task RecommandationBackground()
        {
            var listProductDetails = await _context.ProductDetails.ToListAsync();
            int minsupp = (int)Math.Round(listProductDetails.Count * 0.5);
            string[][] dataset = await PrepareData();
            AssociationRule<string>[] results = Rule(dataset, minsupp, 0.81);
            var listProducts = new List<ProductDetail>();

            if (results != null)
            {
                var listProductDetailids = new List<string>();
                foreach (var item in results)
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
                        string x = "", y = "";
                        foreach (var i in item.X)
                        {
                            x += i + " ";
                        }
                        foreach (var i in item.Y)
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