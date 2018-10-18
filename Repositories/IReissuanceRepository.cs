using Cards.Models.ViewModels.CustomerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public interface IReissuanceRepository
    {
        IEnumerable<IssuanceDisplayViewModel> GetRequests(string searchString);

        string PinReIssuance(int Id);
        string CardReIssuance(int Id);


    }
}
