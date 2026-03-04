using Finance_BackEnd.Data;
using Finance_BackEnd.Models;
using Finance_BackEnd.Models.DTOs;
using Finance_BackEnd.Services;
using Microsoft.EntityFrameworkCore;

namespace Finance_Backend_Tests
{
    public class TransactionServiceTests
    {
        private AppDbContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

            var databaseContext = new AppDbContext(options);
            databaseContext.Database.EnsureCreated();
            return databaseContext;
        }

        [Fact]
        public async Task CreateAsync_UnderagePerson_ShouldThrowException()
        {
            var context = GetDatabaseContext();
            var service = new TransactionService(context);

            var underagePerson = new Person("Test Person", 17);
            context.People.Add(underagePerson);

            var category = new Category("Income Category", CategoryPurpose.Income);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var request = new TransactionDto(
                "Test",
                100,
                DateTime.Now,
                TransactionType.Income,
                underagePerson.Id,
                category.Id
            );

            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                service.CreateAsync(request));

            Assert.Equal("Underage persons can only register expenses.", exception.Message);
        }

        [Fact]
        public async Task CreateAsync_AdultPerson_ShouldAllow()
        {
            var context = GetDatabaseContext();
            var service = new TransactionService(context);

            var adult = new Person("Test Person", 25);
            context.People.Add(adult);

            var category = new Category("Income Category", CategoryPurpose.Income);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var request = new TransactionDto(
                "Test",
                500,
                DateTime.Now,
                TransactionType.Income,
                adult.Id,
                category.Id
            );

            var result = await service.CreateAsync(request);

            Assert.NotNull(result);
            Assert.Equal(500, result.Value);
        }

        [Fact]
        public async Task GetFinancialReportAsync_ShouldCalculateTotalsAndBalanceCorrectly()
        {
            var context = GetDatabaseContext();
            var service = new TransactionService(context);

            var person = new Person("Test Person", 30);
            context.People.Add(person);

            var categoryIn = new Category("Category in", CategoryPurpose.Income);
            var categoryOut = new Category("Category out", CategoryPurpose.Expense);
            context.Categories.AddRange(categoryIn, categoryOut);
            await context.SaveChangesAsync();

            // Income: 5000
            // Expense: 1200 + 300 = 1500
            // Final value should be: 3500
            var transactions = new List<Transaction>
            {
                new("First income", 5000, DateTime.Now, TransactionType.Income, person.Id, categoryIn.Id),
                new("First expense", 1200, DateTime.Now, TransactionType.Expense, person.Id, categoryOut.Id),
                new("Second expense", 300, DateTime.Now, TransactionType.Expense, person.Id, categoryOut.Id)
            };

            context.Transactions.AddRange(transactions);
            await context.SaveChangesAsync();

            var report = await service.GetReportPerPersonAsync();
            var personResult = report.FirstOrDefault(r => r.PersonId == person.Id);

            Assert.NotNull(personResult);
            Assert.Equal("Test Person", personResult.PersonName);
            Assert.Equal(5000, personResult.TotalIncome);
            Assert.Equal(1500, personResult.TotalExpense);
            Assert.Equal(3500, personResult.Balance);
        }

        [Fact]
        public async Task GetFinancialReportAsync_WhenNoTransactions_ShouldReturnZeroTotals()
        {
            var context = GetDatabaseContext();
            var service = new TransactionService(context);

            var person = new Person("Test Person", 25);
            context.People.Add(person);
            await context.SaveChangesAsync();

            var report = await service.GetReportPerPersonAsync();
            var personResult = report.FirstOrDefault(r => r.PersonId == person.Id);

            Assert.NotNull(personResult);
            Assert.Equal(0, personResult.TotalIncome);
            Assert.Equal(0, personResult.TotalExpense);
            Assert.Equal(0, personResult.Balance);
        }

        [Fact]
        public async Task GetFinancialReportAsync_ShouldHandleMultiplePeopleIndependently()
        {
            var context = GetDatabaseContext();
            var service = new TransactionService(context);

            var p1 = new Person("Test Person 1", 20);
            var p2 = new Person("Test Person 2", 20);
            context.People.AddRange(p1, p2);

            var cat = new Category("Test Category", CategoryPurpose.Income);
            context.Categories.Add(cat);
            await context.SaveChangesAsync();

            context.Transactions.Add(new Transaction("T1", 1000, DateTime.Now, TransactionType.Income, p1.Id, cat.Id));
            context.Transactions.Add(new Transaction("T2", 2000, DateTime.Now, TransactionType.Income, p2.Id, cat.Id));
            await context.SaveChangesAsync();

            var report = (await service.GetReportPerPersonAsync()).ToList();

            Assert.Equal(1000, report.First(r => r.PersonId == p1.Id).TotalIncome);
            Assert.Equal(2000, report.First(r => r.PersonId == p2.Id).TotalIncome);
        }
    }
}