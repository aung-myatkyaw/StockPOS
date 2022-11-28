using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class ProductcategoryRepository: RepositoryBase<Productcategory>, IProductcategoryRepository
    {
        public ProductcategoryRepository(StockPOSContext repositoryContext)
            :base(repositoryContext)
        {
        }

    }
}