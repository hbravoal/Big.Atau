using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface.Providers
{
    /// <summary>
    /// Proveedor para hacer una redención.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public interface IRedeemProvider<TRequest, TResponse> where TResponse : class
    {
        Task<TResponse> Execute(TRequest model);
    }
}