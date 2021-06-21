namespace Common
{
    public static class SignalRConstant
    {
        public const string SIGNAL_COUNT_NEW_ORDER = "newOrder";
        public const string SIGNAL_COUNT_NEW_ACCOUNT = "newAccount";
        public const string SIGNAL_COUNT_NEW_RECEPT = "newWarehouse";
        public const string SIGNAL_TOP_PRODUCT = "topProduct";
        public const string SIGNAL_GROUP_ADMIN = "Admin";

        public const string SIGNAL_GROUP_WAREHOUSE = "WarehouseManager";
        public const string SIGNAL_GROUP_SHIPPER = "Shipper";
        public const string SIGNAL_COUNT_ORDER_WAIT_TO_PICK = "orderWaitForPick";
        public const string SIGNAL_COUNT_ORDER_WAIT_TO_DELIVERY = "orderWaitToDelivery";
        public const string SIGNAL_COUNT_ORDER_DELIVERING = "orderDelivering";
        public const string SIGNAL_COUNT_ORDER_DELIVERED = "orderDelivered";
    }
}
