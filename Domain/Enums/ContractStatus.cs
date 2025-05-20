namespace Domain.Enums
{
    /// <summary>
    /// Trạng thái hợp đồng.
    /// </summary>
    public enum ContractStatus
    {
        /// <summary>
        /// Hợp đồng đang có hiệu lực.
        /// </summary>
        InEffect = 1,

        /// <summary>
        /// Hợp đồng đã bị hủy.
        /// </summary>
        Canceled = 2,

        /// <summary>
        /// Hợp đồng đã hoàn thành.
        /// </summary>
        Completed = 3
    }
}
