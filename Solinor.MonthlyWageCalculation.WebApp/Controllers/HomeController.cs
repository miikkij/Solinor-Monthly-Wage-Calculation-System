namespace Solinor.MonthlyWageCalculation.WebApp.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Solinor.MonthlyWageCalculation.WebApp.Repository;

    public class HomeController : Controller
    {
        public HomeController(IWageRepository wageRepository)
        {
            this.WageRepository = wageRepository;
        }

        public IWageRepository WageRepository { get; set; }

        public IActionResult Index()
        {
            ViewData["PersonnelWages"] = this.WageRepository.GetPersonnel();
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Solinor monthly wage calculation system.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
