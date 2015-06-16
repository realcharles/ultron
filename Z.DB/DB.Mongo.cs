using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
namespace Z.DB.Mongo
{
    public class Mapper
    {
        #region 属性
        //mongodb://[username:password@]hostname[:port][/[database][?options]]
        public string ConnectionString
        {
            get;
            set;
        }
        public string DBName{get;set;}
        #endregion

        private Mapper(string dbname)
        {
            DBName = dbname;
        }
        public static Mapper Create(string dbname)
        {
            return new Mapper(dbname);
        }
        private MongoClient _client;
        public MongoClient Client
        {
            get {
                if (_client == null)
                    _client = new MongoClient(MongoUrl.Create(ConnectionString));
                return _client;
            }
        }
        private MongoServer _server;
        public MongoServer Server
        {
            get
            {
                if (_server == null)
                    _server = Client.GetServer();
                return _server;
            }
        }
        private MongoDatabase _db;
        public MongoDatabase DB { get { 
            if(_db == null)
            {
                _db = Server.GetDatabase(DBName);
            }
            return _db;
        } }
        public MongoCollection Collection(string name)
        {
            return DB.GetCollection(name);
        }
     
        public MongoCollection<T> Collection<T>()
        {
            return DB.GetCollection<T>(typeof(T).Name);
        }

        
    }
   
}
