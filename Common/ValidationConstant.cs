using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class ValidationConstant
    {
        public const string ERROR_FULLNAME_EMPTY = "Họ và tên không được để trống";
        public const string ERROR_ADDRESS_EMPTY = "Địa chỉ không được để trống";
        public const string ERROR_FULLNAME_RANGE = "Họ và tên không nhỏ hơn 3 và lớn hơn 200 ký tự";
        public const string ERROR_ADDRESS_RANGE = "Địa chỉ không nhỏ hơn 6 và lớn hơn 300 ký tự";
        public const string ERROR_DISCOUNT_RANGE = "Phần trăm không bé hơn 0 và lớn hơn 100";
        public const string ERROR_FORMAT = "Bạn nhập sai định dạng";
        

        public const string VALIDATE_NAME = @"^[A-Z]+[a-zA-Z]*$";
    }
}
