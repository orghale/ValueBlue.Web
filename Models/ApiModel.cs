using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ValueBlue.Web.Models
{
    public class ApiModel
    {
        public bool Status { get; set; }
        public object ResponseObject { get; set; }
        public object RequestObject { get; set; }
        public object Message { get; set; }
        public long ResponseTime { get; set; }

    }

}
