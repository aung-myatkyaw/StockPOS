using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class ProductRepository: RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(StockPOSContext repositoryContext)
            :base(repositoryContext)
        {
        }

    }
}