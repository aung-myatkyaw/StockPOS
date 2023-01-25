using StockPOS.Models;

namespace StockPOS.Repository
{
    public class ProductbrandRepository : RepositoryBase<Productbrand>, IProductbrandRepository
    {
        public ProductbrandRepository(StockPOSContext repositoryContext)
            : base(repositoryContext)
        {
        }

    }
}
