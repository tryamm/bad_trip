using BusinessLayer.Drugs;
using BusinessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace bad_trip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrugController : ControllerBase
    {
        private readonly DrugService _drugService;

        public DrugController(DrugService drugService)
        {
            _drugService = drugService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<DrugDTO>> Get()
        {
            return await _drugService.GetProductsAsync(string.Empty);
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public async Task Post([FromBody] DrugDTO drug)
        {
            await _drugService.CreateAsync(drug);
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
