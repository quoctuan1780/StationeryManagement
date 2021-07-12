namespace Common
{
    public static class SignalRConstant
    {
        public const string SIGNAL_COUNT_NEW_ORDER = "newOrder";
        public const string SIGNAL_COUNT_NEW_ACCOUNT = "newAccount";
        public const string SIGNAL_COUNT_NEW_RECEPT = "newWarehouse";
        public const string SIGNAL_TOP_PRODUCT = "topProduct";
        public const string SIGNAL_TOP_REVENUE = "newRevenue";
        public const string SIGNAL_TOP_REVENUE_CURRENT_MONTH = "topRevenueCurrentMonth";
        public const string SIGNAL_GROUP_ADMIN = "Admin";
        public const string SIGNAL_NOTIFICATION_NEW_ORDER_ADMIN = "newNotification";
        public const string SIGNAL_NOTIFICATION_REJECT_ORDER_ADMIN = "rejectNotification";
        public const string SIGNAL_NOTIFICATION_REJECT_ORDER_CUSTOMER = "rejectNotificationCustomer";
        public const string SIGNAL_NOTIFICATION_NEW_SALE_CUSTOMER = "newSaleNotificationCustomer";
        public const string SIGNAL_NOTIFICATION_NEW_RECEIPT_REQUEST = "newReceiptRequestNotification";
        public const string SIGNAL_NOTIFICATION_APPROVED_RECEIPT_REQUEST = "appvoredReceiptRequestNotification";
        public const string SIGNAL_NOTIFICATION_REJECT_RECEIPT_REQUEST = "rejectReceiptRequestNotification";

        public const string SIGNAL_GROUP_WAREHOUSE = "WarehouseManager";
        public const string SIGNAL_GROUP_SHIPPER = "Shipper";
        public const string SIGNAL_COUNT_ORDER_WAIT_TO_PICK = "orderWaitForPick";
        public const string SIGNAL_COUNT_ORDER_WAIT_TO_DELIVERY = "orderWaitToDelivery";
        public const string SIGNAL_COUNT_ORDER_DELIVERING = "orderDelivering";
        public const string SIGNAL_COUNT_ORDER_DELIVERED = "orderDelivered";
        public const string SIGNAL_COUNT_ORDER_WAIT_TO_REJECT = "newOrderWaitReject";
        public const string SIGNAL_COUNT_RECEPT_REQUEST_ACCEPT = "AcceptOrders";

        public const string SIGNAL_GROUP_CUSTOMER = "Customer";
    }
}
