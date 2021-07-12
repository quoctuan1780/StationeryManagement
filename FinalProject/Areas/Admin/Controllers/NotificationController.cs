using static Common.Constant;
using static Common.RoleConstant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Services.Interfacies;
using X.PagedList;

namespace FinalProject.Areas.Admin.Controllers
{
    [Area(AREA_ADMIN)]
    [Authorize(Roles = ROLE_ADMIN, AuthenticationSchemes = ROLE_ADMIN)]
    public class NotificationController : Controller
    {
        private readonly INotificationService _notificationService;
        private readonly IAccountService _accountService;

        public NotificationController(INotificationService notificationService, IAccountService accountService)
        {
            _notificationService = notificationService;
            _accountService = accountService;
        }
        public async Task<IActionResult> Notifications(int? page = 1)
        {
            var user = await _accountService.GetUserAsync(User);

            var notifications = await _notificationService.GetNotificationsAsync(ROLE_ADMIN, user.Id);

            var model = notifications.ToPagedList(page.Value, 10);

            return View(model);
        }

        [HttpDelete]
        public async Task<int> DeleteNofity(int? notificationId)
        {
            if(notificationId is null)
            {
                return ERROR_CODE_NULL;
            }

            var result = await _notificationService.DeleteStatusAsync(notificationId.Value);

            if(result > 0)
            {
                return CODE_SUCCESS;
            }

            return ERROR_CODE_SYSTEM;
        }

        [HttpPut]
        public async Task<int> SeenNotify(int? notificationId)
        {
            if(notificationId is null)
            {
                return ERROR_CODE_NULL;
            }

            var result = await _notificationService.UpdateStatusAsync(notificationId.Value);

            if(result > 0)
            {
                return CODE_SUCCESS;
            }

            return ERROR_CODE_SYSTEM;
        }
    }
}
