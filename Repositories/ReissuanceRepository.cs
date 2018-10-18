using Cards.DatabaseLink;
using Cards.Helpers;
using Cards.Models;
using Cards.Models.ViewModels.CustomerViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public class ReissuanceRepository : IReissuanceRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        List<CardIssuance> _cardRequestList;

        public ReissuanceRepository(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<IssuanceDisplayViewModel> GetRequests(string searchString)
        {

            try
            {

               

                if (!String.IsNullOrEmpty(searchString))
                {
                    //var tt = _appDbContext.Customers.Where(cu => cu.PhoneNumber.Contains(searchString)
                    //|| cu.AccountNumber.Contains(searchString)).FirstOrDefault().Id;


                    _cardRequestList = _appDbContext.CardIssuances.Where(c => c.CardStatusId >= 9
                         && c.NameOnCard.Contains(searchString)
                         || c.PAN.Contains(searchString)
                         || c.CustomerId == _appDbContext.Customers.Where(cu => cu.AccountNumber.Contains(searchString)).FirstOrDefault().Id
                         || c.CustomerId == _appDbContext.Customers.Where(cu => cu.PhoneNumber == searchString).FirstOrDefault().Id).ToList();

                    var list = _cardRequestList.Select(item => new IssuanceDisplayViewModel
                    {
                        Id = item.Id,
                        Branch = _appDbContext.Branches.Where(b => b.Id == item.BranchId).Select(b => b.BranchName).FirstOrDefault(),
                        Product = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                        CardStatus = _appDbContext.CardStatuses.Where(c => c.Id == item.CardStatusId).Select(c => c.Status).FirstOrDefault(),
                        PinStatus = _appDbContext.PinStatus.Where(p => p.Id == item.PINStatusId).Select(p => p.Status).FirstOrDefault(),
                        CardVersion = item.CardVersion,
                        NameOnCard = item.NameOnCard,
                        PickUpBranch = _appDbContext.Branches.Where(b => b.Id == item.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                        EntryDate = item.EntryDate,
                        ExpiryDate = item.ExpiryDate,
                        CustomerId = item.CustomerId

                    }).ToList();

                    return list;
                }

                else
                {
                    _cardRequestList = _appDbContext.CardIssuances.Where(c => c.CardStatusId >= 9).ToList();

                    var list = _cardRequestList.Select(item => new IssuanceDisplayViewModel
                    {
                        Id = item.Id,
                        Branch = _appDbContext.Branches.Where(b => b.Id == item.BranchId).Select(b => b.BranchName).FirstOrDefault(),
                        Product = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                        CardStatus = _appDbContext.CardStatuses.Where(c => c.Id == item.CardStatusId).Select(c => c.Status).FirstOrDefault(),
                        PinStatus = _appDbContext.PinStatus.Where(p => p.Id == item.PINStatusId).Select(p => p.Status).FirstOrDefault(),
                        CardVersion = item.CardVersion,
                        NameOnCard = item.NameOnCard,
                        PickUpBranch = _appDbContext.Branches.Where(b => b.Id == item.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                        EntryDate = item.EntryDate,
                        ExpiryDate = item.ExpiryDate,
                        CustomerId = item.CustomerId

                    }).ToList();

                    return list;
                }
            }

            catch(Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }
            return null;
        }

        public string PinReIssuance(int Id)
        {
            string message = string.Empty;
            try
            {
                if (Id > 0)
                {
                    var process = _appDbContext.CardIssuances.Where(c => c.Id == Id && c.PINStatusId >= 3).FirstOrDefault();

                    if (process != null)
                    {

                        process.PINStatusId = 2;
                        process.InitiatedBy = (int)_session.GetInt32("UserId");
                        process.PINVersion ++;
                        process.IsPinRequest = true;
                        process.IsCardRequest = false;
                        process.EntryDate = DateTime.Now;
                        process.ExpiryDate = process.EntryDate.AddYears(3);
                        _appDbContext.SaveChanges();

                        message = "Success!!!";

                    }
                    else
                    {
                        message = "Request already exist!!!";

                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }

            return message;
        }

        public string CardReIssuance(int Id)
        {

            string message = string.Empty;
            try
            {
                if (Id > 0)
                {
                    var process = _appDbContext.CardIssuances.Where(c => c.Id == Id && c.CardStatusId >= 3).FirstOrDefault();

                    if (process != null)
                    {

                        DateTime now = DateTime.Now;
                        TimeSpan span = now.Subtract(process.EntryDate);
                        string timeSpan = string.Empty;

                        if(span.Days < 30)
                        {
                            //Message, Card must be more than 30 days before it can be reissued, CBN rule
                            message = "Primed Cards must be more than 30 days before it can be re-issued";
                            return message;
                        }

                        else
                        {
                            process.CardStatusId = 2;
                            process.PINStatusId = (int)_session.GetInt32("UserId");
                            process.InitiatedBy = 3;
                            process.CardVersion++;
                            process.IsPinRequest = false;
                            process.IsCardRequest = true;
                            process.EntryDate = DateTime.Now;
                            process.ExpiryDate = process.EntryDate.AddYears(3);
                            _appDbContext.SaveChanges();

                            //Message, Card must be more than 30 days before it can be reissued, CBN rule
                            message = "Success!!!";
                            return message;

                        }
                    }

                    else
                    {
                        // Message to state why card is not available for Re-issuance
                        message = "A card request already exist for this customer";
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }

            return message;
        }


    }
}
