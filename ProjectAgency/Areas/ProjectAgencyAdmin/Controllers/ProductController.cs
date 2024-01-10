using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectAgency.Areas.ProjectAgencyAdmin.Models.Utilities.Enums;
using ProjectAgency.Areas.ProjectAgencyAdmin.Models.Utilities.Extension;
using ProjectAgency.Areas.ProjectAgencyAdmin.ViewModels;
using ProjectAgency.DAL;
using ProjectAgency.Models;

namespace ProjectAgency.Areas.ProjectAgencyAdmin.Controllers
{
    [Area("ProjectAgencyAdmin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index(int page)
        {
            double count = await _context.Products.CountAsync();
            List<Product>? products = await _context.Products.Skip(page* 3).Take(3).Include(c => c.Category).ToListAsync();
            PaginationVM<Product> vm = new()
            {
                CurrentPage = page + 1,
                TotalPage = Math.Ceiling(count/3),
                Items = products
            };
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            CreateProductVM vm = new()
            {
                Categories = await GetCategoriesAsync(),
            };


            return View(vm);
        }

        [HttpPost]

        public async Task<IActionResult> Create(CreateProductVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategoriesAsync();
                return View(vm);
            }

            bool checkCatId = await _context.Categories.AnyAsync(c=>c.Id==vm.CategoryId);
            if(!checkCatId)
            {
                vm.Categories = await GetCategoriesAsync();
                ModelState.AddModelError("CatId", "This category is not exists");
                return View(vm);
            }

            if (!vm.Photo.IsValidType(FileType.Image))
            {
                vm.Categories = await GetCategoriesAsync();
                ModelState.AddModelError("Photo", "Photo type should be image type");
                return View(vm);
            }

            if (!vm.Photo.IsValidSize(5, FileSize.Megabite))
            {
                vm.Categories = await GetCategoriesAsync();
                ModelState.AddModelError("Photo", "Photo size can be less or equal 5mb");
                return View(vm);
            }

            Product newProduct = new()
            {
                Name = vm.Name,
                Description = vm.Desctiption,
                CategoryId = vm.CategoryId,
                ImageUrl = await vm.Photo.CreateAsync(_env.WebRootPath, "assets", "img")
            };

            await _context.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Product product = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id); 
            UpdateProductVM vm = new()
            {
                Categories = await GetCategoriesAsync(),
                CategoryId = product.CategoryId,
                Name = product.Name,
                Description = product.Description,
                Url = product.ImageUrl
            };

            return View(vm);

        }

        [HttpPost]

        public async Task<IActionResult> Update(int id,UpdateProductVM vm)
        {
            if (id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                vm.Categories = await GetCategoriesAsync();
                return View(vm);
            }

            bool categoryCheck = await _context.Products.AnyAsync(p=>p.CategoryId==vm.CategoryId);
            if(!categoryCheck)
            {
                vm.Categories = await GetCategoriesAsync();
                ModelState.AddModelError("CatId", "This category is not exists");
                return View(vm);
            }
            Product existed = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p=>p.Id == id);

            if (existed is null) return NotFound();

            if(existed is not null)
            {
                if (!vm.Photo.IsValidType(FileType.Image))
                {
                    vm.Categories = await GetCategoriesAsync();
                    ModelState.AddModelError("Photo", "Photo should be image type");
                    return View(vm);
                }

                if (!vm.Photo.IsValidSize(5, FileSize.Megabite))
                {
                    vm.Categories = await GetCategoriesAsync();
                    ModelState.AddModelError("Photo", "Photo's size can be less or equal 5mb");
                    return View(vm);
                }
                existed.ImageUrl.Delete(_env.WebRootPath, "assets", "img");
                existed.ImageUrl = await vm.Photo.CreateAsync(_env.WebRootPath, "assets", "img");
                               
            }

            existed.Name = vm.Name;
            existed.Description = vm.Description;
            existed.CategoryId = vm.CategoryId;
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }


        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }


    }
}
