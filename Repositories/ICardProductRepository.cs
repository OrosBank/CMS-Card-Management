using Cards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public interface ICardProductRepository
    {
        IEnumerable<CardProduct> GetAllCardProducts();

        CardProduct GetCardProductById(int ProductId);

        void AddProduct(CardProduct cardProduct);

        void EditProduct(int ProductId);


        IEnumerable<FileType> GetAllFileTypes();

        IEnumerable<CardType> GetAllCardType();
        
    }
}
