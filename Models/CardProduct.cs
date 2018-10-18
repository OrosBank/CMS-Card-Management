using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models
{
    public class CardProduct : Base
    {
        //[Required]
        //[StringLength (20, ErrorMessage ="Product Name is Required")]
        [DisplayName ("Product Name")]
        public string ProductName { get; set; }

        //[Required]
        [DisplayName("Product Code")]
        public string ProductCode { get; set; }

        //[Required]
        [DisplayName("Product Bin")]
        public string  ProductBin { get; set; }

        //[Required]
        [DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        //[Required]       
        [ForeignKey ("CardType")]
        public int CardTypeId { get; set; }

        //[Required]
        [DataType(DataType.Currency, ErrorMessage ="Enter the correct Value")]
        public double Charge { get; set; }

        //[Required]
        [DisplayName("File Type")]
        [ForeignKey("FileType")]
        //[StringLength(3, ErrorMessage = "Product Name is Required")]
        public int FileTypeId { get; set; }

        //[Required]
        [DisplayName("Is Sec Account Required")]
        public bool IsSecAccRequired { get; set; }

        //[Required]
        [DisplayName("Charges Account")]
        public string ChargesAccount { get; set; }

        //[Required]
        [DisplayName("Vat Account")]
        [StringLength(15, ErrorMessage = "Product Name is Required")]
        public string VatAccount { get; set; }

        //[Required]
        [DisplayName("Vat Amount")]
        [DataType(DataType.Currency, ErrorMessage = "Enter the correct Value")]
        public double VatAmount { get; set; }

        //[Required]
        [DisplayName("Created By")]
        public string CreatedBy { get; set; }

        //[Required]
        [DisplayName("Create Date")]
        public DateTime CreateDate { get; set; }


        public virtual CardType CardType { get; set; }
        public virtual FileType FileType { get; set; }

    }
}
