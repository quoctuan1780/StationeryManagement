namespace Common
{
    public class Constant
    {
        public const string CONNECTION_STRING = "DefaultConnection";

        public const string EMPTY = "";

        public const string DOUBLE_EMPTY = "  ";

        public const string IMAGE_LINK = "wwwroot/images/product/";

        public const string IMAGE_KEY = "imageKey";

        public const string SUCCESS = "Success";

        public const string FAIL = "Fail";

        public const string MISS_VALUE = "Miss";

        public const string UPDATED = "Updated";

        public const string COMMA = ",";

        public const string DOT = ".";

        public const string AREA_ADMIN = "Admin";

        public const double EXCHANGE_RATE_USD = 23070.26;

        public const string CURRENCY_USE = "USD";

        public const string ZERO = "0";

        public const string VN_CODE = "VN";

        public const string VN_NAME = "Việt Nam";

        #region Error Code
        public const int ERROR_CODE_NULL_ID = -2;

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
        #endregion

        public const string ACTION_CONFIRM_EMAIL = "ConfirmEmail";
        public const string CONTROLLER_ACCOUNT = "Account";

        public const string KEY_CONFIRM_EMAIL = "Confirm";
        public const string KEY_CONFIRM_EMAIL_SUCCESS = "ConfirmSuccess";
        public const string KEY_PAYPAL_CLIENT_ID = "PAYPAL_CLIENT_ID";
        public const string KEY_PAYPAL_CLIENT_SECRET = "PAYPAL_CLIENT_SECRET";

        #region PayPal
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
        #endregion

        #region MoMo        
        public const string MOMO_PARTNER_CODE = "MoMo:PartnerCode";
        public const string MOMO_ACCESS_KEY = "MoMo:AccessKey";
        public const string MOMO_SECRET_KEY= "MoMo:Serectkey";
        public const string MOMO_LINK= "MoMo:MoMoLink";

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

        #region Status
        public const string STATUS_WAITING_CONFIRM = "Chờ xác nhận";
        #endregion
    }
}
