using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskPb.Models.DB
{
    public class CashRequest
    {
        public string Id { get; set; }
        public string DepartmentId { get; set; }
        public string ClientId { get; set; }
    }
}
