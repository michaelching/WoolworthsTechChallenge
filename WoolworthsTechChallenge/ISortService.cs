using System.Collections.Generic;
using System.Threading.Tasks;

namespace WoolworthsTechChallenge
{
    public interface ISortService
    {
        Task<IEnumerable<Product>> Sort(string sortOption, IEnumerable<Product> products);
    }
}