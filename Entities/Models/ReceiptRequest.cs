using Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    [Table(TableNameConstant.TABLE_RECEIPT_REQUEST)]
    public class ReceiptRequest
    {
        public int ReceiptRequestId { get; set; }

        public DateTime CreateDate { get; set; }

        public string UserId { get; set; }

        public string Status { get; set; }

        public virtual IList<ReceiptRequestDetail> ReceiptRequestDetails { get; set; }

        public virtual ImportWarehouse ImportWarehouse { get; set; }
    }
}
