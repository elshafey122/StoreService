using Microsoft.AspNetCore.Mvc;
using Store.Core.Repositories.Contract;
using Store.Core.Entities.Order_Aggregate;
using Store.Core.Specifications.Order_Specs;
namespace AdminDashboard.Controllers
{

    public class OrderController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
                this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {
            var spec = new OrderSpecifications();
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserOrders(string buyerEmail)
        {
            var spec =new OrderSpecifications(buyerEmail);
            var orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return View("Index", orders);
        }
    }
}
