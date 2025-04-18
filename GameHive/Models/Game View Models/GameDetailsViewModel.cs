﻿using Microsoft.AspNetCore.Identity;

namespace GameHive.Models
{
    public class GameDetailsViewModel
    {
        public int Id { get; set; }
        public Game Game { get; set; }
        public List<Tag> Tags { get; set; }
        public List<string> Images { get; set; }
        public double AverageRating { get; set; }
        public double? UserRating { get; set; }
        public IdentityUser Publisher { get; set; }
    }
}
