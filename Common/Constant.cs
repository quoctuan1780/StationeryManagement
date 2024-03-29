﻿using System.Runtime.CompilerServices;

namespace Common
{
    public class Constant
    {
        #region Constant 
        public const string CONNECTION_STRING = "DefaultConnection";

        public const string EMPTY = "";

        public const string SPACE = " ";

        public const string DOUBLE_EMPTY = "  ";

        public const string IMAGE_LINK = "wwwroot/images/product/";

        public const string IMAGE_LINK_BANNER = "wwwroot/images/banner/";

        public const string IMAGE_AVATAR_LINK = @"wwwroot\images\user";

        public const string IMAGE_SALE_LINK = @"wwwroot\images\sale";

        public const string SLASH = "/";

        public const string HYPHEN = " - ";

        public const string IMAGE_KEY = "imageKey";

        public const string SUCCESS = "Success";

        public const string FAIL = "Fail";

        public const string MISS_VALUE = "Miss";

        public const string UPDATED = "Updated";

        public const string COMMA = ",";

        public const string DOT = ".";

        public const string NULL = "null";

        public const string AREA_ADMIN = "Admin";

        public const string AREA_SHIPPER = "Shipper";

        public const string AREA_WAREHOUSE = "Warehouse";

        public const double EXCHANGE_RATE_USD = 23070.26;

        public const string CURRENCY_USE = "USD";

        public const string ZERO = "0";

        public const string VN_CODE = "VN";

        public const string VN_NAME = "Việt Nam";

        public const string METHOD_POST = "POST";

        public const string COD = "COD";

        public const string G29 = "G29";

        public const string COD_SUCCESS = "CodSuccess";

        public const string PROVIDER_GOOGLE = "Google";
        public const string PROVIDER_FACEBOOK = "Facebook";
        #endregion

        #region Error Page
        public const string ERROR_404_PAGE = "~/Views/Shared/_Error404.cshtml";
        public const string ERROR_1000_PAGE = "~/Views/Shared/_Error1000.cshtml";
        public const string ERROR_404_PAGE_ADMIN = "~/Areas/Admin/Views/Shared/_Error404.cshtml";
        public const string ERROR_404_PAGE_SHIPPER = "~/Areas/Shipper/Views/Shared/_Error404.cshtml";
        public const string ERROR_404_PAGE_WAREHOUSE = "~/Areas/Warehouse/Views/Shared/_Error404.cshtml";
        public const string ERROR_PAYMENT_PAGE = "~/Views/Shared/_ErrorPayment.cshtml";
        #endregion

        #region Error Code
        public const int ERROR_CODE_NULL_ID = -2;

        public const int ERROR_CODE_EXISTS = -5;

        public const int ERROR_CODE_NULL = -4;

        public const int ERROR_CODE_CANNOT_FIND_INFOR_BY_ID = -1;

        public const int ERROR_CODE_CONVERT_TO_INT = -3;

        public const int ERROR_CODE_DO_NOT_CONFIRM_EMAIL = 5;

        public const int ERROR_CODE_SYSTEM = -99;
        #endregion

        public const byte CODE_SUCCESS = 1;

        public const byte CODE_LOOK_ACCOUNT = 2;

        public const byte CODE_FAIL = 3;

        public const byte CODE_NOT_EXISTS_ACCOUNT = 4;

        #region Model Field
        public const string MODEL_FIELD_PASSWORD = "Password";
        public const string MODEL_FIELD_CONFIRM_PASSWORD = "ConfirmPassword";
        #endregion

        #region Email Sender
        public const string EMAIL_SENDER_PORT = "EmailSender:Port";
        public const string EMAIL_SENDER_HOST = "EmailSender:Host";
        public const string EMAIL_SENDER_ENABLE_SSL = "EmailSender:EnableSsl";
        public const string EMAIL_SENDER_USE_DEFAULT_CREENTIALS = "EmailSender:UseDefaultCredentials";
        public const string EMAIL_SENDER_EMAIL = "EmailSender:Email";
        public const string EMAIL_SENDER_PASSWORD = "EmailSender:Password";

        public const string EMAIL_SUBJECT = "Xác nhận email";
        public const string EMAIL_HEADER_START = "<h2>Chào mừng bạn ";
        public const string EMAIL_HEADER_END = "</h2>";
        public const string EMAIL_BODY_START = "Chào mừng bạn trở thành thành viên của Shop" + "\n" + "Vui lòng nhấn vào link sau để xác nhận tài"
                    + " khoản của bạn: " + "<a href='";
        public const string EMAIL_BODY_END = "'>Nhấn vào đây</a>";
        #endregion

        #region Route link
        public const string ROUTE_HOME_INDEX_CLIENT = "/Home/Index";
        public const string ROUTE_REGISTER_CLIENT = "/Account/Register";
        public const string ROUTE_LOGIN_CLIENT = "/Account/Login";
        #endregion

