using BlogSiteApplication.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BlogSiteApplication.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _users;

        public UserRepository(IMongoClient mongoClient)
        {
            var database = mongoClient.GetDatabase("BlogSiteDB");
            _users = database.GetCollection<User>("Users");
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _users.InsertOneAsync(user);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while adding a user", ex);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _users.Find(user => user.UserEmail == email).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while retrieving the user by email", ex);
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            try
            {
                var objectid = new ObjectId(id);
                return await _users.Find(user => user.ObjectID == objectid).FirstOrDefaultAsync();
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while retrieving the user by ID", ex);
            }
        }
    }
}
