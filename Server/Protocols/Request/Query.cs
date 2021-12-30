using System.Collections.Generic;

namespace Server.Protocols.Request
{
    public class Query
    {
        public Dictionary<string, string> Queries { get; set; } = new();
    }
}