        #region View name
        public const string VIEW_LOGIN = "Login";
        public const string VIEW_REGISTER_WITH_GOOGLE = "RegisterWithGoogle";
        public const string VIEW_REGISTER_WITH_FACEBOOK = "RegisterWithFacebook";
        public const string VIEW_EXTERNAL_LOGIN = "ExternalLoginCallback";
        #endregion

        public const string ACTION_CONFIRM_EMAIL = "ConfirmEmail";
        public const string CONTROLLER_ACCOUNT = "Account";

        public const string KEY_CONFIRM_EMAIL = "Confirm";
        public const string KEY_CONFIRM_EMAIL_SUCCESS = "ConfirmSuccess";
        public const string KEY_PAYPAL_CLIENT_ID = "PAYPAL_CLIENT_ID";
        public const string KEY_PAYPAL_CLIENT_SECRET = "PAYPAL_CLIENT_SECRET";

        #region PayPal
        public const string PAYPAL = "PayPal";
        public const string PAYPAL_METHOD = "paypal";
        public const string PAYPAL_CLIENT_ID = "PayPal:ClientId";
        public const string PAYPAL_SECRET = "PayPal:Secret";
        public const string PAYPAL_CANCEL_URL = "PayPal:CancelUrl";
        public const string PAYPAL_RETURN_URL = "PayPal:ReturnUrl";
        public const string PAYPAL_SKU = "sku";
        public const string PAYPAL_CAPTURE = "CAPTURE";
        public const string PAYPAL_PHYSICAL_GOODS = "PHYSICAL_GOODS";
        public const string PAYPAL_HEADER_PREFER = "prefer";
        public const string PAYPAL_HEADER_RETURN = "return=representation";
        public const string PAYPAL_REL_APPROVE = "approve";
        public const string PAYPAL_URL = "payUrl";
        public const string PAYPAL_REFUND_DESCRIPTION = "Hoàn tiền do vì hủy đặt hàng";
        #endregion

        #region MoMo        
        public const string MOMO = "MoMo";
        public const string MOMO_PARTNER_CODE = "MoMo:PartnerCode";
        public const string MOMO_ACCESS_KEY = "MoMo:AccessKey";
        public const string MOMO_SECRET_KEY= "MoMo:Serectkey";
        public const string MOMO_PUBLIC_KEY= "MoMo:PublicKey";
        public const string MOMO_LINK= "MoMo:MoMoLink";
        public const string MOMO_REFUND= "MoMo:MomoRefund";
        public const string MOMO_VERSION = "2.0";
        public const string MOMO_DESCRIPTION_REFUND = "Hoàn tiền do vì hủy đặt hàng";

        // Data HMAC SHA256
        public const string MOMO_SHA_PARTNER_CODE = "partnerCode=";
        public const string MOMO_SHA_ACCESS_KEY = "&accessKey=";
        public const string MOMO_SHA_REQUEST_ID = "&requestId=";
        public const string MOMO_SHA_AMOUNT = "&amount=";
        public const string MOMO_SHA_ORDER_ID = "&orderId=";
        public const string MOMO_SHA_ORDER_INFO = "&orderInfo=";
        public const string MOMO_SHA_RETURN_URL = "&returnUrl=";
        public const string MOMO_SHA_NOTIFY_URL = "&notifyUrl=";
        public const string MOMO_SHA_EXTRA_DATA = "&extraData=";

        // Data Json
        public const string MOMO_JSON_PARTNER_CODE = "partnerCode";
        public const string MOMO_JSON_ACCESS_KEY = "accessKey";
        public const string MOMO_JSON_REQUEST_ID = "requestId";
        public const string MOMO_JSON_AMOUNT = "amount";
        public const string MOMO_JSON_ORDER_ID = "orderId";
        public const string MOMO_JSON_ORDER_INFO = "orderInfo";
        public const string MOMO_JSON_RETURN_URL = "returnUrl";
        public const string MOMO_JSON_NOTIFY_URL = "notifyUrl";
        public const string MOMO_JSON_EXTRA_DATA = "extraData";
        public const string MOMO_JSON_REQUEST_TYPE = "requestType";
        public const string MOMO_JSON_SIGNATURE = "signature";
        public const string MOMO_JSON_CAPTURE_MOMO_WALLET = "captureMoMoWallet";
        #endregion

        #region Type Workflow History
        public const string TYPE_ORDER = "Đặt hàng";
        public const string TYPE_IMPORT_WAREHOUSE = "Nhập kho";
        #endregion

