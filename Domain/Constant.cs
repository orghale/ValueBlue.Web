using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Domain
{
    public class ConstMessage
    {
        public const string TIME_OUT_MONGO_ERROR = "A timeout occurred while opening a connection to File database";
        public const string UNKNOWN_MONGO_ERROR = "Unknown error occurred while processing documents";
    }
}
