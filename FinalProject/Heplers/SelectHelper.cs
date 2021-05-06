using Common;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FinalProject.Heplers
{
    public class SelectHelper
    {
        public static IList<SelectListItem> ConvertDeliveryAddressesToSelectListItems(IList<DeliveryAddress> deliveryAddresses)
        {
            var selectListItems = new List<SelectListItem>();
            foreach(var item in deliveryAddresses)
            {
                selectListItems.Add(new SelectListItem()
                {
                    Text = item.StreetName + Constant.HYPHEN + item.Ward.WardName + 
                    Constant.HYPHEN + item.Ward.District.DistrictName + Constant.HYPHEN + item.Ward.District.Province.ProvinceName,
                    Value = item.DeliveryAddressId.ToString()
                });
            }
            return selectListItems;
        }
    }
}
