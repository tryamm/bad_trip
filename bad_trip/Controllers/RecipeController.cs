using BusinessLayer;
using BusinessLayer.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        //[AllowAnonymous]
        //[HttpGet]
        //public IActionResult Test()
        //{
        //    return Ok("success");
        //}

        [HttpGet]
        public RecipeViewModel Get([FromQuery]QueryModel query)
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

        //DELETE to /api/recipes/id — delete recipe

        //PUT/UPDATE to /api/recipes/id — update recipe
    }
}
