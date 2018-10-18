using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public interface IAccountController
    {
        bool CheckSessionState();
    }
}
