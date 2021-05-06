using Common;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject.Controllers
{
    public class DeliveryAddressController : Controller
    {
        private readonly IDeliveryAddressService _deliveryAddressService;

        public DeliveryAddressController(IDeliveryAddressService deliveryAddressService)
        {
            _deliveryAddressService = deliveryAddressService;
        }

        [HttpPost]
        public async Task<string> AddDelivery(string wardCode, string userId, string streetName)
        {
            if (wardCode is null || userId is null || streetName is null)
                return Constant.EMPTY;

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var result = await _deliveryAddressService.AddDeliveryServiceAsync(wardCode, streetName, userId);

                if(result > 0)
                {
                    transaction.Complete();

                    return Constant.SUCCESS;
                }
            }
            catch
            {
            }

            return Constant.FAIL;
        }
    }
}
