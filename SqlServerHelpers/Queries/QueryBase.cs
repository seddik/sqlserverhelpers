using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SqlServerHelpers.Queries
{
    public abstract class QueryBase
    {

        public Dictionary<string, string> Parameters { get; set; }

        public QueryBase(Dictionary<string, string> param)
        {
            Parameters = param;
        }
        public QueryBase()
        {
            Parameters = new Dictionary<string, string>();
        }

        public bool ExecuteCheck(string connection)
        {
            return true;
        }
        public string ExecuteScalar(string connection)
        {
            return "";
        }
        public List<string> ExecuteList(string connection)
        {
            return new List<string>();
        }
    }
}
