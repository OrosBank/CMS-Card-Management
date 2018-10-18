using Cards.DatabaseLink;
using Cards.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class CardIssuance : Base
    {
        [DisplayName("Main Batch")]
        public string MainBatch { get; set; }

        [DisplayName("Branch Batch")]
        public string BranchBatch { get; set; }

        [DisplayName("Product")]
        [Column("CardProductID")]
        public int ProductId { get; set; }

        [DisplayName("Sequence Number")]
        public string SequenceNumber { get; set; }
        
        [DisplayName("Customer")]
        public int CustomerId { get; set; }

        [DisplayName("Card Status")]
        public int CardStatusId { get; set; }

        [DisplayName("Name On Card")]
        public string NameOnCard { get; set; }

        [DisplayName("Pin Status")]
        public int PINStatusId { get; set; }

        [DisplayName("Card Prefix")]
        public string CardPrefix { get; set; }

        [DisplayName("PAN")]
        public string PAN { get; set; }
        //[ForeignKey("Pins")]
        //public int PINId { get; set; }

        [DisplayName("Initiating Branch")]
       // [InverseProperty("InitiatingBranch")]
        public int BranchId { get; set; }

        [DisplayName("Pick Up Branch")]
        //[InverseProperty("PickupBranch")]
        public int PickUpBranchId { get; set; }

        [DisplayName("Entry Date")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Initiated By")]
        public int InitiatedBy { get; set; }

        [DisplayName("Initiator Action")]
        public string InitiatorAction { get; set; }

        [DisplayName("Authorize By")]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public int? AuthorizeBy { get; set; }

        [DisplayName("Authorize Date")]
        public DateTime? AuthorizeDate { get; set; }

        [DisplayName("Processed By")]
        public int? ProcessedBy { get; set; }

        [DisplayName("Processed Date")]
        public DateTime? ProcessedDate { get; set; }

        [DisplayName("Account Documentation")]
        public string AccountDocumentation { get; set; }

        [DisplayName("Currency No")]
        public string CurrencyNo { get; set; }

        [DisplayName("Unique GUID")]
        public string UniqueGUID { get; set; }

        [DisplayName("DefAccount Type")]
        public string DefAccountType { get; set; }

        [DisplayName("Card Stat")]
        public string CardStat { get; set; }

        [DisplayName("Customer start")]
        public string Customerstart { get; set; }

        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [DisplayName("HldRes Code")]
        public string HldResCode { get; set; }

        [DisplayName("Track2Val")]
        public string Track2Val { get; set; }

        [DisplayName("Track2Offset")]
        public string Track2Offset { get; set; }

        [DisplayName("SecurePINLength")]
        public string SecurePINLength { get; set; }

        [DisplayName("SecurePINOffset")]
        public string SecurePINOffset { get; set; }

        [DisplayName("INSecurePINOffset")]
        public string INSecurePINOffset { get; set; }

        [DisplayName("ValDataQue")]
        public string ValDataQue { get; set; }

        [DisplayName("ValData")]
        public string ValData { get; set; }

        [DisplayName("CardHolderResInfo")]
        public string CardHolderResInfo { get; set; }

        [DisplayName("DateActivated")]
        public DateTime? DateActivated { get; set; }

        [DisplayName("IssueRef")]
        public string IssueRef { get; set; }

        [DisplayName("cardAction")]
        public string cardAction { get; set; }

        [DisplayName("IsCard Active")]
        public bool IsCardActive { get; set; }

        [DisplayName("Decline Comment")]
        public string DeclineComment { get; set; }

        [DisplayName("Card Version")]
        public int CardVersion { get; set; }

        [DisplayName("PIN Version")]
        public int PINVersion { get; set; }

        public bool WaiveCharge { get; set; }

        public string Card_PRG { get; set; }
        public string Discre_DATA { get; set; }
        public string Seq_NO { get; set; }

        public bool IsCardRequest { get; set; }

        public bool IsPinRequest { get; set; }

        public int? RecievedFromProcessorBy { get; set; }
        public DateTime? DateRecievedFromProcessor { get; set; }
        public int? SentToBranchBy { get; set; }
        public DateTime? DateSentToBranch { get; set; }
        public int? RecievedAtBranchBy { get; set; }
        public DateTime? DateRecievedAtBranchBy { get; set; }
        public int? ReleasedBy { get; set; }
        public DateTime? DateReleased { get; set; }
        public string Comment { get; set; }



        //public virtual ApplicationUser User { get; set; }
        public ICollection<CardProduct> Product { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CardStatus CardStatus { get; set; }
        public virtual PinStatus PinStatus { get; set; }
        public virtual Branch Branch { get; set; }
        //public virtual Branch PickupBranch { get; set; }


    }
}
