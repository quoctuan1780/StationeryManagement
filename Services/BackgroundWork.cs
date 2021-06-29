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
        private readonly ITestService _testService;

        public BackgroundWork(ITestService testService)
        {
            _testService = testService;
        }

        public Task DoTask()
        {
            return _testService.UpdateProductTotalAsync();
        }
    }
}
