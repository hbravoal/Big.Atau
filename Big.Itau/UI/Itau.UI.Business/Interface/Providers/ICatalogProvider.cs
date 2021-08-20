using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface.Providers
{
    public interface ICatalogProvider<TRequest, TResponse> where TResponse : class
    {
        Task<TResponse> Get(TRequest model);
    }
}