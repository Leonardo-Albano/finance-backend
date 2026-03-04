using Finance_BackEnd.Models;
using Finance_BackEnd.Models.DTOs;

namespace Finance_BackEnd.Services.Interfaces
{
    public interface IPersonService
    {
        Task<Person> CreateAsync(PersonDTO request);
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task<Person?> UpdateAsync(int id, PersonDTO request);
    }
}