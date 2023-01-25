using System;
using System.Collections.Generic;
using StockPOS.Models;

namespace StockPOS.Repository
{
    public class SearchedcountRepository : RepositoryBase<Searchedcount>, ISearchedcountRepository
    {
        public SearchedcountRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}