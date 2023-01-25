using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class ChangedpricelogRepository : RepositoryBase<Changedpricelog>, IChangedpricelogRepository
    {
        public ChangedpricelogRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}
