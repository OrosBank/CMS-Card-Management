using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Cards.DatabaseLink;
using Cards.Helpers;
using Cards.Models;
using Cards.Models.ViewModels;
using Cards.Models.ViewModels.CustomerViewModel;
using Microsoft.AspNetCore.Http;

namespace Cards.Repositories
{
    public class CardRepository : ICardRepository
    {
        List<CardIssuance> _cardRequestList;
        List<CardProduct> _cardProductList;
        List<Branch> _branchList;
        public IssuanceDisplayViewModel issuanceDVM = new IssuanceDisplayViewModel();
        private readonly ApplicationDbContext _appDbContext;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;

        public CardRepository(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }


        public void AddCardRequest(int Id, IFormCollection data)
        {
            Activity activity = new Activity();

            Random random = new Random();
            int rand = random.Next(1, 7);

            CardIssuance cardIssuance = new CardIssuance();

            int ProductId = Convert.ToInt16(data["ProductId"]);

            var checkExistingRequest = _appDbContext.CardIssuances.Where(r => r.CustomerId == Id && r.ProductId == ProductId).FirstOrDefault();

            Guid guidId = Guid.NewGuid();

            if (checkExistingRequest != null)
            {
                _session.SetString("RequestExist", "Request For this customer already Exist");
            }

            else
            {

               

                var cards = (from product in _appDbContext.CardProducts
                                join type in _appDbContext.CardTypes on product.CardTypeId equals type.Id
                                where product.CardTypeId == type.Id
                                select new { CardProducts = product, CardTypes = type }).ToList();

                cardIssuance.ProductId = Convert.ToInt16(data["ProductId"]);
                cardIssuance.NameOnCard = data["cus.NameOnCard"];
                cardIssuance.CustomerId = Id;

                var carType = cards.Where(c => c.CardProducts.Id == cardIssuance.ProductId && c.CardTypes.Id == c.CardProducts.CardTypeId).FirstOrDefault();
                var currency = _appDbContext.Customers.Where(c => c.Id == cardIssuance.CustomerId).Select(s => s.Currrency).FirstOrDefault();
                var _product = _appDbContext.CardProducts.Where(p => p.Id == cardIssuance.ProductId).FirstOrDefault();

                if(!_product.CurrencyCode.Equals(currency))
                {
                    _session.SetString("RequestExist", "Account Currency is not the same with Product Currency!!!");
                    _session.SetString("CreatedRequest", null);
                }

                else
                {
                    cardIssuance.EntryDate = DateTime.Now;
                    cardIssuance.InitiatedBy = (int)_session.GetInt32("UserId");
                    cardIssuance.BranchId = Convert.ToInt16(data["BranchId"]);
                    cardIssuance.PickUpBranchId = Convert.ToInt16(data["PickUpBranchId"]);
                    cardIssuance.CardStatusId = 1;
                    cardIssuance.PINStatusId = 1;
                    cardIssuance.IsCardRequest = true;
                    cardIssuance.IsPinRequest = true;
                    cardIssuance.PINVersion = checkExistingRequest == null ? 0 : checkExistingRequest.PINVersion + 1;
                    cardIssuance.CardVersion = checkExistingRequest == null ? 0 : checkExistingRequest.CardVersion + 1;
                    cardIssuance.IsCardActive = false;
                    cardIssuance.UniqueGUID = guidId.ToString();
                    cardIssuance.SecurePINOffset = "L";
                    cardIssuance.SecurePINOffset = "L";
                    cardIssuance.Discre_DATA = "ssppppccc";
                    cardIssuance.Card_PRG = "FBPVERVECPA";
                    cardIssuance.IsCardRequest = true;
                    cardIssuance.ExpiryDate = cardIssuance.EntryDate.AddYears(3);
                    cardIssuance.WaiveCharge = Convert.ToBoolean(data["cus.WaiveCharge"].ToString().Split(',')[0]);

                    _session.SetString("RequestExist", null);

                    switch (carType.CardTypes.Name)
                    {
                        case "Master":


                            if (currency.Equals("NGN"))
                            {
                                cardIssuance.Card_PRG = "FBPMCNAIRA";
                                cardIssuance.Seq_NO = "001";
                                _appDbContext.Add(cardIssuance);
                                _appDbContext.SaveChanges();

                                cardIssuance.PAN = GenPan(cardIssuance.Id, carType.CardProducts.ProductBin, cardIssuance.BranchId);
                                _appDbContext.SaveChanges();

                                //var getUnmaskedPan = _appDbContext.PanDetails.Where(p => p.CardIssuanceId == cardIssuance.Id).FirstOrDefault();
                                //if(getUnmaskedPan != null)
                                //{
                                //    getUnmaskedPan.Pan = Activity.Base64Decode(getUnmaskedPan.Pan);
                                //    _appDbContext.SaveChanges();
                                //}                               
                            }
                            else if (currency.Equals("USD"))
                            {
                                cardIssuance.Card_PRG = "FBPMCUSD";
                                cardIssuance.Seq_NO = "001";
                                _appDbContext.Add(cardIssuance);
                                _appDbContext.SaveChanges();

                                var request = _appDbContext.CardIssuances.Where(c => c.Id == cardIssuance.Id).FirstOrDefault();

                                cardIssuance.PAN = GenPan(request.Id, carType.CardProducts.ProductBin, cardIssuance.BranchId);
                                _appDbContext.SaveChanges();

                                //var getUnmaskedPan = _appDbContext.PanDetails.Where(p => p.CardIssuanceId == cardIssuance.Id).FirstOrDefault();
                                //if (getUnmaskedPan != null)
                                //{
                                //    getUnmaskedPan.Pan = Activity.Base64Decode(getUnmaskedPan.Pan);
                                //    _appDbContext.SaveChanges();
                                //}
                            }

                            break;


                        case "Verve":


                            if (currency.Equals("NGN"))
                            {
                                cardIssuance.Card_PRG = "FBPVERVECPA";
                                cardIssuance.Seq_NO = "001";
                                _appDbContext.Add(cardIssuance);
                                _appDbContext.SaveChanges();

                                var request = _appDbContext.CardIssuances.Where(c => c.Id == cardIssuance.Id).FirstOrDefault();

                                cardIssuance.PAN = GenPan(request.Id, carType.CardProducts.ProductBin, cardIssuance.BranchId);
                                _appDbContext.SaveChanges();

                                //var getUnmaskedPan = _appDbContext.PanDetails.Where(p => p.CardIssuanceId == cardIssuance.Id).FirstOrDefault();
                                //if (getUnmaskedPan != null)
                                //{
                                //    getUnmaskedPan.Pan = Activity.Base64Decode(getUnmaskedPan.Pan);
                                //    _appDbContext.SaveChanges();
                                //}

                            }
                            else if (currency.Equals("USD"))
                            {
                                //cardIssuance.Card_PRG = "FBPVERVECPA";
                                //cardIssuance.Seq_NO = "001";
                                //_appDbContext.Add(cardIssuance);
                                //_appDbContext.SaveChanges();

                                //var request = _appDbContext.CardIssuances.Where(c => c.Id == cardIssuance.Id).FirstOrDefault();

                                //cardIssuance.PAN = GenPan(request.Id, carType.CardProducts.ProductBin, cardIssuance.BranchId);
                                //_appDbContext.SaveChanges();

                            //Return messsage
                            }

                            break;

                        case "Visa":


                            if (currency.Equals("NGN"))
                            {
                                cardIssuance.Card_PRG = null;
                                cardIssuance.Seq_NO = null;
                                _appDbContext.Add(cardIssuance);
                                _appDbContext.SaveChanges();
                            }
                            else if (currency.Equals("USD"))
                            {
                                //cardIssuance.Card_PRG = null;
                                //cardIssuance.Seq_NO = null;
                                //_appDbContext.Add(cardIssuance);
                                //_appDbContext.SaveChanges();

                                //var request = _appDbContext.CardIssuances.Where(c => c.Id == cardIssuance.Id).FirstOrDefault();

                                //cardIssuance.PAN = GenPan(request.Id, carType.CardProducts.ProductBin, cardIssuance.BranchId);
                                //_appDbContext.SaveChanges();

                            // Return message
                            }

                            break;

                    }

                    _session.SetString("CreatedRequest", "Successful Request!!!");
                }

            }

        }

