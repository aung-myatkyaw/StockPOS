using StockPOS;
using StockPOS.Models;


namespace StockPOS.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private StockPOSContext _repoContext;


        private IBoughtRepository oBought;
        public IBoughtRepository Bought
        {
            get
            {
                if (oBought == null)
                {
                    oBought = new BoughtRepository(_repoContext);
                }

                return oBought;
            }
        }

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

        private ICashierRepository oCashier;
        public ICashierRepository Cashier
        {
            get
            {
                if (oCashier == null)
                {
                    oCashier = new CashierRepository(_repoContext);
                }

                return oCashier;
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
//////Template Place Holder/////


        public RepositoryWrapper(StockPOSContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }


    }
}
