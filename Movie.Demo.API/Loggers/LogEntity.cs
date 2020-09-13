using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Demo.API.Loggers
{
    public class LogEntity
    {
        public string LangCode { get; set; }
        public string Query { get; set; }
        public int? Id { get; set; }
        public int? PageNumber { get; set; }
    }
}
