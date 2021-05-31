using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services.Interfacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchItemService _searchService;

        public SearchController(ISearchItemService searchItemService)
        {
            _searchService = searchItemService; 
        }
        public async Task<IActionResult> Search(string text)
        {
            int price = -1;
            int.TryParse(text,out price);
            if(price > 0)
            {
                ViewBag.ListProduct = await _searchService.SearchByPriceAsync(price);
            }
            else
            {
                ViewBag.ListProduct = await _searchService.SearchByTextAsync(text);
            }
            return View();
        }
        public string SearchAjax(string text)
        {
            return JsonConvert.SerializeObject(_searchService.SearchAjaxAsync(text));
        }
    }
}
