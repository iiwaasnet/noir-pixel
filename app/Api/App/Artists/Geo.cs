﻿using Api.App.Entities;

namespace Api.App.Artists
{
    public class Geo : Entity
    {
        public string CountryCode { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}