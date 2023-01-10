using StockPOS;
using StockPOS.Models;


namespace StockPOS.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private StockPOSContext _repoContext;

        private IEventlogRepository oEventlog;
        public IEventlogRepository Eventlog
        {
            get
            {
                if (oEventlog == null)
                {
                    oEventlog = new EventlogRepository(_repoContext);
                }

                return oEventlog;
            }
        }

        private IUserRepository oUser;
        public IUserRepository User
        {
            get
            {
                if (oUser == null)
                {
                    oUser = new UserRepository(_repoContext);
                }

                return oUser;
            }
        }

        private IProductRepository oProduct;
        public IProductRepository Product
        {
            get
            {
                if (oProduct == null)
                {
                    oProduct = new ProductRepository(_repoContext);
                }

                return oProduct;
            }
        }

        private ISaleRepository oSale;
        public ISaleRepository Sale
        {
            get
            {
                if (oSale == null)
                {
                    oSale = new SaleRepository(_repoContext);
                }

                return oSale;
            }
        }

        private IProductcategoryRepository oProductcategory;
        public IProductcategoryRepository Productcategory
        {
            get
            {
                if (oProductcategory == null)
                {
                    oProductcategory = new ProductcategoryRepository(_repoContext);
                }

                return oProductcategory;
            }
        }

        private ICustomerRepository oCustomer;
        public ICustomerRepository Customer
        {
            get
            {
                if (oCustomer == null)
                {
                    oCustomer = new CustomerRepository(_repoContext);
                }

                return oCustomer;
            }
        }

        private IChangedpricelogRepository ochangedpricelog;
        public IChangedpricelogRepository Changedpricelog
        {
            get
            {
                if (ochangedpricelog == null)
                {
                    ochangedpricelog = new ChangedpricelogRepository(_repoContext);
                }

                return ochangedpricelog;
            }
        }

        private IDebtbalanceRepository odebtbalance;
        public IDebtbalanceRepository Debtbalance
        {
            get
            {
                if (odebtbalance == null)
                {
                    odebtbalance = new DebtbalanceRepository(_repoContext);
                }

                return odebtbalance;
            }
        }

        private IProductbrandRepository oproductbrand;
        public IProductbrandRepository Productbrand
        {
            get
            {
                if (oproductbrand == null)
                {
                    oproductbrand = new ProductbrandRepository(_repoContext);
                }

                return oproductbrand;
            }
        }

        private IProductSizeRepository oProductsize;
        public IProductSizeRepository Productsize
        {
            get
            {
                if (oProductsize == null)
                {
                    oProductsize = new ProductSizeRepository(_repoContext);
                }

                return oProductsize;
            }
        }

        private IProducttypeRepository oProducttype;
        public IProducttypeRepository Producttype
        {
            get
            {
                if (oProducttype == null)
                {
                    oProducttype = new ProducttypeRepository(_repoContext);
                }

                return oProducttype;
            }
        }
        
         private IGroupvillageRepository oGroupvillage;
        public IGroupvillageRepository Groupvillage
        {
            get
            {
                if (oGroupvillage == null)
                {
                    oGroupvillage = new GroupvillageRepository(_repoContext);
                }

                return oGroupvillage;
            }
        }
        
        private ISearchedcountRepository oSearchedcount;
        public ISearchedcountRepository Searchedcount
        {
            get
            {
                if (oSearchedcount == null)
                {
                    oSearchedcount = new SearchedcountRepository(_repoContext);
                }

                return oSearchedcount;
            }
        }

        private IUsertypeRepository oUsertype;
        public IUsertypeRepository Usertype
        {
            get
            {
                if (oUsertype == null)
                {
                    oUsertype = new UsertypeRepository(_repoContext);
                }

                return oUsertype;
            }
        }

        private IVillageRepository oVillage;
        public IVillageRepository Village
        {
            get
            {
                if (oVillage == null)
                {
                    oVillage = new VillageRepository(_repoContext);
                }

                return oVillage;
            }
        }

        private IWarehouseRepository oWarehouse;
        public IWarehouseRepository Warehouse
        {
            get
            {
                if (oWarehouse == null)
                {
                    oWarehouse = new WarehouseRepository(_repoContext);
                }

                return oWarehouse;
            }
        }
        //////Template Place Holder////


        public RepositoryWrapper(StockPOSContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }


    }
}
