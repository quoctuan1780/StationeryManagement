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

        public BackgroundWork(IProductService productService)
        {
            _productService = productService;
        }

        public Task DoTask()
        {
            return _productService.UpdateSalePriceAsync();
        }
    }
}
