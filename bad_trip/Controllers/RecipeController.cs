using BusinessLayer;
using BusinessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace bad_trip.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly RecipeService _recipeController;

        public RecipeController(RecipeService recipeController)
        {
            _recipeController = recipeController;
        }

        [HttpGet]
        public RecipeViewModel Get([FromQuery] QueryModel query)
        {
            return _recipeController.GetRecipes(query);
        }

        [HttpGet("{id}")]
        public RecipeDTO GetRecipe(string id)
        {
            return _recipeController.GetRecipeById(id);
        }

        [AllowAnonymous]
        [HttpGet("phone/{phone}")]
        public List<RecipeDTO> Get(string phone)
        {
            return _recipeController.GetRecipeByPatientPhone(phone);
        }

        [HttpPost]
        public async Task Create([FromBody] RecipeDTO recipe)
        {
            await _recipeController.CreateRecipeAsync(recipe);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _recipeController.DeleteAsync(id);
        }

        [HttpPut]
        public async Task Update([FromBody] RecipeDTO recipe)
        {
            await _recipeController.UpdateAsync(recipe);
        }

        [HttpGet("pdf/{id}")]
        public async Task<IActionResult> GetPdfRecipe(string id)
        {
            try
            {
                return Ok(await _recipeController.GetPdfRecipeAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
