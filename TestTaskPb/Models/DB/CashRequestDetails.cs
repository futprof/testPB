using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskPb.Models.DB
{
    public class CashRequestDetails
    {
        public string Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public string CashRequestId { get; set; }
        public double Amount { get; set; }
        public string CurrencyId { get; set; }
        public int StatusId { get; set; }
    }
}
