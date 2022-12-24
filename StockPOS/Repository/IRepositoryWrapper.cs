using System;
using System.Collections.Generic;
using System.Text;

namespace StockPOS.Repository
{
    public interface IRepositoryWrapper
    {
        IEventlogRepository Eventlog { get; }

        IUserRepository User { get; }

        IProductRepository Product { get; }

        ISaleRepository Sale { get; }

        IProductcategoryRepository Productcategory { get; }

        ICustomerRepository Customer { get; }

        IChangedpricelogRepository Changedpricelog { get; }
        //////Template Place Holder/////
    }
}
