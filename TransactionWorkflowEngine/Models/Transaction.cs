using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionWorkflowEngine.Models
{
    public sealed class Transaction
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int CurrentStatusId { get; set; }

        [ForeignKey(nameof(CurrentStatusId))]
        public TransactionStatus CurrentStatus { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = null!;

    }
}
