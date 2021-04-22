﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    class OrderData
    {
        public string Appid { get; set; }
        public string Apptransid { get; set; }
        public long Apptime { get; set; }
        public string Appuser { get; set; }
        public string Item { get; set; }
        public string Embeddata { get; set; }
        public long Amount { get; set; }
        public string Description { get; set; }
        public string Bankcode { get; set; }
        public string Mac { get; set; }
    }
}
