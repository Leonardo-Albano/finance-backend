using Finance_BackEnd.Data;
using Finance_BackEnd.Models;
using Finance_BackEnd.Models.DTOs;
using Finance_BackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Finance_BackEnd.Services
{
    public class PersonService : IPersonService
    {
        private readonly AppDbContext _context;

        public PersonService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Person>> GetAllAsync() =>
            await _context.People.AsNoTracking().ToListAsync();

        public async Task<Person?> GetByIdAsync(int id) =>
            await _context.People.FindAsync(id);

        public async Task<Person> CreateAsync(PersonDTO request)
        {
            var person = new Person(
                request.Name,
                request.Age
            );

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return person;
        }

        public async Task<Person?> UpdateAsync(int id, PersonDTO request)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null) return null;

            person.UpdateDetails(request.Name, request.Age);

            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var person = await _context.People.FindAsync(id);
            if (person == null) return false;

            _context.People.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
