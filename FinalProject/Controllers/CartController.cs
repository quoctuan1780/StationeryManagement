using Common;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace FinalProject.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IAccountService _accountService;

        public CartController(ICartService cartService, IAccountService accountService)
        {
            _cartService = cartService;
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<string> AddToCart(int? productDetailId, int? quantity, decimal? price)
        {
            if (productDetailId is null || quantity is null || price is null)
                return Constant.MISS_VALUE;

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var userId = _accountService.GetUserId(User);

                var cartItem = await _cartService.GetCartItemByProuctDetailIdAsync(productDetailId.Value);

                if (cartItem is null)
                {

                    var cart = new CartItem()
                    {
                        UserId = userId,
                        ProductDetailId = productDetailId.Value,
                        Quantity = quantity.Value,
                        CreateDate = DateTime.Now,
                        Price = price.Value
                    };

                    var result = await _cartService.AddToCartAsync(cart);

                    if (!(result is null))
                    {
                        transaction.Complete();

                        return JsonConvert.SerializeObject(result);
                    }
                }
                else
                {
                    cartItem.Quantity = quantity.Value;

                    var result = await _cartService.UpdateCartItemAsync(cartItem);

                    if (!(result is null))
                    {
                        transaction.Complete();

                        return Constant.UPDATED;
                    }
                }
            }

            return Constant.EMPTY;
        }

        [HttpDelete]
        public async Task<string> RemoveCartItem(int? productDetailId)
        {
            var userId = _accountService.GetUserId(User);

            if (productDetailId is null || userId is null)
            {
                return Constant.MISS_VALUE;
            }

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var result = await _cartService.DeleteCartItemAsync(productDetailId.Value, userId);

                if (result > 0)
                {
                    transaction.Complete();

                    return Constant.SUCCESS;
                }
            }

            return Constant.FAIL;
        }

        [HttpGet]
        public decimal GetCartTotal()
        {
            var userId = _accountService.GetUserId(User);

            decimal total = _cartService.GetCartTotalByUserId(userId);

            return total;
        }
    }
}