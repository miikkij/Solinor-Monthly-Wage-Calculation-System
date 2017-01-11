using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Solinor.MonthlyWageCalculation.WebApp.ViewModels;
using Solinor.MonthlyWageCalculation.WebApp.Repository;

namespace Solinor.MonthlyWageCalculation.WebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController (IWageRepository wageRepository)
        {
            this.WageRepository = wageRepository;
        }

        public IWageRepository WageRepository { get; set; }

        public IActionResult Index()
        {
            /*ViewData["MonthlyWages"] = this.WageRepository.GetPersonnelWages().MonthlyWages;
            var person = this.WageRepository.GetPerson(this.WageRepository.GetWagesByPersonId("1").FirstOrDefault().personId.ToString());
            ViewData["MVP"] = new MVP()
            {
                Id = person.Id.ToString(),
                Name = person.Name,
                Pay = this.WageRepository.GetWagesByPersonId("1").FirstOrDefault().TotalPay
            };*/
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
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }

    public class MVP 
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Pay { get; set; }
    }
}
