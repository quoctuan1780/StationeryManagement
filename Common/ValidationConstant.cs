﻿namespace Common
{
    public class ValidationConstant
    {
        public const string ERROR_EMAIL_EMPTY = "Email không được để trống";
        public const string ERROR_EMAIL_TYPE = "Định dạng phải là một email";
        public const string ERROR_PASSWORD_ENPTY = "Mật khẩu không được để trống";
        public const string ERROR_CONFIRMPASSWORD_ENPTY = "Mật khẩu xác nhận không được để trống";
        public const string ERROR_COMPARE_PADDWORD = "Hai mật khẩu không giống nhau";

        public const string ERROR_FULLNAME_EMPTY = "Họ và tên không được để trống";
        public const string ERROR_GENDER_EMPTY = "Giới tính không được để trống";
        public const string ERROR_PHONENUMBER_EMPTY = "Số điện thoại không được để trống";
        public const string ERROR_PHONENUMBER_RANGE = "Số điện thoại phải là 10 hoặc 11 số";
        public const string ERROR_PHONENUMBER_FORMAT = "Định dạng phải là số điện thoại";
       
        public const string ERROR_ADDRESS_EMPTY = "Địa chỉ không được để trống";
        public const string ERROR_FULLNAME_RANGE = "Họ và tên không nhỏ hơn 3 và lớn hơn 200 ký tự";
        public const string ERROR_ADDRESS_RANGE = "Địa chỉ không nhỏ hơn 6 và lớn hơn 300 ký tự";
        public const string ERROR_DISCOUNT_RANGE = "Phần trăm không bé hơn 0 và lớn hơn 100";
        public const string ERROR_FORMAT = "Bạn nhập sai định dạng";

        public const string ERROR_CATEGORY_NAME_EMPTY = "Tên loại sản phẩm không được để trống";
        public const string ERROR_CATEGORY_NAME_RANGE = "Tên loại sản phẩm không được quá 30 ký tự";

        public const string ERROR_PRODUCT_NAME_EMPTY = "Tên sản phẩm không được để trống";
        public const string ERROR_PRODUCT_PRICE_EMPTY = "Giá sản phẩm không được để trống";
        public const string ERROR_PRODUCT_PRICE_RANGE = "Khoảng giá bạn nhập không hợp lệ";
        public const string ERROR_PRODUCT_CAREGORY_EMPTY = "Loại sản phẩm không được để trống";
        public const string ERROR_PRODUCT_QUANTITY = "Số lượng sản phẩm không hợp lệ";

        public const string VALIDATE_NAME = @"^[A-Z]+[a-zA-Z]*$";

        public const string VALIDATION_NON_ANPHABETIC = "[^a-zA-Z0-9_. -]";

        public const string VALIDATION_PHONENUMBER = @"^(\+[0-9]{9})$";

        public const string COMPARE_PASSWORD = "Password";
    }
}
