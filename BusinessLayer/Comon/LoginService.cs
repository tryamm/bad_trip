﻿using BusinessLayer.DTO;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Comon
{
    public class LoginService
    {
        IMongoCollection<UserModel> Users;
        public LoginService()
        {
            string connectionString = "mongodb+srv://admin:qwerty_1@badtripcluster.cepoo.mongodb.net/drugstore";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            Users = database.GetCollection<UserModel>("Users");
        }
        public async Task<UserModel> AuthenticateUser(UserModel user)
        {
            var builder = new FilterDefinitionBuilder<UserModel>();
            var filter = builder.Empty;
            filter = filter & builder.Regex("Username", new BsonRegularExpression(user.Username));
            filter = filter & builder.Regex("Password", new BsonRegularExpression(user.Password));
            return await Users.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<UserModel> GetUserById(string id)
        {
            return await Users.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
    }
}
