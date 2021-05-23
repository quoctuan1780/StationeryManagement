using System.ComponentModel.DataAnnotations;

namespace FinalProject.ViewModels
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Bạn chưa chọn phương thức thanh toán")]
        public string PaymentMethod { get; set; }

        [Required(ErrorMessage = "Bạn chưa chọn địa chỉ giao hàng")]
        public string DeliveryAddress { get; set; }
        public decimal Total { get; set; }
    }
}
