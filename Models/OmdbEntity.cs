using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Models
{
    public class OmdbEntity: OmdbDto
    {        
        //public ApiResponseEntity Details { get; set; }OmdbEntity
        //public LogDetails  LogDetails { get; set; }
        public object Doc { get; set; }
    }

    public class OmdbDto
    {
        [BsonId] public ObjectId _id { get; set; }
        [BsonElement("search_token")] public string search_token { get; set; }
        [BsonElement("imdbID")] public string imdbID { get; set; }
        [BsonElement("processing_time_ms")] public long processing_time_ms { get; set; }
        [BsonElement("timestamp")] public DateTime timestamp { get; set; } = DateTime.UtcNow;
        [BsonElement("ip_address")] public string ip_address { get; set; }
    }

    public class RequestStat
    {
        [BsonElement("_id")]
        public DateTime Timestamp { get; set; }
        public int Count { get; set; }
    } 
    
    
    public class UsageReport
    {
        public DateTime Day { get; set; }
        public List<int> Count { get; set; }
    }

}
