﻿using AutoMapper;
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
        IMongoCollection<Drug> Drugs;
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
            Drugs = database.GetCollection<Drug>("Drugs");
        }

        public RecipeViewModel GetRecipes(QueryModel query)
        {
            var builder = new FilterDefinitionBuilder<Recipe>();
            var filter = builder.Empty;
            if (!string.IsNullOrWhiteSpace(query.Name))
            {
                filter = filter & builder.Regex("Name", new BsonRegularExpression(query.Name));
            }

            var result = Recipes.Find(filter).ToList();

            var recipes = result.Skip((query.PageNumber - 1) * query.PageSize)
                   .Take(query.PageSize)
                   .ToList();
            var patients = Patients.AsQueryable();
            var drugs = Drugs.AsQueryable();

            var recipesToSend = (from r in recipes
                                 join p in patients on r.PatientId equals p.Id
                                 join d in drugs on r.Drug equals d.Id
                                 select new RecipeDTO
                                 {
                                     PatientName = p.Fullname,
                                     PatientMedcardNumber = p.CardNumber,
                                     PatientId = p.Id,
                                     DoctorId = r.DoctorId,
                                     Id = r.Id,
                                     DrugName = d.Name
                                 });
            return new RecipeViewModel
            {
                Recipes = recipesToSend,
                TotalRows = result.Count()
            };
        }

        public async Task UpdateAsync(RecipeDTO recipe)
        {
            var recipeModel = new Recipe
            {
                Id = recipe.Id,
                PatientId = recipe.PatientId,
                DoctorId = recipe.DoctorId,
                Drug = recipe.Drug,
                DateTime = recipe.DateTime,
                Doze = recipe.Doze,
                Recommendation = recipe.Recommendation
            };
            await Recipes.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(recipe.Id)), recipeModel);
        }

        public async Task DeleteAsync(string id)
        {
            await Recipes.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }

        public List<RecipeDTO> GetRecipeByPatientPhone(string phone)
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Regex("Phone", new BsonRegularExpression(phone));

            var patients = Patients.Find(filter).ToList();
            var recipes = Recipes.AsQueryable().ToList();
            var drugs = Drugs.AsQueryable().ToList();

            return (from r in recipes
                    join p in patients on r.PatientId equals p.Id
                    join d in drugs on r.Drug equals d.Id
                    select new RecipeDTO
                    {
                        PatientName = p.Fullname,
                        PatientAge = p.Age,
                        PatientPhone = p.Phone,
                        PatientMedcardNumber = p.CardNumber,
                        PatientId = p.Id,
                        DoctorId = r.DoctorId,
                        Id = r.Id,
                        DrugName = d.Name,
                        Doze = r.Doze,
                        Recommendation = r.Recommendation
                    }).ToList();
        }

        public async Task CreateRecipeAsync(RecipeDTO recipe)
        {
            var builder = new FilterDefinitionBuilder<Drug>();
            var filter = builder.Regex("Name", new BsonRegularExpression(recipe.DrugName));
            var drug = Drugs.Find(filter).FirstOrDefault();
            if (recipe.PatientId == null && recipe.PatientPhone != null)
            {
                var build = new FilterDefinitionBuilder<Patient>();
                var filtering = build.Regex("Phone", new BsonRegularExpression(recipe.PatientPhone));
                var patient = Patients.Find(filtering).FirstOrDefault();
                recipe.PatientId = patient.Id;
            }
            if (drug != null)
            {
                var recipeModel = new Recipe
                {
                    PatientId = recipe.PatientId,
                    DoctorId = recipe.DoctorId,
                    Drug = drug.Id,
                    DateTime = DateTime.Now,
                    Doze = recipe.Doze,
                    Recommendation = recipe.Recommendation
                };
                await Recipes.InsertOneAsync(recipeModel);
            }
            else
            {
                throw new Exception("Error while creating recipe: drug not found.");
            }
        }

        public RecipeDTO GetRecipeById(string id)
        {
            var recipes = Recipes.Find(new BsonDocument("_id", new ObjectId(id))).ToList();
            var patients = Patients.AsQueryable();
            var drugs = Drugs.AsQueryable();

            return (from r in recipes
                    join p in patients on r.PatientId equals p.Id
                    join d in drugs on r.Drug equals d.Id
                    select new RecipeDTO
                    {
                        PatientName = p.Fullname,
                        PatientAge = p.Age,
                        PatientPhone = p.Phone,
                        PatientMedcardNumber = p.CardNumber,
                        PatientId = p.Id,
                        DoctorId = r.DoctorId,
                        Id = r.Id,
                        DrugName = d.Name,
                        Doze = r.Doze,
                        Recommendation = r.Recommendation
                    }).FirstOrDefault();
        }
    }
}
