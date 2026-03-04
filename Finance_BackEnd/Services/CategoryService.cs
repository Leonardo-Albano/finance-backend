using Finance_BackEnd.Data;
using Finance_BackEnd.Models;
using Finance_BackEnd.Models.DTOs;
using Finance_BackEnd.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Finance_BackEnd.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;

        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() =>
            await _context.Categories.AsNoTracking().ToListAsync();

        public async Task<Category?> GetByIdAsync(int id) =>
            await _context.Categories.FindAsync(id);

        public async Task<Category> CreateAsync(CategoryDTO request)
        {
            var category = new Category(
                request.Description,
                request.Purpose
            );

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category?> UpdateAsync(int id, CategoryDTO request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return null;

            category.UpdateDetails(request.Description, request.Purpose);

            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
