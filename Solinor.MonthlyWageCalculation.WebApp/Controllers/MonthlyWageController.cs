using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Solinor.MonthlyWageCalculation.WebApp.Repository;
using Solinor.MonthlyWageCalculation.WebApp.ViewModels;

namespace Solinor.MonthlyWageCalculation.WebApp.Controllers
{
    [Route("api/[controller]")]
    public class MonthlyWageController : Controller
    {
        public MonthlyWageController(IWageRepository wageRepository)
        {
            this.WageRepository = wageRepository;
        }
        public IWageRepository WageRepository { get; set; }

        [HttpGet("person/{id}", Name = "GetPerson")]
        public IActionResult GetPerson(string id)
        {
            var item = WageRepository.GetPerson(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpGet("person")]
        public IEnumerable<PersonViewModel> GetPersonnel()
        {
            return WageRepository.GetPersonnel();
        }        
    }
}

