using Cards.DatabaseLink;
using Cards.Models;
using Cards.Models.ViewModels.CustomerViewModel;
using Cards.Repositories;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static Cards.Helpers.Helper;

namespace Cards.Helpers
{
    public class Activity
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private readonly ApplicationDbContext _appDbContext;

        public Activity(IHttpContextAccessor httpContextAccessor, ApplicationDbContext appDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _appDbContext = appDbContext;
        }

        public Activity()
        {
        }

        public void GetAccess()
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader("Access.txt"))
                {
                    // Read the stream to a string, and write the string to the console.
                    String line = sr.ReadToEnd();

                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }

        public static CustomerViewModel GetCustomer(string Id)
        {
            CustomerViewModel customers = new CustomerViewModel();
            try
            {

                using (var client = new HttpClient())
                {
                    string userName = "telcoil";
                    string userPassword = "Unix@123";
                    string usernamePassword = userName + ":" + userPassword;
                    client.BaseAddress = new Uri(@"https://www.orosbank.com/mifos-provider-api-0.0.1-SNAPSHOT/api/customer/");
                    client.Timeout = TimeSpan.FromMinutes(3);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var byteArray = Encoding.ASCII.GetBytes(usernamePassword);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                    HttpResponseMessage response = client.GetAsync(Id).Result;

                    customers = response.Content.ReadAsJsonAsync<CustomerViewModel>();
                    //if (customers == null)
                    //{
                    //    return null;
                    //}

                }
            }
            catch (Exception ex)
            {
                return null;
            }
            return customers;
        }

        public static void GenMasterNGNFiles(IEnumerable<IssuanceDisplayViewModel> list)
        {
            // Specify the directory you want to manipulate.
            DateTime dt = DateTime.Now;
            string format = "dd-MM-yyyy";
            string path = @"c:\MsterCard_" + "NGN" + dt.ToString(format);

            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);


                    string _path = path + "\\accountbalances.txt";
                    if (!File.Exists(_path))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(_path))
                        {

                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    // Try to create the directory.
                    DirectoryInfo d1 = Directory.CreateDirectory(path);

                    string path1 = path + "\\accountoverridelimits.txt";
                    if (!File.Exists(path1))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path1))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d2 = Directory.CreateDirectory(path);
                    string path2 = path + "\\acounts.txt";
                    if (!File.Exists(path2))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path2))
                        {
                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CurrencyCode);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CustomerIdFinacle);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d3 = Directory.CreateDirectory(path);
                    string path3 = path + "\\cardaccounts.txt";
                    if (!File.Exists(path3))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path3))
                        {
                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                            foreach (var item in list)
                            {                            

                                stringBuilder.Append(item.Pan);
                                stringBuilder.Append(",");
                                stringBuilder.Append("001");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append("1");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Guid);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    DirectoryInfo d4 = Directory.CreateDirectory(path);
                    string path4 = path + "\\cardoverridelimits.txt";
                    if (!File.Exists(path4))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path4))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    DirectoryInfo d5 = Directory.CreateDirectory(path);
                    string path5 = path + "\\cards.txt";
                    if (!File.Exists(path5))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path5))
                        {
                            foreach (var item in list)
                            {
                                //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                string[] birth = item.Birthday.Split(' ');
                                string[] month = birth[0].Split('-');

                                stringBuilder.Append(item.Pan);
                                stringBuilder.Append(",");
                                stringBuilder.Append("001");
                                stringBuilder.Append(",");
                                stringBuilder.Append("FBPMCNAIRA");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append("6");
                                stringBuilder.Append(",");
                                stringBuilder.Append("1902");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("4");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.NameOnCard);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.FIO);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Address);
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("234");
                                stringBuilder.Append(",");
                                stringBuilder.Append("NGA");
                                stringBuilder.Append(",");
                                stringBuilder.Append(month[0] + month[1] + month[2]);
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append("ssppppccc");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.EntryDate);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.EntryDate);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Branch);//Branch Id
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CustomerIdFinacle);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Guid);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d6 = Directory.CreateDirectory(path);
                    string path6 = path + "\\statements.txt";
                    if (!File.Exists(path6))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path6))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }

                }

            }

            catch (Exception ex)
            {

            }
        }

        public static void GenMasterUSDFiles(IEnumerable<IssuanceDisplayViewModel> list)
        {
            // Specify the directory you want to manipulate.
            DateTime dt = DateTime.Now;
            string format = "dd-MM-yyyy";
            string path = @"c:\MsterCard_" + "USD" + dt.ToString(format);

            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);


                    string _path = path + "\\accountbalances.txt";
                    if (!File.Exists(_path))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(_path))
                        {

                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    // Try to create the directory.
                    DirectoryInfo d1 = Directory.CreateDirectory(path);

                    string path1 = path + "\\accountoverridelimits.txt";
                    if (!File.Exists(path1))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path1))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d2 = Directory.CreateDirectory(path);
                    string path2 = path + "\\acounts.txt";
                    if (!File.Exists(path2))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path2))
                        {
                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CurrencyCode);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CustomerIdFinacle);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d3 = Directory.CreateDirectory(path);
                    string path3 = path + "\\cardaccounts.txt";
                    if (!File.Exists(path3))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path3))
                        {
                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.Pan);
                                stringBuilder.Append(",");
                                stringBuilder.Append("001");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append("1");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Guid);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    DirectoryInfo d4 = Directory.CreateDirectory(path);
                    string path4 = path + "\\cardoverridelimits.txt";
                    if (!File.Exists(path4))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path4))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    DirectoryInfo d5 = Directory.CreateDirectory(path);
                    string path5 = path + "\\cards.txt";
                    if (!File.Exists(path5))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path5))
                        {
                            foreach (var item in list)
                            {
                                //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                string[] birth = item.Birthday.Split(' ');
                                string[] month = birth[0].Split('-');

                                stringBuilder.Append(item.Pan);
                                stringBuilder.Append(",");
                                stringBuilder.Append("001");
                                stringBuilder.Append(",");
                                stringBuilder.Append("FBPMCUSD");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append("6");
                                stringBuilder.Append(",");
                                stringBuilder.Append("1902");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("4");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.NameOnCard);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.FIO);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Address);
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("234");
                                stringBuilder.Append(",");
                                stringBuilder.Append("NGA");
                                stringBuilder.Append(",");
                                stringBuilder.Append(month[0] + month[1] + month[2]);
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append("ssppppccc");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.EntryDate);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.EntryDate);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Branch);//Branch Id
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CustomerIdFinacle);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Guid);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d6 = Directory.CreateDirectory(path);
                    string path6 = path + "\\statements.txt";
                    if (!File.Exists(path6))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path6))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }

                }

            }

            catch (Exception ex)
            {

            }
        }

        public static void GenVerveFiles(IEnumerable<IssuanceDisplayViewModel> list)
        {
            // Specify the directory you want to manipulate.
            DateTime dt = DateTime.Now;
            string format = "dd-MM-yyyy";
            string path = @"c:\VerveCard_" + "NGN" + dt.ToString(format);

            try
            {
                // Determine whether the directory exists.
                if (!Directory.Exists(path))
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);


                    string _path = path + "\\accountbalances.txt";
                    if (!File.Exists(_path))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(_path))
                        {

                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    // Try to create the directory.
                    DirectoryInfo d1 = Directory.CreateDirectory(path);

                    string path1 = path + "\\accountoverridelimits.txt";
                    if (!File.Exists(path1))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path1))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d2 = Directory.CreateDirectory(path);
                    string path2 = path + "\\acounts.txt";
                    if (!File.Exists(path2))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path2))
                        {
                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CurrencyCode);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CustomerIdFinacle);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d3 = Directory.CreateDirectory(path);
                    string path3 = path + "\\cardaccounts.txt";
                    if (!File.Exists(path3))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path3))
                        {
                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                            foreach (var item in list)
                            {
                                stringBuilder.Append(item.Pan);
                                stringBuilder.Append(",");
                                stringBuilder.Append("001");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccountNumber);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append("1");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Guid);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    DirectoryInfo d4 = Directory.CreateDirectory(path);
                    string path4 = path + "\\cardoverridelimits.txt";
                    if (!File.Exists(path4))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path4))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }


                    DirectoryInfo d5 = Directory.CreateDirectory(path);
                    string path5 = path + "\\cards.txt";
                    if (!File.Exists(path5))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path5))
                        {
                            foreach (var item in list)
                            {
                                //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                string[] birth = item.Birthday.Split(' ');
                                string[] month = birth[0].Split('-');

                                stringBuilder.Append(item.Pan);
                                stringBuilder.Append(",");
                                stringBuilder.Append("001");
                                stringBuilder.Append(",");
                                stringBuilder.Append("FBPVNAIRA");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.AccType2);
                                stringBuilder.Append(",");
                                stringBuilder.Append("6");
                                stringBuilder.Append(",");
                                stringBuilder.Append("1902");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("4");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.NameOnCard);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.FIO);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Address);
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("234");
                                stringBuilder.Append(",");
                                stringBuilder.Append("NGA");
                                stringBuilder.Append(",");
                                stringBuilder.Append(month[0] + month[1] + month[2]);
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append(",");
                                stringBuilder.Append("0");
                                stringBuilder.Append(",");
                                stringBuilder.Append("ssppppccc");
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.EntryDate);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.EntryDate);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Branch);//Branch Id
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.CustomerIdFinacle);
                                stringBuilder.Append(",");
                                stringBuilder.Append(item.Guid);
                                stringBuilder.Append(Environment.NewLine);
                            }

                            sw.WriteLine(stringBuilder);

                        }
                    }

                    DirectoryInfo d6 = Directory.CreateDirectory(path);
                    string path6 = path + "\\statements.txt";
                    if (!File.Exists(path6))
                    {

                        StringBuilder stringBuilder = new StringBuilder();
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path6))
                        {
                            stringBuilder.Append("");

                            sw.WriteLine(stringBuilder);

                        }
                    }

                }

            }

            catch (Exception ex)
            {

            }
        }

        public static void GenVisaFiles(IEnumerable<IssuanceDisplayViewModel> list)
        {
            DateTime dt = DateTime.Now;
            string format = "dd-MM-yyyy";
            // Build the document
            string vpath = @"c:\VisaCardIssuance" + "NGN" + dt.ToString(format);

                if (!Directory.Exists(vpath))
                {
                    DirectoryInfo di = Directory.CreateDirectory(vpath);

                    XDocument xdoc = new XDocument(
                    new XDeclaration("1.0", "cp866", ""),
                    // This is the root of the document
                    new XElement("Root",
                     list.Select(_item =>
                    new XElement("Record",
                        new XElement("FIO", _item.FIO),
                        new XElement("ACCOUNT", _item.AccountNumber),
                        new XElement("EXTACCOUNT", _item.AccountNumber),
                        new XElement("ACCOUNTTP", _item.AccType2),
                        new XElement("ACCTTYPE", _item.AccountType),
                        new XElement("ACCTSTAT", "3"),
                        new XElement("PAN", ""),
                        new XElement("SEX", _item.Sex),
                        new XElement("BIRTHDAY", _item.Birthday),
                        new XElement("PASNOM", _item.CustomerIdFinacle),
                        new XElement("ADRESS", _item.Address),
                        new XElement("PHONE", _item.Phone),
                        new XElement("COMPANY", ""),
                        new XElement("CEH", ""),
                        new XElement("TABNOM", "36063"),
                        new XElement("LATFIO", ""),
                        new XElement("BIRTHFIO", ""),
                        new XElement("BIRTHPLACE", ""),
                        new XElement("FAMILY", "2"),
                        new XElement("SALARY", ""),
                        new XElement("JOBPHONE", ""),
                        new XElement("CORADDRESS", _item.Address),
                        new XElement("EMAIL", _item.Email),
                        new XElement("FAX", ""),
                        new XElement("INN", ""),
                        new XElement("RESIDENT", ""),
                        new XElement("RESADDRESS", ""),
                        new XElement("EDUCATION", ""),
                        new XElement("STARTJOB", ""),
                        new XElement("STARTBANK", ""),
                        new XElement("PASPLACE", "VNP"),
                        new XElement("COUNTRYRES", ""),
                        new XElement("CMSPROFILE", ""),
                        new XElement("BRPART", _item.Branch),
                        new XElement("NAMEONCARD", _item.NameOnCard),
                        new XElement("CURRENCYNO", _item.CurrencyCode),
                        new XElement("CARDPREFIX", _item.CardPrefix),
                        new XElement("ACCOUNTS",
                        new XElement("ACCOUNT", new XAttribute("Index", "0")))))));

                    // Write the document to the file system            
                    xdoc.Save(vpath + $"\\VisaCard_{dt.ToString(format)}.xml");
                }
        }

        public static void GenFiles(IEnumerable<IssuanceDisplayViewModel> list)
        {
            // Specify the directory you want to manipulate.
            DateTime dt = DateTime.Now;
            string format = "dd-MM-yyyy";
            //string path = @"c:\MsterCard_" + "NGN" + dt.ToString(format);


            try
            {

                var pList = list as List<IssuanceDisplayViewModel>;
                var fList = pList.OrderBy(o => o.CardType).ToList();

                foreach (var item in fList)
                {

                    switch (item.CardPrefix)
                    {
                        case "565432":
                            string path = @"c:\MsterCard_" + item.Currency + dt.ToString(format);

                            if (item.FileType == 1)
                            {
                                // Determine whether the directory exists.
                                if (!Directory.Exists(path))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(path);


                                    string _path = path + "\\accountbalances.txt";
                                    if (!File.Exists(_path))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(_path))
                                        {

                                            //foreach (var listItem in fList)
                                            //{
                                            stringBuilder.Append(item.AccountNumber);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(Environment.NewLine);
                                            //}

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    // Try to create the directory.
                                    DirectoryInfo d1 = Directory.CreateDirectory(path);

                                    string path1 = path + "\\accountoverridelimits.txt";
                                    if (!File.Exists(path1))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path1))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d2 = Directory.CreateDirectory(path);
                                    string path2 = path + "\\acounts.txt";
                                    if (!File.Exists(path2))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path2))
                                        {
                                            //foreach (var listItem in list)
                                            //{
                                            stringBuilder.Append(item.AccountNumber);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.CurrencyCode);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.CustomerIdFinacle);
                                            stringBuilder.Append(Environment.NewLine);
                                            //}

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d3 = Directory.CreateDirectory(path);
                                    string path3 = path + "\\cardaccounts.txt";
                                    if (!File.Exists(path3))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path3))
                                        {
                                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                                            //foreach (var listItem in list)
                                            //{
                                            stringBuilder.Append(item.Pan);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("001");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccountNumber);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("1");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Guid);
                                            stringBuilder.Append(Environment.NewLine);
                                            // }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d4 = Directory.CreateDirectory(path);
                                    string path4 = path + "\\cardoverridelimits.txt";
                                    if (!File.Exists(path4))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path4))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d5 = Directory.CreateDirectory(path);
                                    string path5 = path + "\\cards.txt";
                                    if (!File.Exists(path5))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path5))
                                        {
                                            //foreach (var listItem in list)
                                            //{
                                            //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                            string[] birth = item.Birthday.Split(' ');
                                            string[] month = birth[0].Split('-');

                                            stringBuilder.Append(item.Pan);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("001");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("FBPMCNAIRA");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("6");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("1902");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("4");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.NameOnCard);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.FIO);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Address);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("234");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("NGA");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(month[0] + month[1] + month[2]);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("ssppppccc");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.EntryDate);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.EntryDate);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Branch);//Branch Id
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.CustomerIdFinacle);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Guid);
                                            stringBuilder.Append(Environment.NewLine);
                                            //}

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d6 = Directory.CreateDirectory(path);
                                    string path6 = path + "\\statements.txt";
                                    if (!File.Exists(path6))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path6))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                }
                            }

                            else
                            {

                            }

                            break;

                        case "523421":

                            string vervePath = @"c:\VerveCard_" + item.Currency + dt.ToString(format);

                            if (item.FileType == 1)
                            {
                                // Determine whether the directory exists.
                                if (!Directory.Exists(vervePath))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(vervePath);


                                    string _path = vervePath + "\\accountbalances.txt";
                                    if (!File.Exists(_path))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(_path))
                                        {

                                            //foreach (var listItem in list)
                                            //{
                                            stringBuilder.Append(item.AccountNumber);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(Environment.NewLine);
                                            // }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    // Try to create the directory.
                                    DirectoryInfo d1 = Directory.CreateDirectory(vervePath);

                                    string path1 = vervePath + "\\accountoverridelimits.txt";
                                    if (!File.Exists(path1))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path1))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d2 = Directory.CreateDirectory(vervePath);
                                    string path2 = vervePath + "\\acounts.txt";
                                    if (!File.Exists(path2))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path2))
                                        {
                                            //foreach (var listItem in list)
                                            //{
                                            stringBuilder.Append(item.AccountNumber);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.CurrencyCode);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.CustomerIdFinacle);
                                            stringBuilder.Append(Environment.NewLine);
                                            //}

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d3 = Directory.CreateDirectory(vervePath);
                                    string path3 = vervePath + "\\cardaccounts.txt";
                                    if (!File.Exists(path3))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path3))
                                        {
                                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                                            //foreach (var listItem in list)
                                            //{
                                            stringBuilder.Append(item.Pan);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("001");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccountNumber);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("1");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Guid);
                                            stringBuilder.Append(Environment.NewLine);
                                            //}

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d4 = Directory.CreateDirectory(vervePath);
                                    string path4 = vervePath + "\\cardoverridelimits.txt";
                                    if (!File.Exists(path4))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path4))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d5 = Directory.CreateDirectory(vervePath);
                                    string path5 = vervePath + "\\cards.txt";
                                    if (!File.Exists(path5))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path5))
                                        {
                                            //foreach (var listItem in list)
                                            //{
                                            //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                            string[] birth = item.Birthday.Split(' ');
                                            string[] month = birth[0].Split('-');

                                            stringBuilder.Append(item.Pan);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("001");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("FBPMCNAIRA");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.AccType2);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("6");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("1902");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("4");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.NameOnCard);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.FIO);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Address);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("234");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("NGA");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(month[0] + month[1] + month[2]);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("0");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append("ssppppccc");
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.EntryDate);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.EntryDate);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Branch);//Branch Id
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.CustomerIdFinacle);
                                            stringBuilder.Append(",");
                                            stringBuilder.Append(item.Guid);
                                            stringBuilder.Append(Environment.NewLine);
                                            //}

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d6 = Directory.CreateDirectory(vervePath);
                                    string path6 = vervePath + "\\statements.txt";
                                    if (!File.Exists(path6))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path6))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                }
                            }

                            else
                            {

                            }

                            break;

                        case "465432":

                            if (item.FileType == 1)
                            {
                                //string visaPath = @"c:\VisaTxtCard_" + item.Currency + dt.ToString(format);

                                //// Determine whether the directory exists.
                                //if (!Directory.Exists(visaPath))
                                //{
                                //    DirectoryInfo di = Directory.CreateDirectory(visaPath);


                                //    string _path = visaPath + "\\accountbalances.txt";
                                //    if (!File.Exists(_path))
                                //    {

                                //        StringBuilder stringBuilder = new StringBuilder();
                                //        // Create a file to write to.
                                //        using (StreamWriter sw = File.CreateText(_path))
                                //        {

                                //            foreach (var listItem in list)
                                //            {
                                //                stringBuilder.Append(item.AccountNumber);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("0");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("0");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.AccType2);
                                //                stringBuilder.Append(Environment.NewLine);
                                //            }

                                //            sw.WriteLine(stringBuilder);

                                //        }
                                //    }


                                //    // Try to create the directory.
                                //    DirectoryInfo d1 = Directory.CreateDirectory(visaPath);

                                //    string path1 = visaPath + "\\accountoverridelimits.txt";
                                //    if (!File.Exists(path1))
                                //    {

                                //        StringBuilder stringBuilder = new StringBuilder();
                                //        // Create a file to write to.
                                //        using (StreamWriter sw = File.CreateText(path1))
                                //        {
                                //            stringBuilder.Append("");

                                //            sw.WriteLine(stringBuilder);

                                //        }
                                //    }

                                //    DirectoryInfo d2 = Directory.CreateDirectory(visaPath);
                                //    string path2 = visaPath + "\\acounts.txt";
                                //    if (!File.Exists(path2))
                                //    {

                                //        StringBuilder stringBuilder = new StringBuilder();
                                //        // Create a file to write to.
                                //        using (StreamWriter sw = File.CreateText(path2))
                                //        {
                                //            foreach (var listItem in list)
                                //            {
                                //                stringBuilder.Append(item.AccountNumber);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.AccType2);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.CurrencyCode);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.CustomerIdFinacle);
                                //                stringBuilder.Append(Environment.NewLine);
                                //            }

                                //            sw.WriteLine(stringBuilder);

                                //        }
                                //    }

                                //    DirectoryInfo d3 = Directory.CreateDirectory(visaPath);
                                //    string path3 = visaPath + "\\cardaccounts.txt";
                                //    if (!File.Exists(path3))
                                //    {

                                //        StringBuilder stringBuilder = new StringBuilder();
                                //        // Create a file to write to.
                                //        using (StreamWriter sw = File.CreateText(path3))
                                //        {
                                //            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                                //            foreach (var listItem in list)
                                //            {
                                //                stringBuilder.Append(item.Pan);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("001");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.AccountNumber);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.AccType2);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("1");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.AccType2);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.Guid);
                                //                stringBuilder.Append(Environment.NewLine);
                                //            }

                                //            sw.WriteLine(stringBuilder);

                                //        }
                                //    }


                                //    DirectoryInfo d4 = Directory.CreateDirectory(visaPath);
                                //    string path4 = visaPath + "\\cardoverridelimits.txt";
                                //    if (!File.Exists(path4))
                                //    {

                                //        StringBuilder stringBuilder = new StringBuilder();
                                //        // Create a file to write to.
                                //        using (StreamWriter sw = File.CreateText(path4))
                                //        {
                                //            stringBuilder.Append("");

                                //            sw.WriteLine(stringBuilder);

                                //        }
                                //    }


                                //    DirectoryInfo d5 = Directory.CreateDirectory(visaPath);
                                //    string path5 = visaPath + "\\cards.txt";
                                //    if (!File.Exists(path5))
                                //    {

                                //        StringBuilder stringBuilder = new StringBuilder();
                                //        // Create a file to write to.
                                //        using (StreamWriter sw = File.CreateText(path5))
                                //        {
                                //            foreach (var listItem in list)
                                //            {
                                //                //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                //                string[] birth = item.Birthday.Split(' ');
                                //                string[] month = birth[0].Split('-');

                                //                stringBuilder.Append(item.Pan);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("001");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("FBPMCNAIRA");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.AccType2);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("6");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("1902");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("4");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.NameOnCard);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.FIO);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.Address);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("234");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("NGA");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(month[0] + month[1] + month[2]);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("0");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("0");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append("ssppppccc");
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.EntryDate);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.EntryDate);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.Branch);//Branch Id
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.CustomerIdFinacle);
                                //                stringBuilder.Append(",");
                                //                stringBuilder.Append(item.Guid);
                                //                stringBuilder.Append(Environment.NewLine);
                                //            }

                                //            sw.WriteLine(stringBuilder);

                                //        }
                                //    }

                                //    DirectoryInfo d6 = Directory.CreateDirectory(visaPath);
                                //    string path6 = visaPath + "\\statements.txt";
                                //    if (!File.Exists(path6))
                                //    {

                                //        StringBuilder stringBuilder = new StringBuilder();
                                //        // Create a file to write to.
                                //        using (StreamWriter sw = File.CreateText(path6))
                                //        {
                                //            stringBuilder.Append("");

                                //            sw.WriteLine(stringBuilder);

                                //        }
                                //    }

                                //}
                            }

                            else
                            {

                                if (item.CardVersion > 1)
                                {
                                    var _list = list as List<IssuanceDisplayViewModel>;
                                    string _vpath = @"c:\VisaPinReIssuance" + item.Currency + dt.ToString(format);

                                    if (!Directory.Exists(_vpath))
                                    {
                                        DirectoryInfo di = Directory.CreateDirectory(_vpath);

                                        XDocument xdoc = new XDocument(
                                        new XDeclaration("1.0", "cp866", ""),
                                        // This is the root of the document
                                        new XElement("Root",
                                          _list.Select(l =>
                                        new XElement("Record",
                                            new XElement("PAN", ""),
                                            new XElement("MBR", "0"),
                                            new XElement("REASON", "5"),
                                            new XElement("cmCHANGEPAN", "1"),
                                            new XElement("CANCELDATE", ""),
                                            new XElement("NAMEONCARD", l.NameOnCard),
                                            new XElement("BRPART", l.Branch),
                                            new XElement("ADRESS", l.Address),
                                            new XElement("CARDPREFIX", l.CardPrefix),
                                            new XElement("ID", l.Guid)))));

                                        // Write the document to the file system            
                                        xdoc.Save(_vpath + $"\\VisaCard_{dt.ToString(format)}.xml");
                                    }
                                }

                                else
                                {
                                    // Build the document
                                    string vpath = @"c:\VisaCardIssuance" + "NGN" + dt.ToString(format);

                                    if (!Directory.Exists(vpath))
                                    {
                                        DirectoryInfo di = Directory.CreateDirectory(vpath);

                                        XDocument xdoc = new XDocument(
                                        new XDeclaration("1.0", "cp866", ""),
                                        // This is the root of the document
                                        new XElement("Root",
                                         list.Select(_item =>
                                        new XElement("Record",
                                            new XElement("FIO", _item.FIO),
                                            new XElement("ACCOUNT", _item.AccountNumber),
                                            new XElement("EXTACCOUNT", _item.AccountNumber),
                                            new XElement("ACCOUNTTP", _item.AccType2),
                                            new XElement("ACCTTYPE", _item.AccountType),
                                            new XElement("ACCTSTAT", "3"),
                                            new XElement("PAN", ""),
                                            new XElement("SEX", _item.Sex),
                                            new XElement("BIRTHDAY", _item.Birthday),
                                            new XElement("PASNOM", _item.CustomerIdFinacle),
                                            new XElement("ADRESS", _item.Address),
                                            new XElement("PHONE", _item.Phone),
                                            new XElement("COMPANY", ""),
                                            new XElement("CEH", ""),
                                            new XElement("TABNOM", "36063"),
                                            new XElement("LATFIO", ""),
                                            new XElement("BIRTHFIO", ""),
                                            new XElement("BIRTHPLACE", ""),
                                            new XElement("FAMILY", "2"),
                                            new XElement("SALARY", ""),
                                            new XElement("JOBPHONE", ""),
                                            new XElement("CORADDRESS", _item.Address),
                                            new XElement("EMAIL", _item.Email),
                                            new XElement("FAX", ""),
                                            new XElement("INN", ""),
                                            new XElement("RESIDENT", ""),
                                            new XElement("RESADDRESS", ""),
                                            new XElement("EDUCATION", ""),
                                            new XElement("STARTJOB", ""),
                                            new XElement("STARTBANK", ""),
                                            new XElement("PASPLACE", "VNP"),
                                            new XElement("COUNTRYRES", ""),
                                            new XElement("CMSPROFILE", ""),
                                            new XElement("BRPART", _item.Branch),
                                            new XElement("NAMEONCARD", _item.NameOnCard),
                                            new XElement("CURRENCYNO", _item.CurrencyCode),
                                            new XElement("CARDPREFIX", _item.CardPrefix),
                                            new XElement("ACCOUNTS",
                                            new XElement("ACCOUNT", new XAttribute("Index", "0")))))));

                                        // Write the document to the file system            
                                        xdoc.Save(vpath + $"\\VisaCard_{dt.ToString(format)}.xml");
                                    }
                                }

                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

        public static void GenPinFiles(IEnumerable<IssuanceDisplayViewModel> list)
        {
            // Specify the directory you want to manipulate.
            DateTime dt = DateTime.Now;
            string format = "dd-MM-yyyy";

            try
            {

                foreach (var item in list)
                {

                    switch (item.CardPrefix)
                    {
                        case "565432":
                            string path = @"c:\MsterPin_" + item.Currency + dt.ToString(format);

                            if (item.FileType == 1)
                            {
                                // Determine whether the directory exists.
                                if (!Directory.Exists(path))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(path);


                                    string _path = path + "\\accountbalances.txt";
                                    if (!File.Exists(_path))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(_path))
                                        {

                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    // Try to create the directory.
                                    DirectoryInfo d1 = Directory.CreateDirectory(path);

                                    string path1 = path + "\\accountoverridelimits.txt";
                                    if (!File.Exists(path1))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path1))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d2 = Directory.CreateDirectory(path);
                                    string path2 = path + "\\acounts.txt";
                                    if (!File.Exists(path2))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path2))
                                        {
                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CurrencyCode);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CustomerIdFinacle);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d3 = Directory.CreateDirectory(path);
                                    string path3 = path + "\\cardaccounts.txt";
                                    if (!File.Exists(path3))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path3))
                                        {
                                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.Pan);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("001");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("1");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Guid);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d4 = Directory.CreateDirectory(path);
                                    string path4 = path + "\\cardoverridelimits.txt";
                                    if (!File.Exists(path4))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path4))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d5 = Directory.CreateDirectory(path);
                                    string path5 = path + "\\cards.txt";
                                    if (!File.Exists(path5))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path5))
                                        {
                                            foreach (var listItem in list)
                                            {
                                                //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                                string[] birth = item.Birthday.Split(' ');
                                                string[] month = birth[0].Split('-');

                                                stringBuilder.Append(item.Pan);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("001");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("FBPMCNAIRA");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("6");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("1902");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("4");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.NameOnCard);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.FIO);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Address);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("234");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("NGA");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(month[0] + month[1] + month[2]);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("ssppppccc");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.EntryDate);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.EntryDate);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Branch);//Branch Id
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CustomerIdFinacle);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Guid);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d6 = Directory.CreateDirectory(path);
                                    string path6 = path + "\\statements.txt";
                                    if (!File.Exists(path6))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path6))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                }
                            }

                            else
                            {

                            }

                            break;

                        case "523421":

                            string vervePath = @"c:\VervePin_" + item.Currency + dt.ToString(format);

                            if (item.FileType == 1)
                            {
                                // Determine whether the directory exists.
                                if (!Directory.Exists(vervePath))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(vervePath);


                                    string _path = vervePath + "\\accountbalances.txt";
                                    if (!File.Exists(_path))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(_path))
                                        {

                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    // Try to create the directory.
                                    DirectoryInfo d1 = Directory.CreateDirectory(vervePath);

                                    string path1 = vervePath + "\\accountoverridelimits.txt";
                                    if (!File.Exists(path1))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path1))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d2 = Directory.CreateDirectory(vervePath);
                                    string path2 = vervePath + "\\acounts.txt";
                                    if (!File.Exists(path2))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path2))
                                        {
                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CurrencyCode);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CustomerIdFinacle);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d3 = Directory.CreateDirectory(vervePath);
                                    string path3 = vervePath + "\\cardaccounts.txt";
                                    if (!File.Exists(path3))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path3))
                                        {
                                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.Pan);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("001");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("1");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Guid);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d4 = Directory.CreateDirectory(vervePath);
                                    string path4 = vervePath + "\\cardoverridelimits.txt";
                                    if (!File.Exists(path4))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path4))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d5 = Directory.CreateDirectory(vervePath);
                                    string path5 = vervePath + "\\cards.txt";
                                    if (!File.Exists(path5))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path5))
                                        {
                                            foreach (var listItem in list)
                                            {
                                                //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                                string[] birth = item.Birthday.Split(' ');
                                                string[] month = birth[0].Split('-');

                                                stringBuilder.Append(item.Pan);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("001");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("FBPMCNAIRA");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("6");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("1902");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("4");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.NameOnCard);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.FIO);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Address);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("234");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("NGA");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(month[0] + month[1] + month[2]);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("ssppppccc");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.EntryDate);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.EntryDate);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Branch);//Branch Id
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CustomerIdFinacle);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Guid);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d6 = Directory.CreateDirectory(vervePath);
                                    string path6 = vervePath + "\\statements.txt";
                                    if (!File.Exists(path6))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path6))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                }
                            }

                            else
                            {

                            }
                            break;

                        case "465432":

                            if (item.FileType == 1)
                            {
                                string visaPath = @"c:\VisaTxtPin_" + item.Currency + dt.ToString(format);

                                // Determine whether the directory exists.
                                if (!Directory.Exists(visaPath))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(visaPath);


                                    string _path = visaPath + "\\accountbalances.txt";
                                    if (!File.Exists(_path))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(_path))
                                        {

                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    // Try to create the directory.
                                    DirectoryInfo d1 = Directory.CreateDirectory(visaPath);

                                    string path1 = visaPath + "\\accountoverridelimits.txt";
                                    if (!File.Exists(path1))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path1))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d2 = Directory.CreateDirectory(visaPath);
                                    string path2 = visaPath + "\\acounts.txt";
                                    if (!File.Exists(path2))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path2))
                                        {
                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CurrencyCode);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CustomerIdFinacle);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d3 = Directory.CreateDirectory(visaPath);
                                    string path3 = visaPath + "\\cardaccounts.txt";
                                    if (!File.Exists(path3))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path3))
                                        {
                                            //514585 * *****5608,001,4010932125,20,1,20,8e068434 - c942 - 43aa - 8a7f - 2a480e6670b8
                                            foreach (var listItem in list)
                                            {
                                                stringBuilder.Append(item.Pan);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("001");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccountNumber);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("1");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Guid);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d4 = Directory.CreateDirectory(visaPath);
                                    string path4 = visaPath + "\\cardoverridelimits.txt";
                                    if (!File.Exists(path4))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path4))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }


                                    DirectoryInfo d5 = Directory.CreateDirectory(visaPath);
                                    string path5 = visaPath + "\\cards.txt";
                                    if (!File.Exists(path5))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path5))
                                        {
                                            foreach (var listItem in list)
                                            {
                                                //514585 * *****3776,001,FBPMCNAIRA,20,6,,1902,,,,4,,,,,,,FIRST CHOICE, CONCEPT, AND INTERGRATED NIG, FIRST CHOICE CONCEPT,5 ADEMOLA ASHIRU STREET IJEGUN,, LAGOS,,234,NGA,20021216,0,,0,,ssppppccc,2016 - 02 - 12 16:27:12.000,2016 - 02 - 12 16:27:12.000,,016,002865598,b103f428 - ef2d - 4713 - 860b - 8a05592d3b7f

                                                string[] birth = item.Birthday.Split(' ');
                                                string[] month = birth[0].Split('-');

                                                stringBuilder.Append(item.Pan);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("001");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("FBPMCNAIRA");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.AccType2);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("6");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("1902");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("4");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.NameOnCard);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.FIO);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Address);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("234");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("NGA");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(month[0] + month[1] + month[2]);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("0");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append("ssppppccc");
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.EntryDate);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.EntryDate);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Branch);//Branch Id
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.CustomerIdFinacle);
                                                stringBuilder.Append(",");
                                                stringBuilder.Append(item.Guid);
                                                stringBuilder.Append(Environment.NewLine);
                                            }

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                    DirectoryInfo d6 = Directory.CreateDirectory(visaPath);
                                    string path6 = visaPath + "\\statements.txt";
                                    if (!File.Exists(path6))
                                    {

                                        StringBuilder stringBuilder = new StringBuilder();
                                        // Create a file to write to.
                                        using (StreamWriter sw = File.CreateText(path6))
                                        {
                                            stringBuilder.Append("");

                                            sw.WriteLine(stringBuilder);

                                        }
                                    }

                                }
                            }

                            else
                            {
                                // Build the document
                                var _list = list as List<IssuanceDisplayViewModel>;
                                string vpath = @"c:\VisaPinReIssuance" + item.Currency + dt.ToString(format);

                                if (!Directory.Exists(vpath))
                                {
                                    DirectoryInfo di = Directory.CreateDirectory(vpath);

                                    //foreach (var item in _list)
                                    //{
                                    XDocument xdoc = new XDocument(
                                    new XDeclaration("1.0", "cp866", ""),
                                    // This is the root of the document
                                    new XElement("Root",
                                      _list.Select(l =>
                                    new XElement("Record",
                                        new XElement("PAN", ""),
                                        new XElement("MBR", "0"),
                                        new XElement("REASON", "5"),
                                        new XElement("cmCHANGEPAN", "1"),
                                        new XElement("CANCELDATE", ""),
                                        new XElement("NAMEONCARD", l.NameOnCard),
                                        new XElement("BRPART", l.Branch),
                                        new XElement("ADRESS", l.Address),
                                        new XElement("CARDPREFIX", l.CardPrefix),
                                        new XElement("ID", l.Guid)))));

                                    // Write the document to the file system            
                                    xdoc.Save(vpath + $"\\VisaCard_{dt.ToString(format)}.xml");
                                    //}
                                }
                            }
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

            }

        }

        public static string Base64Encode(string plainText)
        {
            if (String.IsNullOrEmpty(plainText))
            {
                return null;
            }

            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        //Decode
        public static string Base64Decode(string base64EncodedData)
        {
            if(String.IsNullOrEmpty(base64EncodedData))
            {
                return null;
            }
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public void ErrorLog(string StackTrace, string Message, string Source,string InnerException)
        {

            try
            {
                string user = _session.GetInt32("UserId").ToString();


                ErrorLog errors = new ErrorLog()
                {
                    User = user,
                    StackTrace = StackTrace,
                    Message = Message,
                    Source = Source,
                    ErrorDate = DateTime.Now,
                    InnerException = InnerException
                };

                _appDbContext.errorLogs.Add(errors);
                _appDbContext.SaveChanges();
            }

            catch (Exception ex)
            {

            }
        }
    } 
}
