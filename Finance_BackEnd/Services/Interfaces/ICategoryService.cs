using Finance_BackEnd.Models;
using Finance_BackEnd.Models.DTOs;

namespace Finance_BackEnd.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category?> GetByIdAsync(int id);
        Task<Category> CreateAsync(CategoryDTO request);
        Task<Category?> UpdateAsync(int id, CategoryDTO request);
        Task<bool> DeleteAsync(int id);
    }
}