using System.Collections.Generic;
using System.Threading.Tasks;

namespace WoolworthsTechChallenge
{
    public interface IProductsClient
    {
        Task<IEnumerable<Product>> GetProducts();
    }
}
