using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class FileType : Base
    {
        [DisplayName ("File Type")]
        public string Name { get; set; }
        
    }
}
