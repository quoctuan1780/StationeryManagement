using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class BackgroundWork
    {
        private readonly IProductService _productService;
        private readonly IAprioriBackground _aprioriBackground;

        public BackgroundWork(IProductService productService, IAprioriBackground aprioriBackground)
        {
            _productService = productService;
            _aprioriBackground = aprioriBackground;
        }

        public Task DoTask()
        {
            return _productService.UpdateSalePriceAsync();
        }

        public Task SaveRecommendation()
        {
            return _aprioriBackground.RecommandationBackground();
        }
    }
}
