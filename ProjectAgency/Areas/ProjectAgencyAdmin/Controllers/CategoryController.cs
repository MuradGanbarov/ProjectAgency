using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels;
using ProjectAgency.DAL;
using ProjectAgency.Models;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.Controllers
{
    [Area("ProjectAgencyAdmin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Products.CountAsync();
            List<Category>? categories = await _context.Categories.Skip(page*3).Take(3).Include(c=>c.Products).ToListAsync();

            PaginationVM<Category> vm = new()
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count/3),
                Items = categories
            };
            return View(vm);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateCategoryVM vm)
        {
            if (!ModelState.IsValid) return View();

            bool existed = await _context.Categories.AnyAsync(c => c.Name == vm.Name);
            if (existed)
            {
                ModelState.AddModelError("Name", "This category already exists");
                return View();
            }

            Category category = new()
            {
                Name = vm.Name,
            };

            await _context.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id <= 0) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();
            return View(existed);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateCategoryVM vm)
        {
            if(id<=0) return BadRequest();

            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if(existed is null) return NotFound();
            bool result = await _context.Categories.AnyAsync(c => c.Name.ToLower().Trim() == vm.Name.ToLower().Trim());
            if (result)
            {
                ModelState.AddModelError("Name","This category already exists");
                return View(existed);
            }

            existed.Name = vm.Name;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            Category existed = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existed is null) return NotFound();
            _context.Remove(existed);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


    }
}
