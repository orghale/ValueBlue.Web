using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NLog;
using System;
using System.Linq;
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
            client = new MongoClient($"mongodb://{_config.mongo.User}:{_config.mongo.Secret}@{_config.mongo.Ip}:{_config.mongo.Port}/{_config.mongo.Db}?authSource=admin");
            db = client.GetDatabase($"{_config.mongo.Db}");
            dbCollection = db.GetCollection<OmdbEntity>($"{_config.mongo.Collections}");
        }

        public async Task<CommonResObj> Add(OmdbEntity request, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                //if (! await IsExist(request.search_token, processId))
                await dbCollection.InsertOneAsync(request);
                result.Status = true;
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Save movie(s) error:: processId: {processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }

        public async Task<CommonResObj> GetSingle(string id, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                var filter = Builders<OmdbEntity>.Filter;
                var eqFilter = filter.Eq(x => x.search_token, id.ToUpper().Trim());

                var movie = await dbCollection.FindAsync<OmdbEntity>(eqFilter);
                result.Status = true;
                result.ResponseObject = movie.FirstOrDefault();
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Get movie error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }

        public async Task<CommonResObj> GetAll(string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                FilterDefinitionBuilder<OmdbEntity> filter = Builders<OmdbEntity>.Filter;
                FilterDefinition<OmdbEntity> emptyFilter = filter.Empty;

                IAsyncCursor<OmdbEntity> allMovies = await dbCollection.FindAsync<OmdbEntity>(emptyFilter);
                result.Status = true;
                result.ResponseObjects = allMovies.ToList();
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Get movies error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }


        public async Task<CommonResObj> GetList(string date1, string date2, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                var dt1 = DateTime.ParseExact(date1, "dd-MM-yyyy", null);
                var dt2 = DateTime.ParseExact($"{date2}T23:59:59", "dd-MM-yyyyTHH:mm:ss", null);

                var filter = Builders<OmdbEntity>.Filter;
                var maxNumberFilter = filter.Lte(x => x.timestamp, dt2);
                var minNumberFilter = filter.Gte(x => x.timestamp, dt1);

                var finalFilter = filter.And(minNumberFilter, maxNumberFilter);

                var movies = await dbCollection.FindAsync<OmdbEntity>(finalFilter);

                result.Status = true;
                result.ResponseObjects = movies.ToList();
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Get movies error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }


        public async Task<CommonResObj> GenerateReport(string date, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                DateTime dt1 = DateTime.ParseExact(date, "dd-MM-yyyy", null);
                DateTime dt2 = dt1.AddDays(1);

                var res = dbCollection.Aggregate().Match(r => r.timestamp >= dt1 && r.timestamp < dt2).Unwind<OmdbEntity, OmdbEntity>
                      (x => x.timestamp)
                      .Group(x => x.timestamp, g => new
                      {
                          Count = g.Count()
                      }).As<RequestStat>().ToList();

                var data = res.GroupBy(p => p.Timestamp.Date, p => p.Count, (key, g) => new UsageReport { Day = key, Count = g.ToList() });

                result.usages = data;
                result.Status = true;

            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Generate aggregation error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }


        public async Task<CommonResObj> GenerateReportByTitle(string title, string processId = null)
        {
            CommonResObj result = new CommonResObj();
            try
            {
                var res = dbCollection.Aggregate().Match(r => r.search_token == title.ToUpper().Trim()).Unwind<OmdbEntity, OmdbEntity>
                      (x => x.timestamp)
                      .Group(x => x.search_token, g => new
                      {
                          Count = g.Count()
                      }).As<RequestStatByTitle>().ToList();

                result.TitleRpts = res;
                result.Status = true;

            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Generate aggregation by title error:: processId:{processId} Exception: {ex}");
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
                var eqFilter = filter.Eq(x => x.search_token, id.ToUpper().Trim());

                var res = await dbCollection.DeleteOneAsync(eqFilter);
                result.Status = res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Delete movie error:: processId:{processId} Exception: {ex}");
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
                var eqFilterDefinition = eqfilter.Eq(x => x.search_token, request.search_token.ToUpper().Trim());
                var updateFilter = Builders<OmdbEntity>.Update;
                var updateFilterDefinition = updateFilter.Set(x =>
                    x.Doc, request.Doc
                );

                var res = await dbCollection.UpdateOneAsync(eqFilterDefinition, updateFilterDefinition);
                result.Status = res.IsAcknowledged;
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] Update movie error:: processId:{processId} Exception: {ex}");
                result.Message = new ErrorHanler(ex.Message.ToString()).Message;
                result.Status = false;
            }
            return result;
        }


        public async Task<bool> IsExist(string id, string processId = null)
        {
            bool result = false;
            try
            {
                var filter = Builders<OmdbEntity>.Filter;
                var eqFilter = filter.Eq(x => x.search_token, id.ToUpper().Trim());

                result = (await dbCollection.FindAsync<OmdbEntity>(eqFilter)).Any();
            }
            catch (Exception ex)
            {
                log.Error($"[MongoRepo] IsExist error:: processId:{processId} Exception: {ex}");
            }
            return result;
        }
    }
}
