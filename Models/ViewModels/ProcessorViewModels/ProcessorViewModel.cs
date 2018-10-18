using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models.ViewModels
{
    public class ProcessorViewModel
    {
        [DisplayName("Account Name")]
        public string accountName { get; set; }

        [DisplayName("Account Number")]
        public string AccountNumber { get; set; }

        [DisplayName("Account Type Name")]
        public string AccountTypeName { get; set; }

        [DisplayName("Account Type")]
        public string AccountType { get; set; }
        [DisplayName("Account Status")]
        public string AccountStatus { get; set; }

        [DisplayName("Gender")]
        public string Gender { get; set; }

        [DisplayName("Birthday")]
        public DateTime Birthday { get; set; }

        [DisplayName("Customer Id")]
        public string CustomerId { get; set; }

        [DisplayName("Customer Address")]
        public string CustomerAddress { get; set; }

        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }

        [DisplayName("Customer Mobile")]
        public string CustomerMobile { get; set; }

        [DisplayName("Name On Card")]
        public string NameOnCard { get; set; }

        [DisplayName("Waive Charge")]
        public bool WaiveCharge { get; set; }

        public int Id { get; set; }

        public string Product { get; set; }

        [DisplayName("Card Status")]
        public string CardStatus { get; set; }

        [DisplayName("Pin Status")]
        public string PinStatus { get; set; }

        [DisplayName("Card Version")]
        public int CardVersion { get; set; }

        public string Branch { get; set; }

        [DisplayName("Pick Up Branch")]
        public string PickUpBranch { get; set; }

        [DisplayName("Entry Date")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

    }
}
