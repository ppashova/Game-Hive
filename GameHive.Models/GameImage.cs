﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.Models
{
    public class GameImage
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        public string imageURL { get; set; }
    }
}
