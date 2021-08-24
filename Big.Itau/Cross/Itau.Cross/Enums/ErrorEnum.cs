namespace Itau.Common.Enums
{
    public enum ErrorEnum
    {
        /// <summary>
        /// Cuando usuario no existe.
        /// </summary>
        UserNotFound = 1,

        InvalidCredential = 2, //Cuando el Password no hace match con el username
        InternatlError = 3,

        /// <summary>
        /// Cuando el Usuario no tiene Token válido.
        /// </summary>
        UnauthorizedRequest,

        /// <summary>
        /// Cuando el Usuario ya realizo el cambio de producto seleccionado diario.
        /// </summary>
        ChangeLock,

        TokenExpired,

        /// <summary>
        /// El usuario cumplió el límite de redenciones
        /// </summary>
        CustomerCantRedeemLimit,

        /// <summary>
        /// Usuario no puede redimir (No cumple requisitos)
        CustomerCantRedeem,

        /// <summary>
        /// Cuando se intentó crear orden
        /// </summary>
        ErrorCreatingOrder,

        /// <summary>
        /// fecha limite para actualizar fecha
        /// </summary>
        DateLimitAward
    }
}