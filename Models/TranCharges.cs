using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class TranCharges : Base
    {
        public int CardProductId { get; set; }
        public DateTime? LogDate { get; set; }
        public double DebitAmount { get; set; }
        public double CMF_Amt { get; set; }
        //public double VAT_Amt { get; set; }
        public string SourceAccount { get; set; }
        public string DestinationAccountCMF { get; set; }
        //public string DestinationAccountVAT { get; set; }
        public int BranchID { get; set; }
        //public int Version { get; set; }
        public bool PstdFlg { get; set; }
        public DateTime? PstdDate { get; set; }
        public string RspCode { get; set; }
        //public string RspComment { get; set; }
        //public string Naration { get; set; }
        //public int RefNumber { get; set; }
        public int? traceNumber { get; set; }
        //public DateTime? RefTraceDate { get; set; }
        public string CurrencyCode { get; set; }
        //public string TransactionType { get; set; }
        //public string status { get; set; }
        public int NumberOfRetries { get; set; }




    }
}
