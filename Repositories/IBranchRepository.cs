using Cards.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public interface IBranchRepository
    {
        Branch GetBranchById(int branchId);

        void AddBranch(Branch branch);

        void EditBranch(int branchId);

        IEnumerable<Branch> GetAllBranches();
    }
}
