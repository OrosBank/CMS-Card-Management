using Cards.Models.ViewModels.CustomerViewModel;
using PagedList.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Services
{
    public class PageService
    {
        private IList<IssuanceDisplayViewModel> sampleData;

        public PageService()
        {
            this.sampleData = new List<IssuanceDisplayViewModel>();

            for (var i = 1; i <= 500; i++)
            {
                this.sampleData.Add(
                    new IssuanceDisplayViewModel()
                    {
                        NameOnCard = "Test " + i
                    });
            }
        }

        public IPagedList<IssuanceDisplayViewModel> GetTests(int pageNumber, int pageSize)
        {
            var tests = this.sampleData.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new StaticPagedList<IssuanceDisplayViewModel>(tests, pageNumber, pageSize, this.sampleData.Count);
        }
    }
}
