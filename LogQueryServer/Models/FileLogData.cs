using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LogQueryServer.Models
{
    public class FileLogData
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public long AccountDbId { get; set; }

        public int UserDbId { get; set; }

        public string LogLevel { get; set; }

        public string Description { get; set; }

        public string Fuction { get; set; }

        public Dictionary<string, string> Variables { get; set; } = new();
    }
}
