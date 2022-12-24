using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class DebtbalanceRepository : RepositoryBase<Debtbalance>, IDebtbalanceRepository
    {
        public DebtbalanceRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}
