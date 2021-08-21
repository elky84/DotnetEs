using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace LogQueryServer.Models
{
    public class Pageable
    {
        public int From { get; set; }

        public int Size { get; set; } = 100;
    }
}
