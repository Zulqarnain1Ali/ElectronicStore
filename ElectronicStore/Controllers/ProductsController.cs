using ElectronicStore.Models;
using ElectronicStore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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





        public async Task<IActionResult> Index()
        {
            var products = await context.Products.ToListAsync();
            return View(products);
        }




        public IActionResult Create()
        {
            return View();
        }



        //Create
        [HttpPost]
        public async Task<IActionResult> Create(ProductDto productDto)
        {
            if (productDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Please upload an image file.");
            }
            if (!ModelState.IsValid)
            {
                return View(productDto);
            }

            try
            {
                //throw new Exception("Simulated disk/database failure!");
                string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(productDto.ImageFile!.FileName);

                string imagefullpath = Path.Combine(Environment.WebRootPath, "images", newFileName);
                using (var stream = System.IO.File.Create(imagefullpath))
                {
                    await productDto.ImageFile.CopyToAsync(stream);
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
                await context.SaveChangesAsync();

                return RedirectToAction("Index", "Products");
            }

            catch(Exception ex)
            {
                ModelState.AddModelError("", "Can't save the product: " + ex.Message);
                return View(productDto);
            }
           

        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await context.Products.FindAsync(id);

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


        [HttpPost]
        public async Task<IActionResult> Edit(int id, ProductDto productDto)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            if (!ModelState.IsValid)
            {
                ViewData["ProductId"] = product.Id;
                ViewData["ImageUrl"] = product.ImageUrl;
                ViewData["CreatedAt"] = product.CreatedAt;

                return View(productDto);
            }

            try
            {
                //throw new Exception("Simulated disk/database failure!");

                string newFileName = product.ImageUrl;
                if (productDto.ImageFile != null)
                {
                    newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                    newFileName += Path.GetExtension(productDto.ImageFile.FileName);

                    //string imageFullPath = Environment.WebRootPath + "images" + newFileName;
                    string imageFullPath = Path.Combine(Environment.WebRootPath, "images", newFileName);
                    using (var stream = System.IO.File.Create(imageFullPath))
                    {
                        await productDto.ImageFile.CopyToAsync(stream);
                    }

                    //delete old image
                    if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl != "1.jpg")
                    {
                        string oldImageFilePath = Path.Combine(Environment.WebRootPath, "images", product.ImageUrl);
                        //string oldImageFilePath = Environment.WebRootPath + "images" + product.ImageUrl;
                        if (System.IO.File.Exists(oldImageFilePath))
                        {
                            System.IO.File.Delete(oldImageFilePath);
                        }
                    }

                }
                product.Name = productDto.Name;
                product.Brand = productDto.Brand;
                product.Category = productDto.Category;
                product.Price = productDto.Price;
                product.Description = productDto.Description;
                product.ImageUrl = newFileName;

                await context.SaveChangesAsync();

                return RedirectToAction("Index", "Products");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Unable to edit the product: " + ex.Message);

                ViewData["ProductId"] = product.Id;
                ViewData["ImageUrl"] = product.ImageUrl;
                ViewData["CreatedAt"] = product.CreatedAt;

                return View(productDto);
                
            }

            
        }



        public async Task<IActionResult> Delete(int id)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction("Index", "Products");
            }

            string imageFullPath = Path.Combine(Environment.WebRootPath, "images", product.ImageUrl);
            //string imageFullPath = Environment.WebRootPath + "images" + product.ImageUrl;
            System.IO.File.Delete(imageFullPath);

            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Products");
        }
    }
}
