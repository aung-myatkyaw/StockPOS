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

        IDebtbalanceRepository Debtbalance { get; }

        IProductbrandRepository Productbrand { get; }
        //////Template Place Holder/////
        
        IProductSizeRepository Productsize { get; }

        IProducttypeRepository Producttype { get; }

        IGroupvillageRepository Groupvillage { get;  }

        ISearchedcountRepository Searchedcount { get; }

        IUsertypeRepository Usertype { get; }

        IVillageRepository Village { get; }

        IWarehouseRepository Warehouse { get; }
    }
}
