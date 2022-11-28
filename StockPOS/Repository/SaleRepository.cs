using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class SaleRepository: RepositoryBase<Sale>, ISaleRepository
    {
        public SaleRepository(StockPOSContext repositoryContext)
            :base(repositoryContext)
        {
        }

    }
}