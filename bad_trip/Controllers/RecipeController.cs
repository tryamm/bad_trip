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
        public RecipeViewModel Get([FromBody]QueryModel query)
        {
            return _recipeController.GetRecipes(query);
        }

        //GET to /api/recipes/more? p = 1 & n = 5 — get more of recipes per one page(n — amount of needed recipes)

        //GET to /api/recipes/id — get specific recipe by id
        [HttpGet("{id}")]
        public RecipeDTO GetRecipe(string id)
        {
            return _recipeController.GetRecipeById(id);
        }

        //POST to /api/recipes — create recipe

        //DELETE to /api/recipes/id — delete recipe

        //PUT/UPDATE to /api/recipes/id — update recipe
    }
}
