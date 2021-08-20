namespace Itau.Common.DTO.Response
{
    public class CategoryDTO
    {
        public short Id { get; set; }
        public string Name { get; set; }
        public string IconName { get; set; }
        public string Tooltip { get; set; }
        public string IsBinary { get; set; }
        public bool IsOnline { get; set; }
        public decimal Available { get; set; }
        public decimal AccumulatedMoney { get; set; }
        public int Position { get; set; }
        public short RedemptionOrder { get; set; }
    }
}