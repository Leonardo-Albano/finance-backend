using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Finance_BackEnd.Models.Responses;

namespace Finance_BackEnd.Models
{
    public class Transaction
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public string Description { get; private set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; private set; }

        [Required]
        public DateTime Date { get; private set; }

        [Required]
        public TransactionType Type { get; private set; }

        [Required]
        public int PersonId { get; private set; }

        [ForeignKey("PersonId")]
        public virtual Person? Person { get; private set; }

        [Required]
        public int CategoryId { get; private set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; private set; }

        #endregion

        #region Constructors

        protected Transaction() { }

        public Transaction(string description, decimal value, DateTime date, TransactionType type, int personId, int categoryId)
        {
            UpdateDetails(description, value, date, type, personId, categoryId);
        }

        #endregion

        #region Methods

        public void UpdateDetails(string description, decimal value, DateTime date, TransactionType type, int personId, int categoryId)
        {
            Description = description?.Trim() ?? string.Empty;
            Value = value;
            Date = date;
            Type = type;
            PersonId = personId;
            CategoryId = categoryId;

            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("Description is required.");

            if (Value <= 0)
                throw new ArgumentException("Value must be greater than zero.");

            if (!Enum.IsDefined(typeof(TransactionType), Type))
                throw new ArgumentException("Invalid transaction type.");
        }

        public TransactionResponse ToResponse()
        {
            return new TransactionResponse(
                Id,
                Description,
                Value,
                Date,
                Type.ToString(),
                Person?.Name ?? string.Empty,
                Category?.Description ?? string.Empty
            );
        }
        #endregion
    }
}
