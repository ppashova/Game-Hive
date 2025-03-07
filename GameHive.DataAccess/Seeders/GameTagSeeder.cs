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
            var CP2077 = context.Games.Where(g=> g.Name == "Cyberpunk 2077").Select(g => g.GameId).FirstOrDefault();
            var TW3 = context.Games.Where(g => g.Name == "The Witcher 3: Wild Hunt").Select(g => g.GameId).FirstOrDefault();
            var GTA5 = context.Games.Where(g => g.Name == "Grand Theft Auto V").Select(g => g.GameId).FirstOrDefault();

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
            var RPGTag = context.Tags.Where(t => t.Name == "RPG").Select(t => t.Id).FirstOrDefault();
            var FPSTag = context.Tags.Where(t => t.Name == "FPS").Select(t => t.Id).FirstOrDefault();

            if (TBOI != 0 && RogueLikeTag != 0)
                context.Add(new GameTag { GameId = TBOI, TagId = RogueLikeTag });
            if(TBOI != 0 && ShooterTag != 0)
                context.Add(new GameTag { GameId = TBOI, TagId = ShooterTag });
            if (TBOI != 0 && Action_AdventureTag != 0)
                context.Add(new GameTag { GameId = TBOI, TagId = Action_AdventureTag });
            if (TBOI != 0 && HorrorTag != 0)
                context.Add(new GameTag { GameId = TBOI, TagId = HorrorTag });

            if (Hades != 0 && RogueLikeTag != 0)
                context.Add(new GameTag { GameId = Hades, TagId = RogueLikeTag });
            if (Hades != 0 && ShooterTag != 0)
                context.Add(new GameTag { GameId = Hades, TagId = ShooterTag });
            if (Hades != 0 && ActionRPGTag != 0)
                context.Add(new GameTag { GameId = Hades, TagId = ActionRPGTag });
            if (Hades != 0 && FighterTag != 0)
                context.Add(new GameTag { GameId = Hades, TagId = FighterTag });

            if(RDR2 != 0 && OpenWorldTag != 0)
                context.Add(new GameTag { GameId = RDR2, TagId = OpenWorldTag });
            if (RDR2 != 0 && ShooterTag != 0)
                context.Add(new GameTag { GameId = RDR2, TagId = ShooterTag });
            if (RDR2 != 0 && Action_AdventureTag != 0)
                context.Add(new GameTag { GameId = RDR2, TagId = Action_AdventureTag });
            if (RDR2 != 0 && AdventureTag != 0)
                context.Add(new GameTag { GameId = RDR2, TagId = AdventureTag });
            
            if(RE8 != 0 && SurvivalHorrorTag != 0)
                context.Add(new GameTag { GameId = RE8, TagId = SurvivalHorrorTag });
            if(RE8 != 0 && HorrorTag != 0)
                context.Add(new GameTag { GameId = RE8, TagId = HorrorTag });
            if (RE8 != 0 && ShooterTag != 0)
                context.Add(new GameTag { GameId = RE8, TagId = ShooterTag });
            if (RE8 != 0 && AdventureTag != 0)
                context.Add(new GameTag { GameId = RE8, TagId = AdventureTag });
            if (RE8 != 0 && PuzzleTag != 0)
                context.Add(new GameTag { GameId = RE8, TagId = PuzzleTag });

            if (CP2077 != 0 && ShooterTag != 0)
                context.Add(new GameTag { GameId = CP2077, TagId = ShooterTag });
            
            if(CP2077 != 0 && AdventureTag != 0)
                context.Add(new GameTag { GameId = CP2077, TagId = AdventureTag });
            
            if (CP2077 != 0 && RPGTag != 0)
                context.Add(new GameTag { GameId = CP2077, TagId = RPGTag });
            
            if (CP2077 != 0 && FighterTag != 0)
                context.Add(new GameTag { GameId = CP2077, TagId = FighterTag });
            

            if (TW3 != 0 && OpenWorldTag != 0)
                context.Add(new GameTag { GameId = TW3, TagId = OpenWorldTag });
            if (TW3 != 0 && ActionRPGTag != 0)
                context.Add(new GameTag { GameId = TW3, TagId = ActionRPGTag });
            if(TW3 != 0 && Action_AdventureTag != 0)
                context.Add(new GameTag { GameId = TW3, TagId = Action_AdventureTag });
            if (TW3 != 0 && FighterTag != 0)
                context.Add(new GameTag { GameId = TW3, TagId = FighterTag });

            if(GTA5 != 0 && OpenWorldTag != 0)
                context.Add(new GameTag { GameId = GTA5, TagId = OpenWorldTag });
            if (GTA5 != 0 && FPSTag != 0)
                context.Add(new GameTag { GameId = GTA5, TagId = FPSTag });
            if (GTA5 != 0 && Action_AdventureTag != 0)
                context.Add(new GameTag { GameId = GTA5, TagId = Action_AdventureTag });

            context.SaveChanges();
        }
    }
}
