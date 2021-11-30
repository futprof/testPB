using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestTaskPb.Services
{
    public interface IDBConfig
    {
        public string ConnectionString { get; set; }
    }
}
