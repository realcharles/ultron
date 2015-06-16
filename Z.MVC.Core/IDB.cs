using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Z.MVC.Core
{
    public class IDB:ILog
    {
        private DB.Mongo.Mapper _mapper;
        private DB.Mongo.Mapper Mapper
        {
            get
            {
                if (_mapper == null)
                {
                    _mapper =Z.DB.Mongo.Mapper.Create(G.MongoDBSetting.DBName);
                    _mapper.ConnectionString = G.MongoDBSetting.ConnectionString;
                }

                return _mapper;
            }
        }
        public MongoCollection Collection(string name)
        {
            return Mapper.Collection(name);
        }
        public MongoCollection<T> Collection<T>()
        {
            return Mapper.Collection<T>();
        }
    }
}