        public string EditCardRequest(int Id, IFormCollection collection)
        {

            string message = string.Empty;

            var updateReq = _appDbContext.CardIssuances.Where(c => c.Id == Id).FirstOrDefault();

            //updateReq.ProductId = Convert.ToInt16(collection["CardProductID"]);
            updateReq.NameOnCard = collection["NameOnCard"];
            updateReq.EntryDate = DateTime.Now;
            updateReq.InitiatedBy = (int)_session.GetInt32("UserId");
            updateReq.BranchId = Convert.ToInt16(collection["Branch"]); //_appDbContext.Branches.Where(b => b.Id == branch).Select(b => b.Id).FirstOrDefault();
            updateReq.PickUpBranchId = Convert.ToInt16(collection["PickUpBranch"]);  //_appDbContext.Branches.Where(b => b.Id == pickUpBranch).Select(b => b.Id).FirstOrDefault();
            updateReq.CardStatusId = 2;
            updateReq.PINStatusId = 2;

            _appDbContext.SaveChanges();

            var updateCus = _appDbContext.Customers.Where(c => c.Id == updateReq.CustomerId).FirstOrDefault();

            updateCus.AccountNumber = collection["AccountNumber"];
            updateCus.AccountTypeName = collection["AccountTypeName"];
            updateCus.AccountType = collection["AccountType"];
            updateCus.Gender = collection["Gender"];
            updateCus.BirthDay = Convert.ToDateTime(collection["BirthDay"]);
            updateCus.CustIdFinacle = collection["CustomerId"];
            updateCus.Address = collection["CustomerAddress"];
            updateCus.Email = collection["CustomerEmail"];
            updateCus.PhoneNumber = collection["CustomerMobile"];
            updateCus.FIO = collection["NameOnCard"];
  

            _appDbContext.SaveChanges();

            return message = "Updated Successfully !!!";
        }

