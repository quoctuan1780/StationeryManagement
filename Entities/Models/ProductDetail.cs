﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ProductDetail
    {
        public int ProductDetailId { get; set; }

        public int ProductId { get; set; }

        public string Origin { get; set; }

        public double Weight { get; set; }

        public string  Color { get; set; }

        public int Quantity { get; set; } = 0;

        public int RemainingQuantity { get; set; } = 0;

        public int QuantityOrdered { get; set; } = 0;

        public decimal Price { get; set; } = 0;
        public decimal SalePrice { get; set; } = 0;

        public virtual IList<OrderDetail> OrderDetails { get; set; }

        public virtual IList<BillDetail> BillDetails { get; set; }

        public virtual IList<ImportWarehouseDetail> ImportWarehouseDetails { get; set; }
        public virtual Product Product { get; set; }
        public virtual IList<RecommendationDetail> RecommendationDetails { get; set; }
        public virtual IList<CartItem> CartItems { get; set; }

        public virtual IList<ReceiptRequestDetail> ReceiptRequestDetails { get; set; }

        public bool IsDeleted { get; set; } = false;
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
