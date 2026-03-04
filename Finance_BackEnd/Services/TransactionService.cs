using Finance_BackEnd.Data;
using Finance_BackEnd.Models.DTOs;
using Finance_BackEnd.Models;
using Microsoft.EntityFrameworkCore;
using Finance_BackEnd.Models.Responses;
using Finance_BackEnd.Services.Interfaces;

namespace Finance_BackEnd.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly AppDbContext _context;

        public TransactionService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransactionResponse>> GetAllAsync()
        {
            return await _context.Transactions
                .Include(t => t.Person)
                .Include(t => t.Category)
                .AsNoTracking()
                .Select(t => t.ToResponse())
                .ToListAsync();
        }

        public async Task<TransactionResponse?> GetByIdAsync(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Person)
                .Include(t => t.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null) return null;

            return transaction.ToResponse();
        }

        public async Task<TransactionResponse> CreateAsync(TransactionDto request)
        {
            var person = await _context.People.FindAsync(request.PersonId)
                ?? throw new ArgumentException("Person not found.");

            var category = await _context.Categories.FindAsync(request.CategoryId)
                ?? throw new ArgumentException("Category not found.");

            if (person.Age < 18 && request.Type == TransactionType.Income)
                throw new ArgumentException("Underage persons can only register expenses.");

            var transaction = new Transaction(
                request.Description,
                request.Value,
                request.Date,
                request.Type,
                request.PersonId,
                request.CategoryId
            );

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return transaction.ToResponse();
        }

        public async Task<TransactionResponse?> UpdateAsync(int id, TransactionDto request)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return null;

            var person = await _context.People.FindAsync(request.PersonId)
                ?? throw new ArgumentException("Person not found.");

            if (person.Age < 18 && request.Type == TransactionType.Income)
                throw new ArgumentException("Underage persons can only register expenses.");

            transaction.UpdateDetails(
                request.Description,
                request.Value,
                request.Date,
                request.Type,
                request.PersonId,
                request.CategoryId
            );

            await _context.SaveChangesAsync();
            return transaction.ToResponse();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null) return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ReportPerPersonResponse>> GetReportPerPersonAsync()
        {
            return await _context.People
                .Include(p => p.Transactions)
                .AsNoTracking()
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    Income = (decimal)p.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => (double)t.Value),
                    Expense = (decimal)p.Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => (double)t.Value)
                })
                .Select(x => new ReportPerPersonResponse(
                    x.Id,
                    x.Name,
                    x.Income,
                    x.Expense,
                    x.Income - x.Expense
                ))
                .ToListAsync();
        }
    }
}
