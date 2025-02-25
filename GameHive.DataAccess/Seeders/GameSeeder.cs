using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace GameHive.DataAccess.Seeders
{
    public static class GameSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if(!context.Games.Any())
            {
                context.Games.AddRange(new List<Game>
                {
                    new Game { Name = "The Binding Of Isaac",
                               Price = decimal.Zero,
                               BriefDescription = "A roguelike dungeon crawler",
                               FullDescription = "The Binding of Isaac is a randomly generated action RPG shooter with heavy roguelike elements. Following Isaac on his journey players will find bizarre treasures that change Isaac’s form giving him super human abilities and enabling him to fight off droves of mysterious creatures, discover secrets and fight his way to safety.",
                               GameIconUrl = "https://cdn2.steamgriddb.com/icon/bc573864331a9e42e4511de6f678aa83/24/256x256.png"
                    },

                    new Game { Name = "Hades",
                               Price = 24.99m,
                               BriefDescription = "A roguelike dungeon crawler",
                               FullDescription = "Hades is a god-like rogue-like dungeon crawler that combines the best aspects of Supergiant's critically acclaimed titles, including the fast-paced action of Bastion, the rich atmosphere and depth of Transistor, and the character-driven storytelling of Pyre.",
                               GameIconUrl = "https://cdn2.steamgriddb.com/icon/851300ee84c2b80ed40f51ed26d866fc/32/256x256.png"
                    },
                    new Game {
                               Name = "Resident Evil 8: Village",
                               Price = 59.99m,
                               BriefDescription = "A survival horror game",
                               FullDescription = "Experience survival horror like never before in the eighth major installment in the storied Resident Evil franchise - Resident Evil Village. Set a few years after the horrifying events in the critically acclaimed Resident Evil 7 biohazard.",
                               GameIconUrl = "https://assets-prd.ignimgs.com/2021/01/22/re-village-button-fin-1611277715193.jpg?width=300&crop=1%3A1%2Csmart&auto=webp&dpr=2"
                    },
                    new Game{
                               Name = "Red Dead Redemption 2",
                               Price = 59.99m,
                               BriefDescription = "An action-adventure game",
                               FullDescription = "America, 1899. The end of the wild west era has begun as lawmen hunt down the last remaining outlaw gangs. Those who will not surrender or succumb are killed.",
                               GameIconUrl = "https://cdn2.steamgriddb.com/icon_thumb/2e65f2f2fdaf6c699b223c61b1b5ab89.png"
                    }
                });
                context.SaveChanges();
            }
        }
    }
}
