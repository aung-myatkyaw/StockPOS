using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class GroupvillageRepository : RepositoryBase<Groupvillage>, IGroupvillageRepository
    {
        public GroupvillageRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}