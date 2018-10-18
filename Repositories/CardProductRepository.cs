using Cards.DatabaseLink;
using Cards.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cards.Repositories
{
    public class CardProductRepository : ICardProductRepository
    {
        List<CardProduct> _productList;
        List<FileType> _fileTypeList;
        List<CardType> _cardTypeList;

        private readonly ApplicationDbContext _appDbContext;

        public CardProductRepository(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public void AddProduct(CardProduct cardProduct)
        {
            cardProduct.IsSecAccRequired = Convert.ToBoolean(cardProduct.IsSecAccRequired.ToString().Split(',')[0]);

            _appDbContext.CardProducts.Add(cardProduct);
            _appDbContext.SaveChanges();
        }

        public IEnumerable<CardProduct> GetAllCardProducts()
        {
            // _productList = new List<CardProduct>();
            _productList = _appDbContext.CardProducts.ToList();

            return _productList;
        }

        public CardProduct GetCardProductById(int ProductId)
        {

            return _productList.FirstOrDefault(p => p.Id == ProductId); 

        }

       

        public IEnumerable<FileType> GetAllFileTypes()
        {
            _fileTypeList = _appDbContext.FileType.ToList();

            _fileTypeList.Insert(0, new FileType { Id = 0, Name = "Select" });

            return _fileTypeList;
        }

        public IEnumerable<CardType> GetAllCardType()
        {

            _cardTypeList = _appDbContext.CardTypes.ToList();

            _cardTypeList.Insert(0, new CardType { Id = 0, Name = "Select" });

            return _cardTypeList;
        }

        public void EditProduct(int ProductId)
        {
            _productList.FirstOrDefault(c => c.Id == ProductId);
            _appDbContext.SaveChanges();
        }
    }
}
