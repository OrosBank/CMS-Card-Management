using Cards.DatabaseLink;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class Pin : Base
    {

        public DateTime PINEntryDate { get; set; }
        [ForeignKey ("User")]
        public int PINInitiatedBy { get; set; }        
        public int PINAuthorizedBy { get; set; }
        public DateTime PINAuthorizedDate { get; set; }
        public int PINInitationBranch { get; set; }
        public int PINPickupBranch { get; set; }
        public int PINStatusId { get; set; }
        public int PINReceivedBy { get; set; }
        public DateTime PINReceivedDate { get; set; }
        public int PINDispatchedBy { get; set; }
        public DateTime PINDispatchedDate { get; set; }
        public DateTime BranchPINReceivedDate { get; set; }
        public int BranchPINReceivedBy { get; set; }
        public int PINPickedUpBy { get; set; }
        public string PINLetterAttorney { get; set; }
        public int PINIssuedBy { get; set; }
        public DateTime PINIssuedDate { get; set; }
        public string SignStatus { get; set; }
        [ForeignKey("CardIssuance")]
        public int CardIssuanceId { get; set; }


        public virtual ApplicationUser User { get; set; }
        //public virtual PinStatus PinStatus { get; set; }
        public virtual CardIssuance CardIssuance { get; set; }
    }
}
