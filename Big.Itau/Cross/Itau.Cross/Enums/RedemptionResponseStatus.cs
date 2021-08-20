namespace Itau.Common.Enums
{
    public enum RedemptionResponseStatus
    {
        Complete = 1,
        FailedCreatingClient,
        FailedCreatingShippingInformation,
        FailedClientCantRedeem,
        FailedResetCategories,
        FailedClientCantRedeemLimit,
        CanRedeem,
        FailedInventoryNotAvailable,
        FailedCreateOrderError,
        FailedCreateTicket,
        FailedResetCode
    }
}