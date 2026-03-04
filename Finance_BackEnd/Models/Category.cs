using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Finance_BackEnd.Models
{
    public class Category
    {
        #region Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [MaxLength(400)]
        public string Description { get; private set; } = string.Empty;

        [Required]
        public CategoryPurpose Purpose { get; private set; }
        
        #endregion
        
        #region Constructors

        protected Category() { }

        public Category(string description, CategoryPurpose purpose)
        {
            Description = description;
            Purpose = purpose;

            Validate();
        }

        #endregion

        #region Methods

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Description))
                throw new ArgumentException("Description is required.");

            if (Description.Length > 400)
                throw new ArgumentException("Description cannot exceed 400 characters.");

            if (!Enum.IsDefined(typeof(CategoryPurpose), Purpose))
                throw new ArgumentException("Invalid category purpose.");
        }

        public void UpdateDetails(string description, CategoryPurpose purpose)
        {
            Description = description?.Trim() ?? string.Empty;
            Purpose = purpose;

            Validate();
        }

        #endregion
    }
}
