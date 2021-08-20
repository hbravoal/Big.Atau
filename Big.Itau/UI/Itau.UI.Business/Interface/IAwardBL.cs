using Mastercard.Common.Helpers;

namespace Mastercard.UI.Business.Interface
{
    public interface IAwardBL
    {
        Response GetAward();

        Response SaveAwards(short AwardId);

        Response GetAwardById(short id);
    }
}