        public IEnumerable<IssuanceDisplayViewModel> GetAllCardRequests()
        {

            _cardRequestList = _appDbContext.CardIssuances.ToList();

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
                CustomerId = item.CustomerId,
                BranchId = item.BranchId,
                PickUpBranchId = item.PickUpBranchId

            }).ToList();

            return list;
        }

        public EditCustomerViewModel GetCardRequestById(int RequestId)
        {
            //int userId = (int)_session.GetInt32("UserId");
            int branchId = (int)_session.GetInt32("BranchId");
            string role = _session.GetString("role");

            CardIssuance result;

            if (role.Equals("Inputer"))
            {
                result = _appDbContext.CardIssuances.Where(c => c.Id == RequestId && c.BranchId == branchId && c.AuthorizeBy == null).FirstOrDefault();

                if (result == null)
                {
                    return null;
                }

            }

            else
            {
                result = _appDbContext.CardIssuances.Where(c => c.Id == RequestId && c.BranchId == branchId && c.CardStatusId <= 3 || c.PINStatusId <= 3).FirstOrDefault();

                if (result == null)
                {
                    return null;
                }

            }



            var cus = _appDbContext.Customers.Where(c => c.Id == result.CustomerId).FirstOrDefault();

            var cardReq = new EditCustomerViewModel
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

        public IEnumerable<CardProduct> GetAllCardProducts()
        {
            _cardProductList = _appDbContext.CardProducts.ToList();
            return _cardProductList;
        }

        public void AddCustomer(IFormCollection data)
        {
            try
            {
                Customer customer = new Customer();
                string Id = data["cus.CustomerId"];

                var checkCustomer = _appDbContext.Customers.Where(c => c.CustIdFinacle == Id).FirstOrDefault();
                if (checkCustomer != null)
                {
                    AddCardRequest(checkCustomer.Id, data);

                }

                else
                {
                    int count = 0;
                    string[] nameSplit = data["cus.AccountName"].ToString().Split(" ");

                    count = nameSplit.Count();
                    if (count > 2 && count == 3)
                    {
                        customer.FirstName = nameSplit[0];
                        customer.MiddleName = nameSplit[1];
                        customer.LastName = nameSplit[2];
                    }

                    else if (count == 2)
                    {
                        customer.FirstName = nameSplit[0];
                        customer.LastName = nameSplit[1];
                    }

                    else if (count == 1)
                    {
                        customer.FirstName = nameSplit[0];
                    }
                    string phone = data["cus.CustomerMobile"];
                    string _phone = "+234" + phone.Substring(1);


                    customer.AccountNumber = data["cus.AccountNumber"];
                    customer.AccountTypeName = data["cus.AccountTypeName"];
                    customer.AccountType = data["cus.AccountType"];
                    customer.Gender = data["cus.Gender"];
                    customer.BirthDay = Convert.ToDateTime(data["cus.BirthDay"]);
                    customer.CustIdFinacle = data["cus.CustomerId"];
                    customer.Address = data["cus.CustomerAddress"];
                    customer.Email = data["cus.CustomerEmail"];
                    customer.PhoneNumber = _phone;
                    customer.FIO = data["cus.NameOnCard"];
                    customer.AccType2 = "10";
                    customer.Currrency = data["cus.currency"]; ;
                    customer.CurrrencyCode = data["cus.currencyCode"]; ;


                    var _customer = _appDbContext.Customers.Add(customer);
                    _appDbContext.SaveChanges();

                    _session.SetString("CustomerCreated", "Successful Request!!!");

                    AddCardRequest(customer.Id, data);

                }
            }

            catch(Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }


        }

        public IEnumerable<Branch> GetAllBranches()
        {
            _branchList = _appDbContext.Branches.ToList();

            return _branchList;
        }

        public string VerifyCard(int Id, string verify, string decline)
        {

            string message = string.Empty;

            var _verify = _appDbContext.CardIssuances.Where(c => c.Id == Id).FirstOrDefault();
       

            if(_verify != null)
            {
                if (!String.IsNullOrEmpty(verify))
                {

                    if(_verify.IsCardRequest == true && _verify.IsPinRequest == false)
                    {

                        _verify.AuthorizeBy = _session.GetInt32("UserId");
                        _verify.AuthorizeDate = DateTime.Now;
                        _verify.CardStatusId = 3;
                       // _verify.PINStatusId = 3;

                        string resp = TakeCharge(Id);
                    }

                    else if(_verify.IsCardRequest == false && _verify.IsPinRequest == true)
                    {

                        _verify.AuthorizeBy = _session.GetInt32("UserId");
                        _verify.AuthorizeDate = DateTime.Now;
                        //_verify.CardStatusId = 3;
                        _verify.PINStatusId = 3;

                       //string resp = TakeCharge(Id);
                    }

                    else
                    {
                        _verify.AuthorizeBy = _session.GetInt32("UserId");
                        _verify.AuthorizeDate = DateTime.Now;
                        _verify.CardStatusId = 3;
                        _verify.PINStatusId = 3;

                        string resp = TakeCharge(Id);
                    }


                    _appDbContext.SaveChanges();

                    return message = "Successfully Verified !!!";

                }

                else if (!String.IsNullOrEmpty(decline))
                {
                    _verify.AuthorizeBy = _session.GetInt32("UserId");
                    _verify.AuthorizeDate = DateTime.Now;
                    _verify.CardStatusId = 4;
                    _verify.PINStatusId = 4;

                    _appDbContext.SaveChanges();

                    return message = "Successfully Verified !!!";
                }
            }
            return message = null;
        }

        public IEnumerable<IssuanceDisplayViewModel> GetCardRequestBySearch(string SearchString, string branch)
        {
            int br = 0;
            
            if(branch == "")
            {
                br = 0;
            }
            else
            {
                br = Convert.ToInt16(branch);
            }

            _cardRequestList = _appDbContext.CardIssuances.Where(c => c.NameOnCard.Contains(SearchString)
                                || c.BranchId == br).ToList();

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
                CustomerId = item.CustomerId,
                BranchId = item.BranchId,
                PickUpBranchId = item.PickUpBranchId

            }).ToList();

            return list;
        }

        public string GenPan(int Id, string bin, int branchId)
        {
            string maskedString = string.Empty;
            try
            {
                var sol = _appDbContext.Branches.Where(b => b.Id == branchId).Select(s => s.Sol).FirstOrDefault();

                if (sol != null)
                {

                    string pan = string.Empty;
                    var _firstDigits = bin + sol + "070";
                    Random random = new Random();
                    int _lastDigits = random.Next(1, 7);

                    var cardNumber = _firstDigits + _lastDigits;

                    var firstDigits = cardNumber.Substring(0, 6);
                    var lastDigits = cardNumber.Substring(cardNumber.Length - 4, 4);

                    var requiredMask = new String('*', cardNumber.Length - firstDigits.Length - lastDigits.Length);

                    maskedString = string.Concat(firstDigits, requiredMask, lastDigits);

                    PanDetails panDetails = new PanDetails()
                    {
                        Pan = Activity.Base64Encode(cardNumber),// cardNumber,
                        MaskedPan = maskedString,
                        CardIssuanceId = Id,
                        EntryDate = DateTime.Now
                    };


                    _appDbContext.PanDetails.Add(panDetails);
                    _appDbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Activity activity = new Activity();

                activity.ErrorLog(ex.StackTrace, ex.Message, ex.Source, ex.InnerException.ToString());
            }


            return maskedString;
        }

        public IEnumerable<IssuanceDisplayViewModel> CardBranchBatch()
        {

            int branchId = (int)_session.GetInt32("BranchId");

            _cardRequestList = _appDbContext.CardIssuances.Where(b => b.BranchId == branchId).GroupBy(c => c.BranchBatch).Select(s => s.First()).Where(c => c.CardStatusId == 8).ToList();

            if (_cardRequestList != null)
            {
                //string[] bBatch = item.MainBatch.Split('_');

                var _list = _cardRequestList.Select(item => new IssuanceDisplayViewModel
                {
                   
                    Id = item.Id,
                    MainBatch = item.BranchBatch,
                    PickUpBranchId = item.PickUpBranchId

                }).ToList();

                return _list;
            }

            return null;
        }

        public string CardBranchBatchUpdate(int Id, string Returned, string Dispatched)
        {
            string feedback = string.Empty;

            try
            {
                if (Id > 0)
                {
                    var id = _appDbContext.CardIssuances.Where(c => c.Id == Id).FirstOrDefault();

                    if (id != null)
                    {
                        var _batch = _appDbContext.CardIssuances.Where(c => c.MainBatch.Equals(id.MainBatch)).ToList();

                        if (_batch != null)
                        {

                            if (!String.IsNullOrEmpty(Returned))
                            {
                                foreach (var b in _batch)
                                {
                                    //b.CardStatusId = 9;
                                    //_appDbContext.SaveChanges();

                                    if (b.IsCardRequest == true && b.IsPinRequest == true)
                                    {
                                        b.CardStatusId = 9;
                                        b.PINStatusId = 9;
                                        b.RecievedAtBranchBy = (int)_session.GetInt32("UserId");
                                        b.DateRecievedAtBranchBy = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Cards Recieved at Branch !!!";
                                    }

                                    else if (b.IsCardRequest == true && b.IsPinRequest == false)
                                    {
                                        b.CardStatusId = 9;
                                        b.RecievedAtBranchBy = (int)_session.GetInt32("UserId");
                                        b.DateRecievedAtBranchBy = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Cards Recieved at Branch !!!";
                                    }

                                    else if (b.IsCardRequest == false && b.IsPinRequest == true)
                                    {
                                        b.PINStatusId = 9;
                                        b.RecievedAtBranchBy = (int)_session.GetInt32("UserId");
                                        b.DateRecievedAtBranchBy = DateTime.Now;
                                        _appDbContext.SaveChanges();

                                        feedback = "Pins Recieved at Branch !!!";
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

            catch (Exception ex)
            {

            }


            return feedback;
        }

        public string ReleaseCard(int Id)
        {
            string feedback = string.Empty;

            if(Id > 0)
            {
                var request = _appDbContext.CardIssuances.Where(c => c.Id == Id).FirstOrDefault();

                if(request != null)
                {

                    if (request.IsCardRequest == true && request.IsPinRequest == true)
                    {
                        request.CardStatusId = 10;
                        request.PINStatusId = 10;
                        request.ReleasedBy = (int)_session.GetInt32("UserId");
                        request.DateReleased = DateTime.Now;
                        _appDbContext.SaveChanges();

                        feedback = "Card Released !!!";
                    }

                    else if (request.IsCardRequest == true && request.IsPinRequest == false)
                    {
                        request.CardStatusId = 10;
                        request.ReleasedBy = (int)_session.GetInt32("UserId");
                        request.DateReleased = DateTime.Now;
                        _appDbContext.SaveChanges();

                        feedback = "Card Released !!!";
                    }

                    else if (request.IsCardRequest == false && request.IsPinRequest == true)
                    {
                        request.PINStatusId = 10;
                        request.ReleasedBy = (int)_session.GetInt32("UserId");
                        request.DateReleased = DateTime.Now;
                        _appDbContext.SaveChanges();

                        feedback = "Pins Released !!!";
                    }

                    else
                    {

                    }
                }
            }


            return feedback;
        }


        public string TakeCharge(int Id)
        {
            CustomerViewModel customers = new CustomerViewModel();
            string resp = string.Empty;
            int tranId = 0;


            string post = string.Empty;
            try
            {
                //var _tran = _appDbContext.CardIssuances.ToList();
                string dateFormat = "yyy-MM-dd";
                var tran = _appDbContext.CardIssuances.Find(Id);
                string open = "{";
                string close = "}";
                

                if (tran != null)
                {

                    var cus = _appDbContext.Customers.Where(c => c.Id == tran.CustomerId).FirstOrDefault();

                    var cardProduct = _appDbContext.CardProducts.Where(p => p.Id == tran.ProductId).FirstOrDefault();

                    TranCharges tcharge = new TranCharges()
                    {
                        CurrencyCode = cardProduct.CurrencyCode,
                        BranchID = tran.BranchId,
                        DebitAmount = cardProduct.Charge,
                        LogDate = DateTime.Now,
                        SourceAccount = cus.AccountNumber,
                        CardProductId = tran.ProductId                        
                        
                    };

                    _appDbContext.TranCharges.Add(tcharge);
                    _appDbContext.SaveChanges();

                    tranId = tcharge.Id;


                    string one = $"\"debitAccount\": \"{cus.AccountNumber}\",";
                    string two = $"\"amount\":" + cardProduct.Charge + ",";
                    string three = $"\"creditAccount\": \"{cardProduct.ChargesAccount}\",";
                    string four = $"\"transactionDate\": \"{tran.EntryDate.ToString(dateFormat)}\"";


                    post = open + one + two + three + four + close;
                }


                using (var client = new HttpClient())
                {
                    string userName = "telcoil";
                    string userPassword = "Unix@123";
                    string usernamePassword = userName + ":" + userPassword;
                    client.BaseAddress = new Uri(@"https://www.orosbank.com/mifos-provider-api-0.0.1-SNAPSHOT/api/accountDebitCredit");
                    client.Timeout = TimeSpan.FromMinutes(3);
                    client.DefaultRequestHeaders.Accept.Clear();
                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var byteArray = Encoding.ASCII.GetBytes(usernamePassword);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage response = client.PostAsync(client.BaseAddress, new StringContent(post, Encoding.UTF8, "application/json")).Result;

                    customers = response.Content.ReadAsJsonAsync<CustomerViewModel>();

                    var updateTran = _appDbContext.TranCharges.Where(t => t.Id == tranId).FirstOrDefault();

                    if (updateTran != null)
                    {
                        updateTran.PstdDate = Convert.ToDateTime(customers.postedDate);
                        updateTran.traceNumber = Convert.ToInt16(customers.tranId);
                        updateTran.RspCode = customers.responseCode;
                        updateTran.PstdFlg = customers.postedFlag == "Y" ? true : false;

                        _appDbContext.SaveChanges();

                    }


                }
            }
            catch (Exception ex)
            {
                //return null;
            }

            return resp;
        }
    }
}
