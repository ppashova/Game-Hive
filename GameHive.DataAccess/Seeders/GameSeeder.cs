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
                    new Game {
                        Name = "The Binding Of Isaac",
                        Price = decimal.Zero,
                        BriefDescription = "A roguelike dungeon crawler",
                        FullDescription = "The Binding of Isaac is a randomly generated action RPG shooter with heavy roguelike elements. Following Isaac on his journey players will find bizarre treasures that change Isaac’s form giving him super human abilities and enabling him to fight off droves of mysterious creatures, discover secrets and fight his way to safety.",
                        GameIconUrl = "https://cdn2.steamgriddb.com/icon/bc573864331a9e42e4511de6f678aa83/24/256x256.png",
                        SteamLink = "https://store.steampowered.com/app/113200/The_Binding_of_Isaac/",
                        RequestStatus = GameHive.Models.enums.RequestEnums.Approved
                    },

                    new Game { 
                        Name = "Hades",
                        Price = 24.99m,
                        BriefDescription = "A roguelike dungeon crawler",
                        FullDescription = "Hades is a god-like rogue-like dungeon crawler that combines the best aspects of Supergiant's critically acclaimed titles, including the fast-paced action of Bastion, the rich atmosphere and depth of Transistor, and the character-driven storytelling of Pyre.",
                        GameIconUrl = "https://cdn2.steamgriddb.com/icon/851300ee84c2b80ed40f51ed26d866fc/32/256x256.png",
                        SteamLink = "https://store.steampowered.com/app/1145360/Hades/",
                        RequestStatus = GameHive.Models.enums.RequestEnums.Approved
                    },
                    new Game {
                        Name = "Resident Evil 8: Village",
                        Price = 59.99m,
                        BriefDescription = "A survival horror game",
                        FullDescription = "Experience survival horror like never before in the eighth major installment in the storied Resident Evil franchise - Resident Evil Village. Set a few years after the horrifying events in the critically acclaimed Resident Evil 7 biohazard.",
                        GameIconUrl = "https://assets-prd.ignimgs.com/2021/01/22/re-village-button-fin-1611277715193.jpg?width=300&crop=1%3A1%2Csmart&auto=webp&dpr=2",
                        SteamLink = "https://store.steampowered.com/app/1196590/Resident_Evil_Village/",
                        RequestStatus = GameHive.Models.enums.RequestEnums.Approved
                    },
                    new Game{
                        Name = "Red Dead Redemption 2",
                        Price = 59.99m,
                        BriefDescription = "An action-adventure game",
                        FullDescription = "America, 1899. The end of the wild west era has begun as lawmen hunt down the last remaining outlaw gangs. Those who will not surrender or succumb are killed.",
                        GameIconUrl = "https://cdn2.steamgriddb.com/icon_thumb/2e65f2f2fdaf6c699b223c61b1b5ab89.png",
                        SteamLink = "https://store.steampowered.com/app/1174180/Red_Dead_Redemption_2/",
                        RequestStatus = GameHive.Models.enums.RequestEnums.Approved
                    },
                    new Game
                    {
                        Name = "Cyberpunk 2077",
                        Price = 59.99m,
                        BriefDescription = "An action-adventure game",
                        FullDescription = "Cyberpunk 2077 is an open-world, action-adventure story set in Night City, a megalopolis obsessed with power, glamour and body modification. You play as V, a mercenary outlaw going after a one-of-a-kind implant that is the key to immortality.",
                        GameIconUrl = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/i/9a6ae0c8-60bc-4cdb-93d4-d81bba9240ed/dfkzvpf-8a472c23-48b9-4ad9-99fc-cc3c6272b3a2.png",
                        SteamLink = "https://store.steampowered.com/app/1091500/Cyberpunk_2077/",
                        RequestStatus = GameHive.Models.enums.RequestEnums.Approved
                    },
                    new Game
                    {
                        Name = "The Witcher 3: Wild Hunt",
                        Price = 39.99m,
                        BriefDescription = "An action-adventure game",
                        FullDescription = "The Witcher: Wild Hunt is a story-driven open world RPG set in a visually stunning fantasy universe full of meaningful choices and impactful consequences. In The Witcher, you play as professional monster hunter Geralt of Rivia tasked with finding a child of prophecy in a vast open world rich with merchant cities, pirate islands, dangerous mountain passes, and forgotten caverns to explore.",
                        GameIconUrl = "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/f/c78bc3fc-9f08-47ca-81ae-d89055c7ec49/d8p7j8m-978d944f-b106-413d-9e7b-18fc7875b47c.png?token=eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJ1cm46YXBwOjdlMGQxODg5ODIyNjQzNzNhNWYwZDQxNWVhMGQyNmUwIiwiaXNzIjoidXJuOmFwcDo3ZTBkMTg4OTgyMjY0MzczYTVmMGQ0MTVlYTBkMjZlMCIsIm9iaiI6W1t7InBhdGgiOiJcL2ZcL2M3OGJjM2ZjLTlmMDgtNDdjYS04MWFlLWQ4OTA1NWM3ZWM0OVwvZDhwN2o4bS05NzhkOTQ0Zi1iMTA2LTQxM2QtOWU3Yi0xOGZjNzg3NWI0N2MucG5nIn1dXSwiYXVkIjpbInVybjpzZXJ2aWNlOmZpbGUuZG93bmxvYWQiXX0.YKJZ24xa3NeCKchv4M8bc3mMBqCjJtto3TRQl57XeJY",
                        SteamLink = "https://store.steampowered.com/app/292030/The_Witcher_3_Wild_Hunt/",
                        RequestStatus = GameHive.Models.enums.RequestEnums.Approved
                    },
                    new Game
                    {
                        Name = "Grand Theft Auto V",
                        Price = 29.99m,
                        BriefDescription = "An open-world rpg game",
                        FullDescription = "When a young street hustler, a retired bank robber and a terrifying psychopath find themselves entangled with some of the most frightening and deranged elements of the criminal underworld, the U.S. government and the entertainment industry, they must pull off a series of dangerous heists to survive in a ruthless city in which they can trust nobody, least of all each other.",
                        GameIconUrl = "https://static.vecteezy.com/system/resources/previews/027/127/540/non_2x/grand-theft-auto-gta-v-logo-grand-theft-auto-gta-v-icon-transparent-free-png.png",
                        SteamLink = "https://store.steampowered.com/app/271590/Grand_Theft_Auto_V/",
                        RequestStatus = GameHive.Models.enums.RequestEnums.Approved
                    }
                });
                context.SaveChanges();
            }
        }
    }
}
