using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class UsertypeRepository : RepositoryBase<Usertype>, IUsertypeRepository
    {
        public UsertypeRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}