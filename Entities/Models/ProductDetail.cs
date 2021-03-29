using System.Collections.Generic;

namespace Entities.Models
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }

        public int ProductId { get; set; }

        public string Origin { get; set; }

        public double Weight { get; set; }

        public int Width { get; set; }
        public int Length { get; set; }
        public int Height { get; set; }

        public string  Color { get; set; }

        public int Quantity { get; set; }

        public virtual IList<OrderDetail> OrderDetails { get; set; }

        public virtual IList<ImportWarehouseDetail> ImportWarehouseDetails { get; set; }

        public virtual Product Product { get; set; }
    }
}
