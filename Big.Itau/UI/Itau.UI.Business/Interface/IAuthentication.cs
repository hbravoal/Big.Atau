using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface
{
    /// <summary>
    /// [Marketplace]Module for get tokens, Auth
    /// </summary>
    public interface IAuthentication
    {
        Task Login();
    }
}