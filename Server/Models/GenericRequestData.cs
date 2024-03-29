﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Server.Models
{
    public class GenericRequestData
    {

        public DateTime Date { get; set; } = DateTime.Now;

        public dynamic Content { get; set; }
    }
}
