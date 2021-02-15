﻿using System;
namespace CoOwnershipManager.Data
{
    public class Apartment
    {
        public Apartment()
        {
        }

        public int Id { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public string Description { get; set; }

        public int BuildingId { get; set; }
        public Building Building { get; set; }
    }
}
