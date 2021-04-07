using System.Formats.Asn1;

namespace Common
{
    public class Constant
    {
        public const string CONNECTION_STRING = "DefaultConnection";

        public const string EMPTY = "";

        public const string IMAGE_LINK = "wwwroot/images/product/";

        public const string IMAGE_KEY = "imageKey";

        public const string SUCCESS = "Success";

        public const string FAIL = "Fail";

        public const string COMMA = ",";

        public const string AREA_ADMIN = "Admin";

        #region Error Code
        public const int ERROR_CODE_NULL_ID = -2;

        public const int ERROR_CODE_NULL = -4;

        public const int ERROR_CODE_CANNOT_FIND_INFOR_BY_ID = -1;

        public const int ERROR_CODE_CONVERT_TO_INT = -3;

        public const int ERROR_CODE_DO_NOT_CONFIRM_EMAIL = 5;
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
    }
}
