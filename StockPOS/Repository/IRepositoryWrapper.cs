using System;
using System.Collections.Generic;
using System.Text;

namespace StockPOS.Repository
{
    public interface IRepositoryWrapper
    {

        IBoughtRepository Bought { get; }

        IEventlogRepository Eventlog { get; }

        ICashierRepository Cashier { get; }

        IProductRepository Product { get; }

        ISaleRepository Sale { get; }

        IProductcategoryRepository Productcategory { get; }
//////Template Place Holder/////
    }
}
