using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Tutorial.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _dbContext;

        public UserRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Guid Add(UserDocument document)
        {
            _dbContext.Users.InsertOne(document);

            return document.Id;
        }

        public async Task<Guid> AddAsync(UserDocument document)
        {
            await _dbContext.Users.InsertOneAsync(document);

            return document.Id;
        }

        public UserDocument Get(Guid userId)
        {
            var cursor = _dbContext.Users.Find(x => x.Id == userId);

            return cursor.Single();
        }

        public async Task<UserDocument> GetAsync(Guid userId)
        {
            var cursor = await _dbContext.Users.FindAsync(x => x.Id == userId);

            return await cursor.SingleOrDefaultAsync();
        }

        public IList<UserDocument> GetAll()
        {
            var filter = Builders<UserDocument>.Filter.Empty;
            var cursor = _dbContext.Users.Find(filter);

            return cursor.ToList();
        }

        public async Task<IList<UserDocument>> GetAllAsync()
        {
            var filter = Builders<UserDocument>.Filter.Empty;
            var cursor = await _dbContext.Users.FindAsync(filter);

            return await cursor.ToListAsync();
        }

        public void Upsert(UserDocument document)
        {
            var options = new FindOneAndReplaceOptions<UserDocument, UserDocument>
            {
                IsUpsert = true
            };

            _dbContext.Users.FindOneAndReplace<UserDocument>(d => d.Id == document.Id, document, options);
        }

        public async Task UpsertAsync(UserDocument document)
        {
            var options = new FindOneAndReplaceOptions<UserDocument, UserDocument>
            {
                IsUpsert = true
            };

            await _dbContext.Users.FindOneAndReplaceAsync<UserDocument>(d => d.Id == document.Id, document, options);
        }

        public void Delete(Guid userId)
        {
            _dbContext.Users.DeleteOne(u => u.Id == userId);
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _dbContext.Users.DeleteOneAsync(u => u.Id == userId);
        }
    }
}
