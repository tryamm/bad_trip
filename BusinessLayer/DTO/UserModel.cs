using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.DTO
{
    public class UserModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string OrgName { get; set; }
        public string Address { get; set; }
        public string EDRPOU { get; set; }

    }
}
