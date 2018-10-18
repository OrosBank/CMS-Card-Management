using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class AccountDetail : Base
    {
        public string AccountNumber { get; set; }
        public int AccountTypeId { get; set; }
        public string AccountStatus { get; set; }
        public int AccountTypeInTWCMS { get; set; }
        public string AccountExternalNumber { get; set; }
        public string AccountName { get; set; }
        public string LedgerBalance { get; set; }
        public string AvailableBalance { get; set; }
        public string AccountSchemeCode { get; set; }

        
    }
}
