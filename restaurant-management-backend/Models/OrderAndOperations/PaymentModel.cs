using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace restaurant_management_backend.Models.OrderAndOperations
{
    public class PaymentModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid PaymentId { get; set; }

        [Required]
        public Guid OrderId { get; set; }

        [Required]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Amount { get; set; }

        [Required]
        public PaymentStatusEnum PaymentStatus { get; set; }

        [Required]
        public CurrencyEnum Currency { get; set; } = CurrencyEnum.INR; // Added Currency property

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public int SplitCount { get; set; } = 1;


        [ForeignKey("OrderId")]
        public virtual OrderModel Order { get; set; }
    }

    public enum CurrencyEnum
    {
        INR = 1,
        USD = 2,
        EUR = 3,
        GBP = 4
    }

    public enum PaymentStatusEnum
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4
    }

    public enum PaymentMethodEnum
    {
        Cash = 1,
        Card = 2,
        UPI = 3,
        Wallet = 4
    }
}
