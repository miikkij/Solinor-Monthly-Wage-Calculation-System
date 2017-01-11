using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Solinor.MonthlyWageCalculation.Models;
using MvcApp.Repository;

namespace MvcApp.Controllers
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
        public IEnumerable<Person> GetPersonnel()
        {
            return WageRepository.GetPersonnel();
        }

        [HttpGet("personnelwages")]
        public PersonnelWages GetPersonnelWages()
        {
            return WageRepository.GetPersonnelWages();
        }

        [HttpGet("personnelwages/{id}", Name = "GetWagesByPersonId")]
        public IActionResult GetPersonnelWagesForPerson(string id)
        {
            var item = WageRepository.GetWagesByPersonId(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        
    }
}

