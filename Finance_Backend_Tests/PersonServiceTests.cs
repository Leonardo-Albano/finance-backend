using Finance_BackEnd.Models;

namespace Finance_Backend_Tests
{
    public class PersonTests
    {

        [Fact]
        public void CreatePerson_WithValidData_ShouldSucceed()
        {
            string validName = "Person Test";
            int validAge = 25;

            var person = new Person(validName, validAge);

            Assert.Equal("Person Test", person.Name);
            Assert.Equal(25, person.Age);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void CreatePerson_WithInvalidName_ShouldThrowArgumentException(string invalidName)
        {
            var exception = Assert.Throws<ArgumentException>(() => new Person(invalidName, 30));
            Assert.Equal("Name is required.", exception.Message);
        }

        [Fact]
        public void CreatePerson_WithLongName_ShouldThrowArgumentException()
        {
            string longName = new string('A', 201);

            var exception = Assert.Throws<ArgumentException>(() => new Person(longName, 30));
            Assert.Equal("Name cannot exceed 200 characters.", exception.Message);
        }

        [Fact]
        public void CreatePerson_WithNegativeAge_ShouldThrowArgumentException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new Person("Test Person", -1));
            Assert.Equal("Age cannot be negative.", exception.Message);
        }

        [Fact]
        public void Person_ShouldInitializeWithEmptyTransactionList()
        {
            var person = new Person("Test Person", 20);

            Assert.NotNull(person.Transactions);
            Assert.Empty(person.Transactions);
        }
    }
}
