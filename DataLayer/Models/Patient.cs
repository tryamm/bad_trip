using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Models
{
    public class Patient
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string CardNumber { get; set; }
    }
}
