using System.Collections.Generic;

namespace LogQueryServer.Protocols.Request
{
    public class Query
    {
        public Dictionary<string, string> Queries { get; set; } = new();
    }
}
