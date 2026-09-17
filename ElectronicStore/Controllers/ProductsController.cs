using ElectronicStore.Models;
using ElectronicStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

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
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please upload an image file.");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }


            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(productDto.ImageFile.FileName);

            string imagefullpath = Path.Combine(Environment.WebRootPath, "images", newFileName);
            using (var stream = System.IO.File.Create(imagefullpath))
            {
                productDto.ImageFile.CopyTo(stream);
            }

            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description ?? "",
                ImageUrl = newFileName,
                CreatedAt = DateTime.Now
            };

            context.Products.Add(product);
            context.SaveChanges();

            return RedirectToAction("Index", "Products");

        }

        public IActionResult Edit(int id)
        {
            var product = context.Products.Find(id);

            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            var productDto = new ProductDto()
            {
                Name = product.Name,
                Brand = product.Brand,
                Category = product.Category,
                Price = product.Price,
                Description = product.Description,
            };


            ViewData["ProductId"] = product.Id;
            ViewData["ImageUrl"] = product.ImageUrl;
            ViewData["CreatedAt"] = product.CreatedAt;

            return View(productDto);
        }
    }
}
