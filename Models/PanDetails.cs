using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class PanDetails : Base
    {
        public string Pan { get; set; }
        public string MaskedPan { get; set; }
        public int? CardIssuanceId { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
