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
    public interface IEBankingRepository
    {
        void ExportXml();
        void ExportTxt();

        IEnumerable<IssuanceDisplayViewModel> GetAllCardReqs();
        ProcessorViewModel GetCardRequestById(int RequestId);

        IEnumerable<IssuanceDisplayViewModel> GetCardRequestByProduct(int productId);

        IEnumerable<IssuanceDisplayViewModel> GetCardRequestBySearchString(string search, string branch, string cardStatus, string pinStatus, string fromDate, string toDate);

        IEnumerable<IssuanceDisplayViewModel> GetCardRequestByCardType(int RequestId);

        IssuanceDisplayViewModel ProcessCards(IEnumerable<IssuanceDisplayViewModel> list, int product);

        IssuanceDisplayViewModel GetCardRequestByRequestDate(int RequestId);

        IEnumerable<Branch> GetAllBranches();
        IEnumerable<CardProduct> GetAllCardProducts();
        void AuthorizeSingle(int Id, string authorize, string decline, IFormCollection form);

        IssuanceDisplayViewModel AuthorizeAllCard(IEnumerable<IssuanceDisplayViewModel> list);
        IssuanceDisplayViewModel DeclineAllCard(IEnumerable<IssuanceDisplayViewModel> list);

        void Request(int Id);
        IEnumerable<CardStatus> GetCardStatus();

        IEnumerable<PinStatus> GetPinStatus();

        IEnumerable<IssuanceDisplayViewModel> CardBatch();

        string CardBatchUpdate(int Id, string Returned, string Dispatched);

        //string PanUpload();

    }
}
