namespace Mastercard.UI.Business.Interface
{
    /// <summary>
    /// Validar captchas
    /// </summary>
    public interface ICatpcha
    {
        bool Validate(string captchaResponse);
    }
}