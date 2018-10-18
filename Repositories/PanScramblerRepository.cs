using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Cards.DatabaseLink;
using Cards.Helpers;
using Cards.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Cards.Repositories
{
    public class PanScramblerRepository : IPanScramblerRepository
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        private IHostingEnvironment _env;
        List<PanUploadDetails> _panList;
        public PanScramblerRepository(ApplicationDbContext appDbContext, IHostingEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<PanUploadDetails> GetAllPanDetails()
        {
            _panList = _appDbContext.PanUploadDetails.ToList();

            return _panList;
        }

        public string UploadPanFile(IFormFile file)
        {
            string feedback = string.Empty;
            PanUpload panUpload = new PanUpload();
            string[] lineDetails;

            
            List<PanUploadDetails> panDetailsList = new List<PanUploadDetails>();

            try
            {

                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    string path = "c:\\VisaPanUpoads\\";
                    var webRoot = _env.WebRootPath;//safe file to wwwroot

                    //var readFile = reader.ReadToEnd();
                    //string textLine = reader.ReadLine();
                    do
                    {
                        string textLine = reader.ReadLine();

                        lineDetails = textLine.Split(',');
                        PanUploadDetails panUploadDetails = new PanUploadDetails();

                        panUploadDetails.guid = lineDetails[0];
                        panUploadDetails.itc_id = lineDetails[1];
                        string pan = lineDetails[2];

                        panUploadDetails.pan = MaskVisaPan(pan);

                        panUploadDetails.nameoncard = lineDetails[3];
                        panUploadDetails.card_type = lineDetails[4];
                        panUploadDetails.acctno1 = lineDetails[5];
                        panUploadDetails.acctno2 = lineDetails[6];

                        panDetailsList.Add(panUploadDetails);

                    }

                    while (reader.Peek() != -1);
                    reader.Close();

                    _appDbContext.PanUploadDetails.AddRange(panDetailsList);
                    _appDbContext.SaveChanges();


                    var parsedContentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                    var fileName = parsedContentDisposition.FileName;
                    var fileType = parsedContentDisposition.Name;

                    var str = fileName.Substring(1, fileName.Length - 2);
                    var _path = path + str;

                    using (var stream = new FileStream(_path, FileMode.Create))
                    {
                        file.CopyToAsync(stream);
                    }


                    panUpload.filename = str;
                    panUpload.upload_date = DateTime.Now;
                    panUpload.uploaded_by = (int)_session.GetInt32("UserId");

                    _appDbContext.PanUploads.Add(panUpload);
                    _appDbContext.SaveChanges();

                    feedback = "File Uploaded Successfully !!!";
                }
            }

            catch(Exception ex)
            {

            }

            return feedback;
        }

        public string MaskVisaPan(string pan)
        {
            string maskedString = string.Empty;
            try
            {
                //var sol = _appDbContext.Branches.Where(b => b.Id == branchId).Select(s => s.Sol).FirstOrDefault();

                //if (sol != null)
                //{

                    string _pan = pan;
                Random r = new Random();
                int midDigits = r.Next(1, 7);

                var _firstDigits = _pan.Substring(4);
                    Random random = new Random();
                    int _lastDigits = random.Next(1, 7);

                    //var cardNumber = _firstDigits + midDigits + _lastDigits;

                    var firstDigits = _pan.Substring(0, 6);
                    var lastDigits = _pan.Substring(_pan.Length - 4, 4);

                    var requiredMask = new String('*', _pan.Length - firstDigits.Length - lastDigits.Length);

                    maskedString = string.Concat(firstDigits, requiredMask, lastDigits);

                    PanDetails panDetails = new PanDetails()
                    {
                        Pan = Activity.Base64Encode(_pan),
                        MaskedPan = maskedString,
                        CardIssuanceId = null,
                        EntryDate = DateTime.Now
                    };


                    _appDbContext.PanDetails.Add(panDetails);
                    _appDbContext.SaveChanges();
                //}

            }
            catch (Exception ex)
            {

            }


            return maskedString;
        }
    }
}
