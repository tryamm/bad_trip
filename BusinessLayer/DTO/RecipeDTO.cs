using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.DTO
{
    public class RecipeViewModel : ViewModel
    {
        public IEnumerable<RecipeDTO> Recipes { get; set; }
    }
    public class RecipeDTO : Recipe
    {
        public string PatientName { get; set; }
        public string PatientMedcardNumber { get; set; }
        public string DrugNames { get; set; }
        public int PatientAge { get; set; }
        public string PatientPhone { get; set; }
    }
}
