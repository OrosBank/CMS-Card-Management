using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Models.ViewModels.CustomerViewModel
{
    public class CustomerViewModel
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
        public string Birthday { get; set; }

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

        public string Currency { get; set; }
        [DisplayName("Currency Code")]
        public string CurrencyCode { get; set; }

        public string responseCode { get; set; }
        public string tranDate { get; set; }
        public string postedDate { get; set; }
        public string postedFlag { get; set; }
        public string tranId { get; set; }

    }

    public class EditCustomerViewModel
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

        [DisplayName("Branch")]
        public string Branch { get; set; }

        [DisplayName("Pick Up Branch")]
        public string PickUpBranch { get; set; }

        [DisplayName("Waive Charge")]
        public bool WaiveCharge { get; set; }

    }

    public class IssuanceDisplayViewModel
    {
        public int Id { get; set; }


        public string Product { get; set; }

        [DisplayName("Card Status")]
        public string CardStatus { get; set; }

        [DisplayName("Pin Status")]
        public string PinStatus { get; set; }

        [DisplayName("Card Version")]
        public int CardVersion { get; set; }

        [DisplayName("Name On Card")]
        public string NameOnCard { get; set; }

        public string Branch { get; set; }

        [DisplayName("Pick Up Branch")]
        public string PickUpBranch { get; set; }

        [DisplayName("Entry Date")]
        public DateTime EntryDate { get; set; }

        [DisplayName("Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [DisplayName("Customer Id")]
        public int CustomerId { get; set; }

        public string CustomerIdFinacle { get; set; }

        [DisplayName("Waive Charge")]
        public bool WaiveCharge { get; set; }

        public string Card_PRG { get; set; }
        public string Discre_DATA { get; set; }
        public string Seq_NO { get; set; }
        public string AccountNumber { get; set; }
        public string AccType2 { get; set; }
        public string AccountType { get; set; }
        public string Sex { get; set; }
        public string Birthday { get; set; }
        public string Phone { get; set; }
        public string CurrencyCode { get; set; }
        public string CardPrefix { get; set; }
        public string Email { get; set; }
        public string Guid { get; set; }
        public string Pan { get; set; }

        public string Address { get; set; }
        public string FIO { get; set; }
        public string Currency { get; set; }
        public int FileType { get; set; }
        public int CardType { get; set; }
        public string MainBatch { get; set; }
        public string BranchBatch { get; set; }
        public int? RecievedFromProcessorBy { get; set; }
        public DateTime? DateRecievedFromProcessor { get; set; }
        public int? SentToBranchBy { get; set; }
        public DateTime? DateSentToBranch { get; set; }
        public int? RecievedAtBranchBy { get; set; }
        public DateTime? DateRecievedAtBranchBy { get; set; }
        public int? ReleasedBy { get; set; }
        public DateTime? DateReleased{ get; set; }
        public string Comment { get; set; }

        public string UnmaskedPan { get; set; }

        public int BranchId { get; set; }
        public int PickUpBranchId { get; set; }
    }

    public class ListViewmodel
    {
        public IPagedList<IssuanceDisplayViewModel> List { get; set; }
    }

    public class ApplicationRoleListViewModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public int NumberOfUsers { get; set; }
    }

     public class BootstrapModel
    {
        public string ID { get; set; }
        public string AreaLabeledId { get; set; }
        public ModalSize Size { get; set; }
        public string Message { get; set; }
        public string ModalSizeClass
        {
            get
            {
                switch (this.Size)
                {
                    case ModalSize.Small:
                        return "modal-sm";
                    case ModalSize.Large:
                        return "modal-lg";
                    case ModalSize.Medium:
                    default:
                        return "";
                }
            }
        }        
    }

    public enum ModalSize
    {
        Small,
        Large,
        Medium
    }

    public class ApplicationRoleViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
        public string Description { get; set; }
    }

    public class ModalHeader
    {
        public string Heading { get; set; }
    }

    public class ModalFooter
    {
        public string SubmitButtonText { get; set; } = "Submit";
        public string CancelButtonText { get; set; } = "Cancel";
        public string SubmitButtonID { get; set; } = "btn-submit";
        public string CancelButtonID { get; set; } = "btn-cancel";
        public bool OnlyCancelButton { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }

    public class EditUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<SelectListItem> ApplicationRoles { get; set; }
        [Display(Name = "Role")]
        public int ApplicationRoleId { get; set; }
        public int BranchId { get; set; }
    }

    public class ModifyRoleVm
    {
        [Required]
        public string RoleName { get; set; }

        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToRemove { get; set; }

    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public List<SelectListItem> ApplicationRoles { get; set; }
        [Display(Name = "Role")]
        public int ApplicationRoleId { get; set; }
        [Display(Name = "Branch")]
        public int BranchId { get; set; }
    }

    public class UserListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string BranchName { get; set; }
    }

    public class PaginatedList<T> : List<T>
    {

        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static PaginatedList<T> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }
    }

}
