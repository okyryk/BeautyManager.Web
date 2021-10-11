using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using BeautyManager.Web.Repository.Settings;


namespace BeautyManager.Web.Repository.MongoDB
{
	public class MongoDBBaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		protected readonly IMongoDBContext _mongoContext;

		private readonly IMongoCollection<TEntity> _dbCollection;

		public MongoDBBaseRepository(IDatabaseSettings databaseSettings)
		{
			string _entityPrefix = typeof(TEntity).Name;
			_mongoContext = new MongoDBContext(databaseSettings);
			_dbCollection = _mongoContext.GetCollection<TEntity>(_entityPrefix);
		}

		public MongoDBBaseRepository(IDatabaseSettings databaseSettings, string DatabaseCollectionName)
		{
			_mongoContext = new MongoDBContext(databaseSettings);
			_dbCollection = _mongoContext.GetCollection<TEntity>(DatabaseCollectionName);
		}


		public async Task<TEntity> AddItemAsync(TEntity item)
		{
			var objectId = ObjectId.GenerateNewId();
			item.GetType().GetProperty("Id").SetValue(item, objectId.ToString());
			await _dbCollection.InsertOneAsync(item);
			return item;
		}

		public async Task DeleteItemAsync(string id)
		{
			var objectId = new ObjectId(id);
			_ = await _dbCollection.DeleteOneAsync(Builders<TEntity>.Filter.Eq("Id", objectId));
		}

		public async Task<TEntity> GetItemAsync(string id)
		{
			var objectId = new ObjectId(id);
			FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("Id", objectId);
			return await _dbCollection.FindAsync(filter).Result.FirstOrDefaultAsync();
		}

		public async Task<System.Collections.Generic.IEnumerable<TEntity>> GetItemsAsync(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
		{
			var all = await _dbCollection.FindAsync<TEntity>(predicate);
			return await all.ToListAsync();
		}

		public async Task<IEnumerable<TEntity>> GetItemsAsync()
		{
			var all = await _dbCollection.FindAsync<TEntity>(Builders<TEntity>.Filter.Empty);
			return await all.ToListAsync();
		}

		public async Task<TEntity> UpdateItemAsync(string id, TEntity item)
		{
			var res = await _dbCollection.ReplaceOneAsync(Builders<TEntity>.Filter.Eq("Id", id), item);
			if (res.IsAcknowledged)
			{
				return item;
			}
			else
			{
				return null;
			}
		}
	}
}
