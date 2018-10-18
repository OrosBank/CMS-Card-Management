using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class PanUpload : Base
    {
        public DateTime upload_date { get; set; }
        public string filename { get; set; }
        public int uploaded_by { get; set; }
    }
}
