using Cards.Models;
using Cards.Models.ViewModels;
using Cards.Models.ViewModels.CustomerViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public interface ICardRepository
    {

        IEnumerable<IssuanceDisplayViewModel> GetAllCardRequests();

        EditCustomerViewModel GetCardRequestById(int RequestId);
        IEnumerable<IssuanceDisplayViewModel> GetCardRequestBySearch(string SearchString, string branch);

        void AddCustomer(IFormCollection data);

        void AddCardRequest(int Id, IFormCollection data);

        string EditCardRequest(int Id, IFormCollection collection);

        IEnumerable<CardProduct> GetAllCardProducts();

        IEnumerable<Branch> GetAllBranches();

        string VerifyCard(int id, string verify, string decline);

        IEnumerable<IssuanceDisplayViewModel> CardBranchBatch();

        string CardBranchBatchUpdate(int Id, string Returned, string Dispatched);

        string ReleaseCard(int Id);

        string TakeCharge(int Id);
    }
}
