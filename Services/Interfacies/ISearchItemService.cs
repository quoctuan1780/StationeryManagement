﻿using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfacies
{
    public interface ISearchItemService
    {
        Task<IList<Product>> SearchByPriceAsync(int? price);
        Task<IList<Product>> SearchByTextAsync(string text);
        Task<IList<Product>> SearchAjaxAsync(string text);
    }
}
