using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Seeders
{
    public static class TagSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.Tags.Any())
            {
                context.Tags.AddRange(new List<Tag>
                {
                    new Tag { Name = "Action" },
                    new Tag { Name = "Beat 'em up" },
                    new Tag { Name = "Fighting" },
                    new Tag { Name = "Platformer" },

                    new Tag { Name = "Shooter" },
                    new Tag { Name = "FPS" },
                    new Tag { Name = "TPS" },
                    new Tag { Name = "Hero Shooter" },
                    new Tag { Name = "Bullet Hell" },
                    new Tag { Name = "Rhythm" },

                    new Tag { Name = "Adventure" },
                    new Tag { Name = "Graphic Adventure" },
                    new Tag { Name = "Narrative / Interactive Story" },
                    new Tag { Name = "Metroidvania" },
                    new Tag { Name = "Puzzle Adventure" },
                    new Tag { Name = "Walking Simulator" },
                    new Tag { Name = "Action-Adventure" },

                    new Tag { Name = "RPG" },
                    new Tag { Name = "Action RPG" },
                    new Tag { Name = "Turn-Based RPG" },
                    new Tag { Name = "Tactical RPG" },
                    new Tag { Name = "Open-World RPG" },
                    new Tag { Name = "Dungeon Crawler" },
                    new Tag { Name = "Monster Tamer" },
                    new Tag { Name = "MMORPG" },

                    new Tag { Name = "Simulation" },
                    new Tag { Name = "Life Simulation" },
                    new Tag { Name = "Vehicle Simulation" },
                    new Tag { Name = "Farming Simulation" },
                    new Tag { Name = "Business / Tycoon" },
                    new Tag { Name = "Visual Novel" },

                    new Tag { Name = "Strategy" },
                    new Tag { Name = "Real-Time Strategy" },
                    new Tag { Name = "Turn-Based Strategy" },
                    new Tag { Name = "4X" },
                    new Tag { Name = "Tower Defense" },
                    new Tag { Name = "Auto Battler" },

                    new Tag {Name = "Sports"},
                    new Tag {Name = "Racing"},
                    new Tag {Name = "Traditional Sports"},
                    new Tag {Name = "Racing Simulation"},
                    new Tag {Name = "Extreme Sports"},
                    new Tag {Name = "Arcade Racing"},

                    new Tag {Name = "Horror"},
                    new Tag {Name = "Survival Horror"},
                    new Tag {Name = "Psychological Horror"},
                    new Tag {Name = "Multiplayer Horror"},
                    new Tag {Name = "Cosmic Horror"},

                    new Tag {Name = "Casual"},
                    new Tag {Name = "Party"},
                    new Tag {Name = "Trivia"},
                    new Tag {Name = "Board Game"},

                    new Tag {Name = "Sandbox"},
                    new Tag {Name = "Open-World"},
                    new Tag {Name = "Creative Sandbox"},

                    new Tag {Name = "Roguelike"},
                    new Tag {Name = "Roguelite"},
                    new Tag {Name = "Classic Roguelike"},
                    new Tag {Name = "Action Roguelite"},
                    new Tag {Name = "Roguelike Deckbuilder"},

                    new Tag {Name = "Multiplayer"},
                    new Tag {Name = "Singleplayer"},
                    new Tag {Name = "Co-op"},
                    new Tag {Name = "Competitive"},
                    new Tag {Name = "Battle Royale"},
                    new Tag {Name = "MOBA"},
                    new Tag {Name = "MMO"},
                    new Tag {Name = "Social Deduction"},

                    new Tag {Name = "First-Person"},
                    new Tag {Name = "Third-Person"},
                    new Tag {Name = "Isometric"},
                    new Tag {Name = "Top-Down"},
                    new Tag {Name = "Side-Scroller"},

                    new Tag {Name = "Fantasy"},
                    new Tag {Name = "Sci-Fi"},
                    new Tag {Name = "Dark Fantasy"},
                    new Tag {Name = "Contemporary Fantasy"},
                    new Tag {Name = "Low Fantasy"},
                    new Tag {Name = "Grimdark"},

                    new Tag {Name = "Mobile Game"},
                    new Tag {Name = "VR"},
                    new Tag {Name = "Playstation"},
                    new Tag {Name = "Xbox"},
                    new Tag {Name = "Nintendo"},
                    new Tag {Name = "PC"},

                    new Tag {Name = "Pixel Art"},
                    new Tag {Name = "Low-Poly"},
                    new Tag {Name = "2D"},
                    new Tag {Name = "3D"},
                    new Tag {Name = "Realistic"},
                    new Tag {Name = "Cartoon"},
                    new Tag {Name = "Anime"},

                    new Tag {Name = "Family-Friendly"},
                    new Tag {Name = "Educational"},
                    new Tag {Name = "Dark & Gritty"},
                    new Tag {Name = "Mature"},
                    new Tag {Name = "NSFW"},
                    new Tag {Name = "Violent"},
                    new Tag {Name = "Gore"},
                    new Tag {Name = "Funny"},
                    new Tag {Name = "Puzzle"}
                });

                context.SaveChanges();
            }
        }
    }
}
