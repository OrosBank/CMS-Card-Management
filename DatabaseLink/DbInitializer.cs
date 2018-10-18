//using Cards.Models;
//using Cards.Models.ViewModels;
using Cards.Models;
using Cards.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.DatabaseLink
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.CardStatuses.Any())
            {

                context.AddRange(
                new CardStatus { Status = "New" },
                new CardStatus { Status = "Unverified" },
                new CardStatus { Status = "Verified" },
                new CardStatus { Status = "Declined By Authorizer" },
                new CardStatus { Status = "Authorized By Authorizer" },
                new CardStatus { Status = "Card Sent For Processing" },
                new CardStatus { Status = "Card Recieved From Processing" },
                new CardStatus { Status = "Card Dispatched To Branch" },
                new CardStatus { Status = "Card Recieved From Dispatch" },
                new CardStatus { Status = "Card Released To Customer" }
                );
                context.SaveChanges();
            }

            if (!context.PinStatus.Any())
            {

                context.AddRange(
                new PinStatus { Status = "New" },
                new PinStatus { Status = "Unverified" },
                new PinStatus { Status = "Verified" },
                new PinStatus { Status = "Declined By Authorizer" },
                new PinStatus { Status = "Authorized By Authorizer" },
                new PinStatus { Status = "Pin Sent For Processing" },
                new PinStatus { Status = "Pin Recieved From Processing" },
                new PinStatus { Status = "Pin Dispatched To Branch" },
                new PinStatus { Status = "Pin Recieved From Dispatch" },
                new PinStatus { Status = "Pin Released To Customer" }
                );
                context.SaveChanges();
            }

            if (!context.FileType.Any())
            {

                context.AddRange(
                new FileType { Name = "txt" },
                new FileType { Name = "xml" }
                );
                context.SaveChanges();
            }

            if (!context.CardModes.Any())
            {

                context.AddRange(
                new CardMode { Name = "Card Issuance" },
                new CardMode { Name = "Card Re-Issuance" },
                new CardMode { Name = "Pin Re-Issuance" }
                );
                context.SaveChanges();
            }

            if (!context.CardTypes.Any())
            {

                context.AddRange(
                new CardType { Name = "Master" },
                new CardType { Name = "Visa" },
                new CardType { Name = "Verve" }
                );
                context.SaveChanges();
            }

            if (!context.Branches.Any())
            {

                context.AddRange(
                new Branch { BranchName = "Branch 1", Sol = "001" },
                new Branch { BranchName = "Branch 2", Sol = "002" },
                new Branch { BranchName = "Branch 3", Sol = "003" },
                new Branch { BranchName = "Branch 4", Sol = "004" },
                new Branch { BranchName = "Branch 5", Sol = "005" },
                new Branch { BranchName = "Branch 6", Sol = "006" },
                new Branch { BranchName = "Branch 7", Sol = "007" },
                new Branch { BranchName = "Branch 8", Sol = "008" },
                new Branch { BranchName = "Branch 9", Sol = "009" },
                new Branch { BranchName = "Branch 10", Sol = "010" }
                );
                context.SaveChanges();
            }


            if (!context.Roles.Any())
            {

                context.AddRange(
                new ApplicationRole { Name = "Admin", CreatedDate = DateTime.Now }
                );
                context.SaveChanges();
            }
        }
    }
}
