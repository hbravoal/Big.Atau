using System.Threading.Tasks;

namespace Mastercard.UI.Business.Interface
{
    /// <summary>
    /// Module for get tokens, Auth
    /// </summary>
    public interface IAuthentication
    {
        Task Login();
    }
}