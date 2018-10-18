using Cards.Models.ViewModels.CustomerViewModel;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Cards.Helpers
{
    public static class HttpContentExtensions

    {
        public static CustomerViewModel ReadAsJsonAsync<IDictionary>(this HttpContent content)
        {
            CustomerViewModel vm = new CustomerViewModel();
            string json = content.ReadAsStringAsync().Result;
           
            IDictionary<string, string> value = JsonConvert.DeserializeObject<IDictionary<string, string>>(json);
            List<string> list = new List<string>(value.Values);
            if (value != null)
            {

                if(value.Count == 5)
                {
                    vm.responseCode = list[0];
                    vm.tranDate = list[1];
                    vm.postedDate = list[2];
                    vm.postedFlag = list[3];
                    vm.tranId = list[4];

                }

                else
                {

                    vm.accountName = list[0];
                    vm.AccountNumber = list[1];
                    vm.AccountTypeName = list[2];
                    vm.AccountType = list[3];
                    vm.AccountStatus = list[4];
                    vm.Gender = list[5];
                    vm.Birthday = list[6];
                    vm.CustomerId = list[7];
                    vm.CustomerAddress = list[8];
                    vm.CustomerEmail = list[9];
                    vm.CustomerMobile = list[10];
                    vm.Currency = list[11];
                    vm.CurrencyCode = list[12];
                }               
            }

            else
            {
                return null;
            }

            return vm;
        }
    }
}
