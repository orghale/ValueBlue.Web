using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NLog;
using System;
using System.Threading.Tasks;
using ValueBlue.Web.BusinessLogic.Interface;
using ValueBlue.Web.Common;
using ValueBlue.Web.Models;

namespace ValueBlue.Web.BusinessLogic
{
    public class MongoRepo : IMongoRepo
    {
        private readonly MongoClient client;
        private readonly IMongoDatabase db;
        private readonly IMongoCollection<OmdbEntity> dbCollection;
        private readonly AppSettings _config;
        private static Logger log = LogManager.GetCurrentClassLogger();

        public MongoRepo(IOptions<AppSettings> config)
        {
            this._config = config.Value;
            client = new MongoClient($"mongodb://{_config.mongo.User}:{_config.mongo.Secret}@{_config.mongo.Ip}:{_config.mongo.Port}/MgDocumentsdb?authSource=admin");
            db = client.GetDatabase("MgDocumentsdb");
            dbCollection = db.GetCollection<OmdbEntity>("MgDocuments");
        }

        public async Task<CommonResObj> Add(OmdbEntity request, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                await dbCollection.InsertOneAsync(request);
                result.Status = true;
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Save document(s) error:: processId: {processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }

        public async Task<CommonResObj> Get(string id, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                var filter = Builders<OmdbEntity>.Filter;
                var eqFilter = filter.Eq(x => x.omdbID, id);

                var file = await dbCollection.FindAsync<OmdbEntity>(eqFilter);
                result.Status = true;
                result.ResponseObject = file.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Get document error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }

        public async Task<CommonResObj> Delete(string id, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                var filter = Builders<OmdbEntity>.Filter;
                var eqFilter = filter.Eq(x => x.omdbID, id);

                var res = await dbCollection.DeleteOneAsync(eqFilter);
                result.Status = res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Delete document error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }

        public async Task<CommonResObj> Update(OmdbEntity request, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                var eqfilter = Builders<OmdbEntity>.Filter;
                var eqFilterDefinition = eqfilter.Eq(x => x.Title, request.Title);
                var updateFilter = Builders<OmdbEntity>.Update;
                var updateFilterDefinition = updateFilter.Set(x =>
                    x.doc, request.doc
                );

                var res = await dbCollection.UpdateOneAsync(eqFilterDefinition, updateFilterDefinition);
                result.Status = res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Update document error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }

    }
}
