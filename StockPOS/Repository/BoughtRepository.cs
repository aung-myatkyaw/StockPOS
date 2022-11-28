using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class BoughtRepository: RepositoryBase<Bought>, IBoughtRepository
    {
        public BoughtRepository(StockPOSContext repositoryContext)
            :base(repositoryContext)
        {
        }
    }
}