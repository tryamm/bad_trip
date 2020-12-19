using AutoMapper;
using BusinessLayer.DTO;
using DataLayer.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BusinessLayer
{
    public class RecipeService
    {
        IMongoCollection<Recipe> Recipes;
        IMongoCollection<Patient> Patients;
        private readonly IMapper _mapper;

        public RecipeService(IMapper mapper)
        {
            _mapper = mapper;

            string connectionString = "mongodb+srv://admin:qwerty_1@badtripcluster.cepoo.mongodb.net/drugstore";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            Recipes = database.GetCollection<Recipe>("Recipes");
            Patients = database.GetCollection<Patient>("Patients");
        }

        public RecipeViewModel GetRecipes(QueryModel query)
        {
            var builder = new FilterDefinitionBuilder<Recipe>();
            var filter = builder.Empty;
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                filter = filter & builder.Regex("Name", new BsonRegularExpression(query.Name));
            }

            var result = Recipes.AsQueryable().ToList();

            var recipes = result.Skip((query.PageNumber - 1) * query.PageSize)
                   .Take(query.PageSize)
                   .ToList();
            var patients = Patients.AsQueryable();

            var recipesToSend = (from d in recipes
                                 join p in patients on d.PatientId equals p.Id
                                 select new RecipeDTO
                                 {
                                     PatientName = p.Fullname,
                                     PatientMedcardNumber = p.CardNumber,
                                     PatientId = p.Id,
                                     DoctorId = d.DoctorId,
                                     Id = d.Id
                                 });
            return new RecipeViewModel
            {
                Recipes = recipesToSend,
                TotalRows = result.Count()
            };
        }
    }
}
