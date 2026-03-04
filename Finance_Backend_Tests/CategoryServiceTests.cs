using Finance_BackEnd.Models;

namespace Finance_BackEnd_Tests
{
    public class CategoryServiceTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Category_Constructor_ShouldThrowException_DescriptionIsEmpty(string invalidDescription)
        {
            Assert.Throws<ArgumentException>(() =>
                new Category(invalidDescription, CategoryPurpose.Income));
        }
    }
}