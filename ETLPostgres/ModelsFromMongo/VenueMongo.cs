﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETLPostgres.ModelsFromMongo
{
    internal class VenueMongo
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? City { get; set; }
    }
}
