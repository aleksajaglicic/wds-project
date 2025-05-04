namespace Trainer.Server.Data
{
    using MongoDB.Driver;
    
    public class MongoDbService
    {
        #region Props
        private readonly IConfiguration _configuration;
        public IMongoDatabase? Database => _database;
        private readonly IMongoDatabase? _database;
        #endregion

        #region Constructor
        public MongoDbService(IConfiguration configuration)
        {
            _configuration = configuration;

            var connectionString = _configuration.GetConnectionString("DbConnection");
            var mongoUrl = MongoUrl.Create(connectionString);
            var mongoClient = new MongoClient(mongoUrl);
            _database = mongoClient.GetDatabase(mongoUrl.DatabaseName);
        }
        #endregion

    }
}
