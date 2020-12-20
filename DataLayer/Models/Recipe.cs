using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Recipe
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public string Drug { get; set; }
        public DateTime DateTime { get; set; }
        public double Doze { get; set; }
        public string Recommendation { get; set; }
    }
}
