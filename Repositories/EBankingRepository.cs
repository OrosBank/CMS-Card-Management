using Cards.Controllers;
using Cards.DatabaseLink;
using Cards.Helpers;
using Cards.Models;
using Cards.Models.ViewModels;
using Cards.Models.ViewModels.CustomerViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public class EBankingRepository : IEBankingRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        List<CardIssuance> _cardRequestList;
        List<Branch> _allBranches;
        List<CardProduct> _allProducts;
        List<CardStatus> _cardStatus;
        List<PinStatus> _pinStatus;
        List<IssuanceDisplayViewModel> _cardReqList;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public EBankingRepository(IHttpContextAccessor httpContextAccessor, ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
            //var result = new AccountController().CheckSessionState();
        }
       

        public void ExportTxt()
        {
            throw new NotImplementedException();
        }

        public void ExportXml()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Branch> GetAllBranches()
        {
            _allBranches = _appDbContext.Branches.ToList();

            return _allBranches;
        }

        public IEnumerable<IssuanceDisplayViewModel> GetAllCardReqs()
        {
            _cardRequestList = _appDbContext.CardIssuances.Where(c => c.AuthorizeBy != null && c.CardStatusId == 3 || c.CardStatusId == 3).ToList();

            List<IssuanceDisplayViewModel> _list = new List<IssuanceDisplayViewModel>();

            //if (_cardRequestList.Count > 0)
            //{
                var list = _cardRequestList.Select(item => new IssuanceDisplayViewModel
                {
                    Id = item.Id,
                    Branch = _appDbContext.Branches.Where(b => b.Id == item.BranchId).Select(b => b.Sol).FirstOrDefault(),
                    Product = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                    CardStatus = _appDbContext.CardStatuses.Where(c => c.Id == item.CardStatusId).Select(c => c.Status).FirstOrDefault(),
                    PinStatus = _appDbContext.PinStatus.Where(p => p.Id == item.PINStatusId).Select(p => p.Status).FirstOrDefault(),
                    CardVersion = item.CardVersion,
                    NameOnCard = item.NameOnCard,
                    PickUpBranch = _appDbContext.Branches.Where(b => b.Id == item.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                    EntryDate = item.EntryDate,
                    ExpiryDate = item.ExpiryDate,
                    CustomerId = item.CustomerId,
                    CustomerIdFinacle = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.CustIdFinacle).FirstOrDefault(),
                    AccountNumber = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccountNumber).FirstOrDefault(),
                    AccType2 = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault(),
                    AccountType = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault() == "Savings" ? "11" : "01",
                    Birthday = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.BirthDay.ToString()).FirstOrDefault(),
                    Phone = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.PhoneNumber).FirstOrDefault(),
                    Currency = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Currrency).FirstOrDefault(),
                    CurrencyCode = _appDbContext.Customers.Where(i => i.Id == item.CustomerId).Select(s => s.CurrrencyCode).FirstOrDefault(),
                    CardPrefix = _appDbContext.CardProducts.Where(c => c.Id == item.ProductId).Select(s => s.ProductBin).FirstOrDefault(),
                    Email = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Email).FirstOrDefault(),
                    Guid = item.UniqueGUID,
                    Pan = item.PAN,
                    FIO = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.FIO).FirstOrDefault(),
                    Address = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Address).FirstOrDefault(),
                    Sex = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Gender).FirstOrDefault() == "Male" ? "M" : "F",
                    FileType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.FileTypeId).FirstOrDefault(),
                    CardType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.CardTypeId).FirstOrDefault(),
                    UnmaskedPan = Activity.Base64Decode(_appDbContext.PanDetails.Where(p => p.CardIssuanceId == item.Id).Select(s => s.Pan).FirstOrDefault())

                }).ToList();

                return list;
            //}

            //return null;
        }

        public IEnumerable<IssuanceDisplayViewModel> GetCardRequestByCardType(int RequestId)
        {
            throw new NotImplementedException();
        }

        public ProcessorViewModel GetCardRequestById(int RequestId)
        {
            var result = _appDbContext.CardIssuances.Where(c => c.Id == RequestId).FirstOrDefault();
            var cus = _appDbContext.Customers.Where(c => c.Id == result.CustomerId).FirstOrDefault();

            if (result != null && result != null)
            {
                var cardReq = new ProcessorViewModel
                {
                    accountName = result.NameOnCard,
                    AccountNumber = cus.AccountNumber,
                    AccountTypeName = cus.AccountTypeName,
                    AccountType = result.Customer.AccountType,
                    Gender = cus.Gender,
                    Birthday = cus.BirthDay,
                    CustomerAddress = cus.Address,
                    CustomerId = cus.CustIdFinacle,
                    CustomerEmail = cus.Email,
                    CustomerMobile = cus.PhoneNumber,
                    NameOnCard = result.NameOnCard,
                    Branch = _appDbContext.Branches.Where(b => b.Id == result.BranchId).Select(b => b.BranchName).FirstOrDefault(),
                    PickUpBranch = _appDbContext.Branches.Where(b => b.Id == result.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                };

                return cardReq;
            }
            return null;

        }

        public IEnumerable<IssuanceDisplayViewModel> GetCardRequestByProduct(int productId)
        {
            int pr = 0;

            if (productId > 0)
            {
                pr = Convert.ToInt16(productId);

                _cardRequestList = _appDbContext.CardIssuances.Where(c => c.ProductId == pr && c.CardStatusId == 5 && c.PINStatusId == 5).ToList();

                var list = _cardRequestList.Select(item => new IssuanceDisplayViewModel
                {
                    Id = item.Id,
                    Branch = _appDbContext.Branches.Where(b => b.Id == item.BranchId).Select(b => b.Sol).FirstOrDefault(),
                    Product = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                    CardStatus = _appDbContext.CardStatuses.Where(c => c.Id == item.CardStatusId).Select(c => c.Status).FirstOrDefault(),
                    PinStatus = _appDbContext.PinStatus.Where(p => p.Id == item.PINStatusId).Select(p => p.Status).FirstOrDefault(),
                    CardVersion = item.CardVersion,
                    NameOnCard = item.NameOnCard,
                    PickUpBranch = _appDbContext.Branches.Where(b => b.Id == item.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                    EntryDate = item.EntryDate,
                    ExpiryDate = item.ExpiryDate,
                    CustomerId = item.CustomerId,
                    CustomerIdFinacle = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.CustIdFinacle).FirstOrDefault(),
                    AccountNumber = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccountNumber).FirstOrDefault(),
                    AccType2 = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault(),
                    AccountType = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault() == "Savings" ? "11" : "01",
                    Birthday = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.BirthDay.ToString()).FirstOrDefault(),
                    Phone = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.PhoneNumber).FirstOrDefault(),
                    Currency = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Currrency).FirstOrDefault(),
                    CurrencyCode = _appDbContext.Customers.Where(i => i.Id == item.CustomerId).Select(s => s.CurrrencyCode).FirstOrDefault(),
                    CardPrefix = _appDbContext.CardProducts.Where(c => c.Id == item.ProductId).Select(s => s.ProductBin).FirstOrDefault(),
                    Email = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Email).FirstOrDefault(),
                    Guid = item.UniqueGUID,
                    Pan = item.PAN,
                    FIO = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.FIO).FirstOrDefault(),
                    Address = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Address).FirstOrDefault(),
                    Sex = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Gender).FirstOrDefault() == "Male" ? "M" : "F",
                    FileType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.FileTypeId).FirstOrDefault(),
                    CardType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.CardTypeId).FirstOrDefault(),
                    UnmaskedPan = Activity.Base64Decode(_appDbContext.PanDetails.Where(p => p.CardIssuanceId == item.Id).Select(s => s.Pan).FirstOrDefault())

                }).ToList();

                return list;
            }

            return null;
        }

        public IssuanceDisplayViewModel GetCardRequestByRequestDate(int RequestId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IssuanceDisplayViewModel> GetCardRequestBySearchString(string searchString, string branch, string cardStatus, string pinStatus, string fromDate, string toDate)
        {
            int br = 0;
            int cs = 0;
            int ps = 0;
            DateTime toD = DateTime.MinValue;
            DateTime fromD = DateTime.MinValue;

            if (!String.IsNullOrEmpty(branch) || !String.IsNullOrEmpty(searchString) || !String.IsNullOrEmpty(cardStatus) || !String.IsNullOrEmpty(pinStatus) 
                || !String.IsNullOrEmpty(fromDate) || !String.IsNullOrEmpty(toDate))
            {
                br = Convert.ToInt16(branch);
                cs = Convert.ToInt16(cardStatus);
                ps = Convert.ToInt16(pinStatus);
                toD = Convert.ToDateTime(toDate);
                fromD = Convert.ToDateTime(fromDate);

                //toD = DateTime.ParseExact(fromD, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                //fromD = DateTime.ParseExact(toD, "yyyy-MM-dd hh:mm:ss", CultureInfo.InvariantCulture);
                //Fix date search...

                _cardRequestList = _appDbContext.CardIssuances.Where(c => (c.NameOnCard.Contains(searchString) 
                        || c.BranchId == br
                        || c.CardStatusId == cs
                        || c.PINStatusId == ps
                        ||(c.EntryDate >= fromD && c.EntryDate <= toD)) 
                        && c.CardStatusId == 3 || c.PINStatusId == 3).ToList();

                var list = _cardRequestList.Select(item => new IssuanceDisplayViewModel
                {
                    //Id = item.Id,
                    //Branch = _appDbContext.Branches.Where(b => b.Id == item.BranchId).Select(b => b.BranchName).FirstOrDefault(),
                    //Product = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                    //CardStatus = _appDbContext.CardStatuses.Where(c => c.Id == item.CardStatusId).Select(c => c.Status).FirstOrDefault(),
                    //PinStatus = _appDbContext.PinStatus.Where(p => p.Id == item.PINStatusId).Select(p => p.Status).FirstOrDefault(),
                    //CardVersion = item.CardVersion,
                    //NameOnCard = item.NameOnCard,
                    //PickUpBranch = _appDbContext.Branches.Where(b => b.Id == item.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                    //EntryDate = item.EntryDate,
                    //ExpiryDate = item.ExpiryDate,
                    //CustomerId = item.CustomerId

                    Id = item.Id,
                    Branch = _appDbContext.Branches.Where(b => b.Id == item.BranchId).Select(b => b.Sol).FirstOrDefault(),
                    Product = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                    CardStatus = _appDbContext.CardStatuses.Where(c => c.Id == item.CardStatusId).Select(c => c.Status).FirstOrDefault(),
                    PinStatus = _appDbContext.PinStatus.Where(p => p.Id == item.PINStatusId).Select(p => p.Status).FirstOrDefault(),
                    CardVersion = item.CardVersion,
                    NameOnCard = item.NameOnCard,
                    PickUpBranch = _appDbContext.Branches.Where(b => b.Id == item.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                    EntryDate = item.EntryDate,
                    ExpiryDate = item.ExpiryDate,
                    CustomerId = item.CustomerId,
                    CustomerIdFinacle = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.CustIdFinacle).FirstOrDefault(),
                    AccountNumber = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccountNumber).FirstOrDefault(),
                    AccType2 = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault(),
                    AccountType = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault() == "Savings" ? "11" : "01",
                    Birthday = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.BirthDay.ToString()).FirstOrDefault(),
                    Phone = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.PhoneNumber).FirstOrDefault(),
                    Currency = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Currrency).FirstOrDefault(),
                    CurrencyCode = _appDbContext.Customers.Where(i => i.Id == item.CustomerId).Select(s => s.CurrrencyCode).FirstOrDefault(),
                    CardPrefix = _appDbContext.CardProducts.Where(c => c.Id == item.ProductId).Select(s => s.ProductBin).FirstOrDefault(),
                    Email = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Email).FirstOrDefault(),
                    Guid = item.UniqueGUID,
                    Pan = item.PAN,
                    FIO = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.FIO).FirstOrDefault(),
                    Address = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Address).FirstOrDefault(),
                    Sex = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Gender).FirstOrDefault() == "Male" ? "M" : "F",
                    FileType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.FileTypeId).FirstOrDefault(),
                    CardType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.CardTypeId).FirstOrDefault()

                }).ToList();

                return list;
            }

            return null;
        }

        public IEnumerable<CardStatus> GetCardStatus()
        {
            _cardStatus = _appDbContext.CardStatuses.ToList();

            return _cardStatus;
        }

        public IEnumerable<PinStatus> GetPinStatus()
        {
            _pinStatus = _appDbContext.PinStatus.ToList();

            return _pinStatus;
        }



        public IssuanceDisplayViewModel ProcessCards(IEnumerable<IssuanceDisplayViewModel> list, int product)
        {
            try
            {

                switch (product)
                {
                    case 1:
                        Activity.GenMasterNGNFiles(list);

                        foreach (var item in list)
                        {
                            var statusUpdate = _appDbContext.CardIssuances.Where(c => c.Id == item.Id).FirstOrDefault();

                            if (statusUpdate != null)
                            {
                                statusUpdate.CardStatusId = 6;
                                statusUpdate.PINStatusId = 6;
                                statusUpdate.ProcessedBy = _session.GetInt32("UserId");
                                statusUpdate.ProcessedDate = DateTime.Now;

                                _appDbContext.SaveChanges();
                            }
                        }

                        break;

                    case 2:
                        Activity.GenMasterUSDFiles(list);

                        foreach (var item in list)
                        {
                            var statusUpdate = _appDbContext.CardIssuances.Where(c => c.Id == item.Id).FirstOrDefault();

                            if (statusUpdate != null)
                            {
                                statusUpdate.CardStatusId = 6;
                                statusUpdate.PINStatusId = 6;
                                statusUpdate.ProcessedBy = _session.GetInt32("UserId");
                                statusUpdate.ProcessedDate = DateTime.Now;

                                _appDbContext.SaveChanges();
                            }
                        }
                        break;

                    case 3:
                        Activity.GenVerveFiles(list);

                        foreach (var item in list)
                        {
                            var statusUpdate = _appDbContext.CardIssuances.Where(c => c.Id == item.Id).FirstOrDefault();

                            if (statusUpdate != null)
                            {
                                statusUpdate.CardStatusId = 6;
                                statusUpdate.PINStatusId = 6;
                                statusUpdate.ProcessedBy = _session.GetInt32("UserId");
                                statusUpdate.ProcessedDate = DateTime.Now;

                                _appDbContext.SaveChanges();
                            }
                        }
                        break;

                    case 4:
                        Activity.GenVisaFiles(list);

                        foreach (var item in list)
                        {
                            var statusUpdate = _appDbContext.CardIssuances.Where(c => c.Id == item.Id).FirstOrDefault();

                            if (statusUpdate != null)
                            {
                                statusUpdate.CardStatusId = 6;
                                statusUpdate.PINStatusId = 6;
                                statusUpdate.ProcessedBy = _session.GetInt32("UserId");
                                statusUpdate.ProcessedDate = DateTime.Now;

                                _appDbContext.SaveChanges();
                            }
                        }

                        break;
                }

            }

            catch(Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }

            return null;
        }

        public void AuthorizeSingle(int Id, string authorize, string decline, IFormCollection form)
        {           

            if(Id > 0)
            {
                var process = _appDbContext.CardIssuances.Where(c => c.Id == Id).FirstOrDefault();

                if (process != null)
                {
                    if (authorize != null)
                    {
                        Random random = new Random();
                        string mainRand = random.Next(0, 999999).ToString("D6");

                        Random random1 = new Random();
                        string branRand = random1.Next(0, 99999999).ToString("D6");

                        switch (process.IsCardRequest)
                        {
                            case true:

                                process.CardStatusId = 5;
                                process.PINStatusId = 5;
                                process.ProcessedBy = _session.GetInt32("UserId");
                                process.ProcessedDate = DateTime.Now;
                                process.BranchBatch = branRand + "_" + process.BranchId;
                                process.MainBatch = mainRand + "_" + branRand.ToString();
                                _appDbContext.SaveChanges();

                                break;

                            case false:
                                process.PINStatusId = 5;
                                process.ProcessedBy = _session.GetInt32("UserId");
                                process.ProcessedDate = DateTime.Now;
                                process.BranchBatch = branRand + "_" + process.BranchId;
                                process.MainBatch = mainRand + "_" + branRand.ToString();
                                _appDbContext.SaveChanges();

                                break;

                            default:
                                process.CardStatusId = 5;
                                process.PINStatusId = 5;
                                process.ProcessedBy = _session.GetInt32("UserId");
                                process.ProcessedDate = DateTime.Now;
                                process.BranchBatch = branRand + "_" + process.BranchId;
                                process.MainBatch = mainRand + "_" + branRand.ToString();
                                _appDbContext.SaveChanges();
                                break;
                        }
                        
                    }

                    else if(decline != null)
                    {
                        switch (process.IsCardRequest)
                        {
                            case true:
                                process.CardStatusId = 4;
                                process.PINStatusId = 4;
                                process.ProcessedBy = _session.GetInt32("UserId");
                                process.ProcessedDate = DateTime.Now;
                                _appDbContext.SaveChanges();

                                break;

                            case false:
                                process.PINStatusId = 4;
                                process.ProcessedBy = _session.GetInt32("UserId");
                                process.ProcessedDate = DateTime.Now;
                                _appDbContext.SaveChanges();

                                break;

                            default:
                                process.CardStatusId = 4;
                                process.PINStatusId = 4;
                                process.ProcessedBy = _session.GetInt32("UserId");
                                process.ProcessedDate = DateTime.Now;
                                _appDbContext.SaveChanges();
                                break;
                        }

                    }
                }
            }

     
        }

        public void Request(int Id)
        {
            try
            {
                if (Id > 0)
                {
                    var process = _appDbContext.CardIssuances.Where(c => c.Id == Id).FirstOrDefault();

                    if (process != null)
                    {

                        var getRequest = _appDbContext.CardIssuances.Where(c => c.Id == Id).ToList();

                        if (getRequest != null)
                        {

                            var list = getRequest.Select(item => new IssuanceDisplayViewModel
                            {
                                Id = item.Id,
                                Branch = _appDbContext.Branches.Where(b => b.Id == item.BranchId).Select(b => b.Sol).FirstOrDefault(),
                                Product = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.ProductName).FirstOrDefault(),
                                CardStatus = _appDbContext.CardStatuses.Where(c => c.Id == item.CardStatusId).Select(c => c.Status).FirstOrDefault(),
                                PinStatus = _appDbContext.PinStatus.Where(p => p.Id == item.PINStatusId).Select(p => p.Status).FirstOrDefault(),
                                CardVersion = item.CardVersion,
                                NameOnCard = item.NameOnCard,
                                PickUpBranch = _appDbContext.Branches.Where(b => b.Id == item.PickUpBranchId).Select(b => b.BranchName).FirstOrDefault(),
                                EntryDate = item.EntryDate,
                                ExpiryDate = item.ExpiryDate,
                                CustomerId = item.CustomerId,
                                CustomerIdFinacle = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.CustIdFinacle).FirstOrDefault(),
                                AccountNumber = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccountNumber).FirstOrDefault(),
                                AccType2 = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault(),
                                AccountType = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.AccType2).FirstOrDefault() == "Savings" ? "11" : "01",
                                Birthday = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.BirthDay.ToString()).FirstOrDefault(),
                                Phone = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.PhoneNumber).FirstOrDefault(),
                                Currency = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Currrency).FirstOrDefault(),
                                CurrencyCode = _appDbContext.Customers.Where(i => i.Id == item.CustomerId).Select(s => s.CurrrencyCode).FirstOrDefault(),
                                CardPrefix = _appDbContext.CardProducts.Where(c => c.Id == item.ProductId).Select(s => s.ProductBin).FirstOrDefault(),
                                Email = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Email).FirstOrDefault(),
                                Guid = item.UniqueGUID,
                                Pan = item.PAN,
                                FIO = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.FIO).FirstOrDefault(),
                                Address = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Address).FirstOrDefault(),
                                Sex = _appDbContext.Customers.Where(c => c.Id == item.CustomerId).Select(s => s.Gender).FirstOrDefault() == "Male" ? "M" : "F",
                                FileType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.FileTypeId).FirstOrDefault(),
                                CardType = _appDbContext.CardProducts.Where(p => p.Id == item.ProductId).Select(p => p.CardTypeId).FirstOrDefault()


                            }).ToList();

                            Activity.GenPinFiles(list);
                        }



                        process.CardStatusId = 6;
                        process.PINStatusId = 6;
                        process.ProcessedBy = _session.GetInt32("UserId");
                        process.ProcessedDate = DateTime.Now;
                        _appDbContext.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }
        }

        public IEnumerable<CardProduct> GetAllCardProducts()
        {
            _allProducts = _appDbContext.CardProducts.ToList();

            return _allProducts;
        }

        public IssuanceDisplayViewModel AuthorizeAllCard(IEnumerable<IssuanceDisplayViewModel> list)
        {
            try {

                Random random = new Random();
                string mainRand = random.Next(0, 999999).ToString("D6");

                Random random1 = new Random();
                string branRand = random1.Next(0, 99999999).ToString("D6");

                foreach (var item in list)
                {
                    var statusUpdate = _appDbContext.CardIssuances.Where(c => c.Id == item.Id).FirstOrDefault();

                    if (statusUpdate != null)
                    {
                        statusUpdate.CardStatusId = 5;
                        statusUpdate.PINStatusId = 5;
                        statusUpdate.ProcessedBy = _session.GetInt32("UserId");
                        statusUpdate.ProcessedDate = DateTime.Now;
                        statusUpdate.BranchBatch = branRand + "_" + statusUpdate.BranchId;
                        statusUpdate.MainBatch = mainRand + "_" + branRand.ToString();

                        _appDbContext.SaveChanges();
                    }
                }
            }

            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }

            return null;
        }

        public IssuanceDisplayViewModel DeclineAllCard(IEnumerable<IssuanceDisplayViewModel> list)
        {
            try
            {

                foreach (var item in list)
                {
                    var statusUpdate = _appDbContext.CardIssuances.Where(c => c.Id == item.Id).FirstOrDefault();

                    if (statusUpdate != null)
                    {
                        statusUpdate.CardStatusId = 4;
                        statusUpdate.PINStatusId = 4;
                        statusUpdate.ProcessedBy = _session.GetInt32("UserId");
                        statusUpdate.ProcessedDate = DateTime.Now;

                        _appDbContext.SaveChanges();
                    }
                }
            }

            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }

            return null;
        }

        public IEnumerable<IssuanceDisplayViewModel> CardBatch()
        {

            _cardRequestList = _appDbContext.CardIssuances.GroupBy(c => c.MainBatch).Select(s => s.First()).Where(c => c.CardStatusId == 6 || c.CardStatusId == 7).ToList();

            if (_cardRequestList != null)
            {
                var _list = _cardRequestList.Select(item => new IssuanceDisplayViewModel
                {
                    Id = item.Id,
                    MainBatch = item.MainBatch

                }).ToList();

                return _list;
            }

            return null;
        }

        public string CardBatchUpdate(int Id, string Returned, string Dispatched)
        {
            string feedback = string.Empty;

            try
            {
                if(Id > 0)
                {
                    var id = _appDbContext.CardIssuances.Where(c => c.Id == Id).FirstOrDefault();

                    if (id != null)
                    {
                        var _batch = _appDbContext.CardIssuances.Where(c => c.MainBatch.Equals(id.MainBatch)).ToList();

                        if(_batch != null)
                        {

                            if (!String.IsNullOrEmpty(Returned))
                            {
                                foreach (var b in _batch)
                                {
                                    if(b.IsCardRequest == true && b.IsPinRequest == true)
                                    {
                                        b.CardStatusId = 7;
                                        b.PINStatusId = 7;
                                        b.RecievedFromProcessorBy = (int)_session.GetInt32("UserId");
                                        b.DateRecievedFromProcessor = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Requests Returned from Processor Successfully !!!";
                                    }

                                    else if(b.IsCardRequest == true && b.IsPinRequest == false)
                                    {
                                        b.CardStatusId = 7;
                                        b.RecievedFromProcessorBy = (int)_session.GetInt32("UserId");
                                        b.DateRecievedFromProcessor = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Cards Returned from Processor Successfully !!!";
                                    }

                                    else if (b.IsCardRequest == false && b.IsPinRequest == true)
                                    {
                                        b.PINStatusId = 7;
                                        b.RecievedFromProcessorBy = (int)_session.GetInt32("UserId");
                                        b.DateRecievedFromProcessor = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Pins Returned from Processor Successfully !!!";
                                    }

                                    else
                                    {

                                    }
                                }

                                
                            }

                            else if (!String.IsNullOrEmpty(Dispatched))
                            {
                                foreach (var b in _batch)
                                {
                                    b.CardStatusId = 8;
                                    _appDbContext.SaveChanges();

                                    if (b.IsCardRequest == true && b.IsPinRequest == true)
                                    {
                                        b.CardStatusId = 8;
                                        b.PINStatusId = 8;
                                        b.SentToBranchBy = (int)_session.GetInt32("UserId");
                                        b.DateSentToBranch = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Requests Dispatched to Branches Successfully !!!";
                                    }

                                    else if (b.IsCardRequest == true && b.IsPinRequest == false)
                                    {
                                        b.CardStatusId = 8;
                                        b.SentToBranchBy = (int)_session.GetInt32("UserId");
                                        b.DateSentToBranch = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Cards Dispatched to Branches Successfully !!! !!!";
                                    }

                                    else if (b.IsCardRequest == false && b.IsPinRequest == true)
                                    {
                                        b.PINStatusId = 8;
                                        b.SentToBranchBy = (int)_session.GetInt32("UserId");
                                        b.DateSentToBranch = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Pins Dispatched to Branches Successfully !!!";
                                    }

                                    else
                                    {

                                    }
                                }

                            }

                            else
                            {
                                feedback = "Something Went wrong";
                            }
                        }
                    }
                }

            }

            catch(Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }


            return feedback;
        }
    }
}
