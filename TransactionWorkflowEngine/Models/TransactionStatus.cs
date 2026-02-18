using System.ComponentModel.DataAnnotations;

namespace TransactionWorkflowEngine.Models
{
    /// <summary>
    /// TransactionStatus represents the various states a transaction can be in during its lifecycle.
    /// </summary>
    public sealed class TransactionStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
