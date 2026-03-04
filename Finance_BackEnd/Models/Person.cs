using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Finance_BackEnd.Models
{
    public class Person
    {
        #region Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [MaxLength(200)]
        public string Name { get; private set; } = string.Empty;

        [Required]
        public int Age { get; private set; }

        public virtual ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        #endregion

        #region Constructors
        protected Person() { }

        public Person(string name, int age)
        {
            UpdateDetails(name, age);
        }

        #endregion

        #region Methods
        public void UpdateDetails(string name, int age)
        {
            Name = name?.Trim() ?? string.Empty;
            Age = age;
            Validate();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("Name is required.");

            if (Name.Length > 200)
                throw new ArgumentException("Name cannot exceed 200 characters.");

            if (Age < 0)
                throw new ArgumentException("Age cannot be negative.");
        }
        #endregion
    }
}
