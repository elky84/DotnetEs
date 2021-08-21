using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LogQueryServer.Models
{
    public class Query
    {
        public Dictionary<string, string> Queries { get; set; } = new();
    }
}
