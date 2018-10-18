﻿using Cards.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public interface IPanScramblerRepository
    {
        IEnumerable<PanUploadDetails> GetAllPanDetails();

        string UploadPanFile(IFormFile file);
    }
}
