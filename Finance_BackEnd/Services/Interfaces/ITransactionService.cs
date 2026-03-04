using Finance_BackEnd.Models;
using Finance_BackEnd.Models.DTOs;
using Finance_BackEnd.Models.Responses;

namespace Finance_BackEnd.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponse> CreateAsync(TransactionDto request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<TransactionResponse>> GetAllAsync();
        Task<TransactionResponse?> GetByIdAsync(int id);
        Task<IEnumerable<ReportPerPersonResponse>> GetReportPerPersonAsync();
        Task<TransactionResponse?> UpdateAsync(int id, TransactionDto request);
    }
}