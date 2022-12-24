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
        //////Template Place Holder/////


        public RepositoryWrapper(StockPOSContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }


    }
}
