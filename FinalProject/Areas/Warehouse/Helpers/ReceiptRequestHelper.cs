using System;
using System.Collections.Generic;

namespace FinalProject.Areas.Warehouse.Helpers
{
    public class ReceiptRequestHelper
    {
        public static IList<int> ConvertStringToListInt(string value)
        {
            var result = new List<int>();

            if (value is null)
            {
                return result;
            }

            var splitStr = value.Split(",");

            foreach (var item in splitStr)
            {
                result.Add(Convert.ToInt32(item));
            }

            return result;
        }

        public static IList<decimal> ConvertStringToListDecimal(string value)
        {
            var result = new List<decimal>();

            if (value is null)
            {
                return result;
            }

            var splitStr = value.Split(",");

            foreach (var item in splitStr)
            {
                result.Add(Convert.ToDecimal(item));
            }

            return result;
        }
    }
}
