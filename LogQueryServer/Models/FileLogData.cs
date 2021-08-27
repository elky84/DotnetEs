using Nest;
using System;
using System.Collections.Generic;

namespace LogQueryServer.Models
{
    public class FileLogData
    {
        public int _id { get; set; }

        [Text(Name = "@timestamp")]
        public DateTime DateTime { get; set; }

        public long? AccountDbId { get; set; }

        public int? UserDbId { get; set; }

        [Text(Name = "level")]
        public string Level { get; set; }

        public string Description { get; set; }

        public string Function { get; set; }

        public Dictionary<string, string> Variables { get; set; } = new();

        [Text(Name = "messageBody")]
        public string Message { get; set; }
    }
}
