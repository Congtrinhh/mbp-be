namespace Domain.Entities
{
    /// <summary>
    /// a transaction from a contract
    /// </summary>
    public class Transaction : BaseEntity
    {
        public int ContractId { get; set; }
        public string FromAccount { get; set; } = string.Empty;
        public string ToAccount { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Time { get; set; }
        public string FromBank { get; set; } = string.Empty;
        public string ToBank { get; set; } = string.Empty;
    }
}
