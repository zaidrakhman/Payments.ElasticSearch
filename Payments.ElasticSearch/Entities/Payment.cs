﻿using System;

namespace Payments.ElasticSearch.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
