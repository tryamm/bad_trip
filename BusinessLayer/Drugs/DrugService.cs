using AutoMapper;
using BusinessLayer.DTO;
using DataLayer.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Drugs
{
    public class DrugService
    {
        IMongoCollection<Drug> Drugs; // коллекция в базе данных
        private readonly IMapper _mapper;

        public DrugService(IMapper mapper)
        {
            _mapper = mapper;

            string connectionString = "mongodb+srv://admin:qwerty_1@badtripcluster.cepoo.mongodb.net/drugstore";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            Drugs = database.GetCollection<Drug>("Drugs");
        }
        
        public async Task<IEnumerable<DrugDTO>> GetProductsAsync(string name)
        {
            var builder = new FilterDefinitionBuilder<Drug>();
            var filter = builder.Empty; 
            if (!string.IsNullOrWhiteSpace(name))
            {
                filter = filter & builder.Regex("Name", new BsonRegularExpression(name));
            }

            var drugs = await Drugs.Find(filter).ToListAsync();

            return _mapper.Map<List<Drug>, List<DrugDTO>>(drugs);
        }

        public async Task<Drug> GetProductAsync(string id)
        {
            return await Drugs.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        
        public async Task CreateAsync(Drug p)
        {
            await Drugs.InsertOneAsync(p);
        }
        
        public async Task UpdateAsync(Drug p)
        {
            await Drugs.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
        }
        
        public async Task DeleteAsync(string id)
        {
            await Drugs.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }


        // получение изображения
        //public async Task<byte[]> GetImage(string id)
        //{
        //    return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        //}
        //// сохранение изображения
        //public async Task StoreImage(string id, Stream imageStream, string imageName)
        //{
        //    Product p = await GetProduct(id);
        //    if (p.HasImage())
        //    {
        //        // если ранее уже была прикреплена картинка, удаляем ее
        //        await gridFS.DeleteAsync(new ObjectId(p.ImageId));
        //    }
        //    // сохраняем изображение
        //    ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
        //    // обновляем данные по документу
        //    p.ImageId = imageId.ToString();
        //    var filter = Builders<Product>.Filter.Eq("_id", new ObjectId(p.Id));
        //    var update = Builders<Product>.Update.Set("ImageId", p.ImageId);
        //    await Products.UpdateOneAsync(filter, update);
        //}
    }
}
