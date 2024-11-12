using CodeFirst.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly AdventureWorks adventureWorks;
        IEnumerable<string> enumerableApproach = new List<string> { "SC", "EM" };

        public PersonController()
        {
            this.adventureWorks = new AdventureWorks();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Enumerable Approach

            var enumerableApproachData = this.adventureWorks.Person.Where(x => enumerableApproach.Contains(x.PersonType)); // OPENJSON

            // List Approach
            IEnumerable<string> listApproach = new List<string> { "SC", "EM" };
            
            var listApproachData = this.adventureWorks.Person.Where(x => listApproach.ToList().Contains(x.PersonType)); // OPENJSON

            List<string> listApproach2 = listApproach.ToList();

            var listApproachData2 = this.adventureWorks.Person.Where(x => listApproach2.Contains(x.PersonType)); // OPENJSON

            List<string> listApproach3 = new List<string> { "SC", "EM" };
            var listApproachData3 = this.adventureWorks.Person.Where(x => listApproach3.Contains(x.PersonType)); //OPENJSON

            var listApproachData4 = this.adventureWorks.Person.Where(x => new List<string> { "SC", "EM" }.Contains(x.PersonType)); //IN


            var listApproachData5 = this.adventureWorks.Person.Where(x => GetData().Contains(x.PersonType)); //IN


            var arrayApproachData = this.adventureWorks.Person.Where(x => listApproach.ToArray().Contains(x.PersonType)); // OPENJSON


            return Ok(new List<string> { "Test"});
        }

        private List<string>  GetData ()
        {
            var list =  new List<string> {};

            foreach (var item in enumerableApproach)
                list.Add(item);

            return list;
        }
    }
}
