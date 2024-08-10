using Microsoft.AspNetCore.Mvc;
using Store.Core.Entities;
using Store.Core.Repositories.Contract;
using System.Threading.Tasks;

namespace AdminDashboard.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public BrandController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var Brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return View(Brands);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductBrand brand)
        {
            try
            {
                await unitOfWork.Repository<ProductBrand>().AddAsync(brand);
                await unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (System.Exception)
            {
                ModelState.AddModelError("Name", "Please Enter A New Brand");
                return View(brand);
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await unitOfWork.Repository<ProductBrand>().GetByIdAsync(id);
            unitOfWork.Repository<ProductBrand>().Delete(brand);
            await unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
