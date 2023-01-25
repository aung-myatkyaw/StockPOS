using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class ProducttypeRepository : RepositoryBase<Producttype>, IProducttypeRepository
    {
        public ProducttypeRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}