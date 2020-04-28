using System.Threading.Tasks;

namespace WoolworthsTechChallenge
{
    public interface ITrolleyCalculatorClient
    {
        Task<decimal> CalculateTotal(string trolleyJson);
    }
}
