﻿using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_ORDER)]
    public class Order
    {
        public int OrderId { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime ReceivedDate { get; set; }
        public DateTime ShipperPickOrderDate { get; set; }
        public DateTime ExportWarehouseDate { get; set; }
        public string Province { get; set; }
        public string Status { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 10)]
        public string Address { get; set; }
        [StringLength(10)]
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string AdminId { get; set; }
        public string WarehouseId { get; set; }
        public string ShipperId { get; set; }
        public string ModifiedBy { get; set; }
        //public virtual Bill Bill { get; set; }
        public virtual MoMoPayment MoMoPayment { get; set; }
        public virtual PayPalPayment PayPalPayment { get; set; }
        public virtual ExportWarehouse ExportWarehouse { get; set; }
        public IList<OrderDetail> OrderDetails { get; set; }
        public DateTime ModifiedDate { get; set; } = DateTime.Now;
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}