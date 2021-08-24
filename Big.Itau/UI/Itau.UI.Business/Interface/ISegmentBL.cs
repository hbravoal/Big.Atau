using Itau.Common.Wrapper;

namespace Mastercard.UI.Business.Interface
{
    public interface IUserState
    {
        Response<bool> GetGoal();
    }
}