using static Common.RoleConstant;
using static Common.Constant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Services.Interfacies;
using System.Transactions;
using Entities.Models;
using System;

namespace FinalProject.Controllers
{
    public class RatingController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRatingService _ratingService;

        public RatingController(IAccountService accountService, IRatingService ratingService)
        {
            _accountService = accountService;
            _ratingService = ratingService;
        }

        [HttpPost]
        [Authorize(Roles = ROLE_CUSTOMER, AuthenticationSchemes = ROLE_CUSTOMER)]
        public async Task<int> AddRating(int? ratingId, string content, int? productId)
        {
            if(productId is null || ratingId is null || content is null || content.Equals(EMPTY))
            {
                return ERROR_CODE_NULL;
            }

            using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            try
            {
                var user = await _accountService.GetUserAsync(User);

                var checkRateExists = await _ratingService.CheckExistsRatingAsync(productId.Value, user.Id);

                if (!checkRateExists)
                {

                    var rating = new RatingDetail()
                    {
                        UserId = user.Id,
                        ProductId = productId.Value,
                        Content = content,
                        RatingId = ratingId.Value,
                        RatingDate = DateTime.Now
                    };

                    var result = await _ratingService.AddRatingAsync(rating);
                    if (result > 0)
                    {
                        transaction.Complete();

                        return CODE_SUCCESS;
                    }
                }
                else
                {
                    return ERROR_CODE_EXISTS;
                }
            }
            catch
            {

            }

            return ERROR_CODE_SYSTEM;
        }

        public async Task<string> GetMoreRating(int? skip, int? productId)
        {
            if(skip is null || productId is null)
            {
                return ERROR_CODE_NULL.ToString();
            }

            var result = await _ratingService.GetRatingsDetailJsonAsync(productId.Value, skip.Value);

            if(result != null)
            {
                return result;
            }

            return ERROR_CODE_SYSTEM.ToString();
        }
    }
}
