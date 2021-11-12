using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Domain
{
    public class ConstMessage//ConstMessage.NOT_FOUND
    {
        public const string TIME_OUT_MONGO_ERROR = "A timeout occurred while opening a connection to File database";
        public const string UNKNOWN_MONGO_ERROR = "Unknown error occurred while processing documents";
        public const string BAD_REQUEST = "Invalid request: {0} cannot be null";
        public const string NOT_FOUND = "The movie(s) you are looking for could not be found";
        public const string INTERNAL_ERROR = "System encountered error while processing your request";
        public const string OPERATION_SUCCESS = "Movie with title [{0}] was successfully deleted";
    }
}
