using System.Collections.Generic;
using System.Threading.Tasks;

namespace WoolworthsTechChallenge
{
    public interface IShopperHistoryClient
    {
        Task<IEnumerable<ShopperHistory>> GetShopperHistory();
    }
}