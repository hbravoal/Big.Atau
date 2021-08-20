using Mastercard.Common.Helpers;

namespace Mastercard.UI.Business.Interface
{
    public interface IMisionBL
    {
        Response GetMisionCustomer();

        Response GetMisionById(int Id);
        Response RedemptionPoint();
    }
}