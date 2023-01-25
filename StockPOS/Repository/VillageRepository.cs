using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class VillageRepository : RepositoryBase<Village>, IVillageRepository
    {
        public VillageRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}