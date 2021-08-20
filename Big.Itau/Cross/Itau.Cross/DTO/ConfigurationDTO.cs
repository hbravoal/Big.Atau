namespace Itau.Common.DTO
{
    /// <summary>
    /// PAra segmento de usuario
    /// </summary>
    public class ConfigurationDTO
    {
        public short Id
        {
            get; set;
        }

        public byte ProgramId
        {
            get; set;
        }

        public string RealSegment
        {
            get; set;
        }

        public byte? BillingGroupGoal
        {
            get; set;
        }

        public byte? RedemptionLimit
        {
            get; set;
        }
    }
}