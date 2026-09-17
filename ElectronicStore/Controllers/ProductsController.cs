using ElectronicStore.Models;
using ElectronicStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStore.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment Environment;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment Environment)
        {
            this.context = context;
            this.Environment = Environment;
        }
        public IActionResult Index()
        {
            var products = context.Products.ToList();
            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ProductDto productDto)
        {
            if(productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please upload an image file.");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            return RedirectToAction("Index", "Products");
        }
    }
}
