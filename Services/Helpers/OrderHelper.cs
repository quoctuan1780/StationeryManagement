using Entities.Models;

namespace Services.Helpers
{
    public static class OrderHelper
    {
        public struct OrderJoinHelper
        {
            public Order Order;

            public string ShipperName;

            public string WarehouseManagementName;
        };
    }
}
