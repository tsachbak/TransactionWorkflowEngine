using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionWorkflowEngine.Models
{
    /// <summary>
    /// TransactionStatusTransition represents the allowed transitions between different transaction statuses in the workflow.
    /// </summary>
    public sealed class TransactionStatusTransition
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int FromStatusId { get; set; }

        [Required]
        public int ToStatusId { get; set; }

        [ForeignKey(nameof(FromStatusId))]
        public TransactionStatus FromStatus { get; set; } = null!;

        [ForeignKey(nameof(ToStatusId))]
        public TransactionStatus ToStatus { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }
    }
}
