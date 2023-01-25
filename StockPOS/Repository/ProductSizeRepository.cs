using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class ProductSizeRepository : RepositoryBase<Productsize>, IProductSizeRepository
    {
        public ProductSizeRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}