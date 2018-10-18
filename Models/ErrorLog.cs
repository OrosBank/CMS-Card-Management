using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class ErrorLog : Base
    {
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public DateTime ErrorDate { get; set; }
        public string User { get; set; }

        public string InnerException { get; set; }
    }
}
