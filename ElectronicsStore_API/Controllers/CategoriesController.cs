using ElectronicsStore.API.Data;
using ElectronicsStore.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/categories
        // Returns the category tree (Root categories with their children)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetCategories()
        {
            // Fetch all categories
            var categories = await _context.Categories.ToListAsync();

            // Find root categories (where ParentCategoryId is null)
            var rootCategories = categories.Where(c => c.ParentCategoryId == null).ToList();

            // Map entities to DTOs recursively
            var response = rootCategories.Select(c => MapToDto(c, categories)).ToList();

            return Ok(response);
        }

        // Recursive method to build the category tree DTO
        private CategoryResponseDto MapToDto(Models.Category category, List<Models.Category> allCategories)
        {
            var dto = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };

            var children = allCategories.Where(c => c.ParentCategoryId == category.Id).ToList();
            foreach (var child in children)
            {
                dto.SubCategories.Add(MapToDto(child, allCategories));
            }

            return dto;
        }
    }
}