        #region Status
        public const string STATUS_CONFIRMED_PAYMENT = "xác nhận đã thanh toán";
        public const string STATUS_WAITING_CONFIRM = "Chờ xác nhận";
        public const string STATUS_PREPARING_GOODS = "Đang chuẩn bị hàng";
        public const string STATUS_PREPARED_GOODS = "Đã chuẩn bị xong";
        public const string STATUS_WAITING_PICK_GOODS = "Chờ lấy hàng";
        public const string STATUS_WAITING_FOR_DELIVERING = "Chờ giao hàng";
        public const string STATUS_ON_DELIVERY_GOODS = "Đang giao hàng";
        public const string STATUS_RECEIVED_GOODS = "Đã nhận hàng";
        public const string STATUS_WAITING_EVALUATE = "Chờ đánh giá";
        public const string STATUS_EVALUATED = "Đã đánh giá";
        public const string STATUS_CANCELED = "Đã hủy";
        public const string STATUS_PENDING_ADMIN_CANCED_ORDER = "Chờ người quản trị xác nhận hủy đơn hàng";
        public const string STATUS_SEEN_NOTIFICATION = "Đã xem";
        public const string STATUS_NOT_SEEN_NOTIFICATION = "Chưa xem";
        #endregion

        #region Fast Delivery
        public const string FAST_DELIVERY_URL_API = "FastDelivery:UrlAPI";
        public const string FAST_DELIVERY_TOKEN = "FastDelivery:Token";
        public const string FAST_DELIVERY_SHOP_ID = "FastDelivery:ShopId";
        public const string FAST_DELIVERY_SHOP_ID_NAME = "ShopId";
        public const string FAST_DELIVERY_TOKEN_NAME = "Token";

        public const string FAST_DELIVERY_FROM_DISTRICT_ID = "from_district_id";
        public const string FAST_DELIVERY_SERVICE_ID = "service_id";
        public const string FAST_DELIVERY_SERVICE_TYPE_ID = "service_type_id";
        public const string FAST_DELIVERY_TO_DISTRICT_ID = "to_district_id";
        public const string FAST_DELIVERY_TO_WARD_CODE = "to_ward_code";
        public const string FAST_DELIVERY_ITEM_HEIGHT = "height";
        public const string FAST_DELIVERY_ITEM_LENGTH = "length";
        public const string FAST_DELIVERY_ITEM_WEIGHT = "weight";
        public const string FAST_DELIVERY_ITEM_WIDTH = "width";
        public const string FAST_DELIVERY_INSURANCE_FEE = "insurance_fee";
        public const string FAST_DELIVERY_COUPON = "coupon";
        #endregion

        #region Json key 
        public const string JSON_KEY_STATUS = "Status";
        public const string JSON_KEY_COMMENT_ID = "CommentId";
        #endregion

        #region Validation Key
        public const string KEY_IMAGE = "Image";
        #endregion

        #region Receipt Status
        public const string RECEIPT_STATUS_WAITING = "Chờ xử lý";
        public const string RECEIPT_STATUS_PROCESSING = "Đang nhập hàng";
        public const string RECEIPT_STATUS_COMPLETE = "Hoàn thành";
        public const string RECEIPT_REQUEST_STATUS_WAITING = "Chờ xác nhận";
        public const string RECEIPT_REQUEST_STATUS_APPROVED = "Đã duyệt";
        public const string RECEIPT_REQUEST_REJECT = "Từ chối";

        #endregion

        #region Guide Constant
        public const string GUIDE_BUY_PRODUCT = "Hướng dẫn mua hàng";
        public const string GUIDE_PAYMENT = "Hướng dẫn thanh toán";
        #endregion

        #region Sale constant
        public const string TYPE_SALE_FOR_PRODUCT = "Khuyến mãi cho sản phẩm";
        public const string TYPE_SALE_FOR_ORDER = "Khuyến mãi cho đơn hàng";
        #endregion

        #region Notification Type
        public const string NOTIFICATION_ORDER = "Thông báo đặt hàng";
        public const string NOTIFICATION_DELIVERY_ORDER = "Thông báo giao hàng";
        public const string NOTIFICATION_REJECT_ORDER = "Thông báo hủy đơn hàng";
        public const string NOTIFICATION_SALE = "Thông báo khuyến mại";
        public const string NOTIFICATION_IMPORT_WAREHOUSE = "Thông báo nhập kho";
        public const string NOTIFICATION_REQUEST_RECEIPT_IMPORT_WAREHOUSE = "Thông báo yêu cầu nhập kho";
        public const string NOTIFICATION_REJECT_RECEIPT_IMPORT_WAREHOUSE = "Thông báo từ chối đơn nhập kho";
        public const string NOTIFICATION_EXPORT_WAREHOUSE = "Thông báo xuất kho";
        public const string NOTIFICATION_DELETE = "Thông báo xóa";
        #endregion
    }
}
