using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class PanUploadDetails : Base
    {

        public string guid { get; set; }
        public string itc_id { get; set; }
        public string pan { get; set; }
        public string nameoncard { get; set; }
        public string card_type { get; set; }
        public string acctno1 { get; set; }
        public string acctno2 { get; set; }
    }
}
