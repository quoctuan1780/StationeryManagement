using static Common.Constant;
using static Common.RoleConstant;
using Microsoft.AspNetCore.Mvc;
using Services.Interfacies;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers
{
    [Authorize(Roles = ROLE_CUSTOMER)]
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
                return EMPTY;

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            try
            {
                var result = await _deliveryAddressService.AddDeliveryServiceAsync(wardCode, streetName, userId);

                if(result > 0)
                {
                    transaction.Complete();

                    return SUCCESS;
                }
            }
            catch
            {
            }

            return FAIL;
        }
    }
}
