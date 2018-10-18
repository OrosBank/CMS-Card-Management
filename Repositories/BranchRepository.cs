using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cards.DatabaseLink;
using Cards.Models.ViewModels;

namespace Cards.Repositories
{
    public class BranchRepository : IBranchRepository
    {

        List<Branch> _branchList;
        private readonly ApplicationDbContext _appDbContext;

        public BranchRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddBranch(Branch branch)
        {
            _appDbContext.Branches.Add(branch);
            _appDbContext.SaveChanges();
        }

        public void EditBranch(int branchId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Branch> GetAllBranches()
        {
            _branchList = _appDbContext.Branches.ToList();

            return _branchList;
        }

        public Branch GetBranchById(int branchId)
        {
            return _appDbContext.Branches.Where(b => b.Id == branchId).FirstOrDefault();
        }
    }
}
