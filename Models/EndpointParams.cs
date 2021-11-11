using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Models
{
    public class EndpointParams
    {
        public List<Param> epParam;
        public EndpointParams(List<Param> values)
        {
            epParam = values;
        }
    }

    public class Param
    {
        public object key { get; set; }
        public object value { get; set; }
    }
}
