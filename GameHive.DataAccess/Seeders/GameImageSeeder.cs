using GameHive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameHive.DataAccess.Seeders
{
    public static class GameImageSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.GameImages.Any()) return;
            var TBOI = context.Games.Where(g => g.Name == "The Binding Of Isaac").Select(g => g.GameId).FirstOrDefault();
            var Hades = context.Games.Where(g => g.Name == "Hades").Select(g => g.GameId).FirstOrDefault();
            var RE8 = context.Games.Where(g => g.Name == "Resident Evil 8: Village").Select(g => g.GameId).FirstOrDefault();
            var RDR2 = context.Games.Where(g => g.Name == "Red Dead Redemption 2").Select(g => g.GameId).FirstOrDefault();
            var CP2077 = context.Games.Where(g => g.Name == "Cyberpunk 2077").Select(g => g.GameId).FirstOrDefault();
            var TW3 = context.Games.Where(g => g.Name == "The Witcher 3: Wild Hunt").Select(g => g.GameId).FirstOrDefault();
            var GTA5 = context.Games.Where(g => g.Name == "Grand Theft Auto V").Select(g => g.GameId).FirstOrDefault();

            if (TBOI != 0)
            {
                context.Add(new GameImage { GameId = TBOI, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1426300/header.jpg?t=1732645274" });
                context.Add(new GameImage { GameId = TBOI, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1426300/ss_368e275491bce7f2d43ce32bc451eede42d176ad.1920x1080.jpg?t=1732645274" });
                context.Add(new GameImage { GameId = TBOI, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1426300/ss_a142fd0ef647fb0c5dad64c36d6f21c688a7881f.1920x1080.jpg?t=1732645274" });
                context.Add(new GameImage { GameId = TBOI, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1426300/ss_886598ad4e9c40e6486e2375b0f00712522d999d.1920x1080.jpg?t=1732645274" });
            }
            if (Hades != 0)
            {
                context.Add(new GameImage { GameId = Hades, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1145360/header.jpg?t=1715722799" });
                context.Add(new GameImage { GameId = Hades, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1145360/ss_c0fed447426b69981cf1721756acf75369801b31.1920x1080.jpg?t=1715722799" });
                context.Add(new GameImage { GameId = Hades, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1145360/ss_8a9f0953e8a014bd3df2789c2835cb787cd3764d.1920x1080.jpg?t=1715722799" });
                context.Add(new GameImage { GameId = Hades, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1145360/ss_68300459a8c3daacb2ec687adcdbf4442fcc4f47.1920x1080.jpg?t=1715722799" });
            }
            if (RE8 != 0)
            {
                context.Add(new GameImage { GameId = RE8, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1196590/header.jpg?t=1741142800" });
                context.Add(new GameImage { GameId = RE8, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1196590/ss_d25704b01be292d1337df4fea0fba2aab322b58a.1920x1080.jpg?t=1741142800" });
                context.Add(new GameImage { GameId = RE8, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1196590/ss_8113ec993ec474055c4cdce5ee86f91f7cf6663f.600x338.jpg?t=1741142800" });
                context.Add(new GameImage { GameId = RE8, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1196590/ss_d6c5bfb48d7fda343ed583750372b0d3e513ae17.600x338.jpg?t=1741142800" });
            }
            if (RDR2 != 0)
            {
                context.Add(new GameImage { GameId = RDR2, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1174180/header.jpg?t=1720558643" });
                context.Add(new GameImage { GameId = RDR2, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1174180/ss_66b553f4c209476d3e4ce25fa4714002cc914c4f.600x338.jpg?t=1720558643" });
                context.Add(new GameImage { GameId = RDR2, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1174180/ss_bac60bacbf5da8945103648c08d27d5e202444ca.600x338.jpg?t=1720558643" });
                context.Add(new GameImage { GameId = RDR2, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1174180/ss_668dafe477743f8b50b818d5bbfcec669e9ba93e.600x338.jpg?t=1720558643" });
            }
            if (CP2077 != 0)
            {
                context.Add(new GameImage { GameId = CP2077, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1091500/header.jpg?t=1734434803" });
                context.Add(new GameImage { GameId = CP2077, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1091500/ss_2f649b68d579bf87011487d29bc4ccbfdd97d34f.600x338.jpg?t=1734434803" });
                context.Add(new GameImage { GameId = CP2077, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1091500/ss_af2804aa4bf35d4251043744412ce3b359a125ef.600x338.jpg?t=1734434803" });
                context.Add(new GameImage { GameId = CP2077, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1091500/ss_9284d1c5b248726760233a933dbb83757d7d5d95.600x338.jpg?t=1734434803" });
            }
            if (TW3 != 0)
            {
                context.Add(new GameImage { GameId = TW3, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/292030/header.jpg?t=1736424367" });
                context.Add(new GameImage { GameId = TW3, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/292030/ss_5710298af2318afd9aa72449ef29ac4a2ef64d8e.600x338.jpg?t=1736424367" });
                context.Add(new GameImage { GameId = TW3, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/292030/ss_0901e64e9d4b8ebaea8348c194e7a3644d2d832d.600x338.jpg?t=1736424367" });
                context.Add(new GameImage { GameId = TW3, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/292030/ss_112b1e176c1bd271d8a565eacb6feaf90f240bb2.600x338.jpg?t=1736424367" });
            }
            if (GTA5 != 0)
            {
                context.Add(new GameImage { GameId = GTA5, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/3240220/header.jpg?t=1741381848" });
                context.Add(new GameImage { GameId = GTA5, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/3240220/d61184a98c1cf2db2b08b2999c04b0519e3615bb/ss_d61184a98c1cf2db2b08b2999c04b0519e3615bb.600x338.jpg?t=1741381848" });
                context.Add(new GameImage { GameId = GTA5, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/3240220/f2e70b5823510daa062293ff0b03821e1dee2d37/ss_f2e70b5823510daa062293ff0b03821e1dee2d37.600x338.jpg?t=1741381848" });
                context.Add(new GameImage { GameId = GTA5, imageURL = "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/3240220/cf15774bb38c9b74f9e98c985228510c97df80b6/ss_cf15774bb38c9b74f9e98c985228510c97df80b6.600x338.jpg?t=1741381848" });
            }
            context.SaveChanges();
        }
    }
}
