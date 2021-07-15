using System;
using System.Collections.Generic;

namespace FinalProject.Areas.Admin.Helpers
{
    public class SaleHelper
    {
        public static IList<int> ConvertStringToListInt(string value)
        {
            var result = new List<int>();

            if(value is null)
            {
                return result;
            }

            var splitStr = value.Split(",");

            foreach(var item in splitStr)
            {
                result.Add(Convert.ToInt32(item));
            }

            return result;
        }
    }
}
