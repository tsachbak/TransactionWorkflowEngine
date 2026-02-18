using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionWorkflowEngine.Models
{
    /// <summary>
    /// TransactionStatusHistory represents the historical record of status changes for a transaction.
    /// </summary>
    public sealed class TransactionStatusHistory
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public Guid TransactionId { get; set; }

        [Required]
        public int FromStatusId { get; set; }

        [Required]
        public int ToStatusId { get; set; }

        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; } = null!;

        [Required]
        public DateTime ChangedAt { get; set; }

        [MaxLength(500)]
        public string? Reason { get; set; }
    }
}
