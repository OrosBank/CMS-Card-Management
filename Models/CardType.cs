using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class CardType : Base
    {
        [DisplayName("Card Type")]
        public string Name { get; set; }
    }
}
