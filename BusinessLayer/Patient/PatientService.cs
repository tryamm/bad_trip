using AutoMapper;
using BusinessLayer.DTO;
using DataLayer.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class PatientService
    {
        IMongoCollection<Patient> Patients;
        private readonly IMapper _mapper;

        public PatientService(IMapper mapper)
        {
            _mapper = mapper;

            string connectionString = "mongodb+srv://admin:qwerty_1@badtripcluster.cepoo.mongodb.net/drugstore";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            Patients = database.GetCollection<Patient>("Patients");
        }
        public async Task<PatientDTO> GetPatientByPhone(string phone)
        {
            var builder = new FilterDefinitionBuilder<Patient>();
            var filter = builder.Regex("Phone", new BsonRegularExpression(phone));

            return _mapper.Map<PatientDTO>(await Patients.Find(filter).FirstOrDefaultAsync());
        }
    }
}
