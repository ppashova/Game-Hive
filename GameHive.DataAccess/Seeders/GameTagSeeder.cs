using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Seeders
{
    public static class GameTagSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.GameTags.Any()) return;

            var TBOI = context.Games.Where(g => g.Name == "The Binding Of Isaac").Select(g => g.GameId).FirstOrDefault();
            var Hades = context.Games.Where(g => g.Name == "Hades").Select(g => g.GameId).FirstOrDefault();
            var RE8 = context.Games.Where(g => g.Name == "Resident Evil 8: Village").Select(g => g.GameId).FirstOrDefault();
            var RDR2 = context.Games.Where(g => g.Name == "Red Dead Redemption 2").Select(g=>g.GameId).FirstOrDefault();

            var RogueLikeTag = context.Tags.Where(t => t.Name == "Roguelike").Select(t => t.Id).FirstOrDefault();
            var Action_AdventureTag = context.Tags.Where(t => t.Name == "Action-Adventure").Select(t => t.Id).FirstOrDefault(); 
            var SurvivalHorrorTag = context.Tags.Where(t => t.Name == "Survival Horror").Select(t => t.Id).FirstOrDefault(); 
            var ShooterTag = context.Tags.Where(t => t.Name == "Shooter").Select(t => t.Id).FirstOrDefault(); 
            var ActionRPGTag = context.Tags.Where(t => t.Name == "Action RPG").Select(t => t.Id).FirstOrDefault(); 
            var FighterTag = context.Tags.Where(t => t.Name == "Fighting").Select(t => t.Id).FirstOrDefault(); 
            var HorrorTag = context.Tags.Where(t => t.Name == "Horror").Select(t => t.Id).FirstOrDefault(); 
            var PuzzleTag = context.Tags.Where(t => t.Name == "Puzzle").Select(t => t.Id).FirstOrDefault(); 
            var AdventureTag = context.Tags.Where(t => t.Name == "Adventure").Select(t => t.Id).FirstOrDefault(); 
            var OpenWorldTag = context.Tags.Where(t => t.Name == "Open-World").Select(t => t.Id).FirstOrDefault(); 

            context.GameTags.AddRange(new List<GameTag>
            {
                new GameTag{GameId = TBOI, TagId = RogueLikeTag},
                new GameTag{GameId = TBOI, TagId = ShooterTag},
                new GameTag{GameId = TBOI, TagId = Action_AdventureTag},
                new GameTag{GameId = TBOI, TagId = HorrorTag},

                new GameTag{ GameId = Hades, TagId = RogueLikeTag },
                new GameTag{ GameId = Hades, TagId = ShooterTag },
                new GameTag{ GameId = Hades, TagId = ActionRPGTag },
                new GameTag{ GameId = Hades, TagId = FighterTag },

                new GameTag{ GameId = RDR2, TagId = OpenWorldTag },
                new GameTag{ GameId = RDR2, TagId = ShooterTag },
                new GameTag{ GameId = RDR2, TagId = Action_AdventureTag },
                new GameTag{ GameId = RDR2, TagId = AdventureTag },

                new GameTag{ GameId = RE8, TagId = SurvivalHorrorTag },
                new GameTag{ GameId = RE8, TagId = HorrorTag },
                new GameTag{ GameId = RE8, TagId = ShooterTag },
                new GameTag{ GameId = RE8, TagId = AdventureTag },
                new GameTag{ GameId = RE8, TagId = PuzzleTag }
            });
            context.SaveChanges();
        }
    }
}
