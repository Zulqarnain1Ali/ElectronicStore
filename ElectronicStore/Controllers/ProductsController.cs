using ElectronicStore.Models;
using ElectronicStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicStore.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext context)
        {
            this.context = context;
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
            return View();
        }
    }
}
