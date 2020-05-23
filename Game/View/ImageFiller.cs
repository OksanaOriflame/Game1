using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Game.View
{
    public static class ImageFiller
    {
        private static string Path;
        public static void FillSets(Dictionary<ObjectType, Dictionary<SpriteType, Image[]>> Sets)
        {
            Path = Directory.GetCurrentDirectory() + @"\Sprites";
            Sets.Add(ObjectType.Room, new Dictionary<SpriteType, Image[]>());
            Sets[ObjectType.Room][SpriteType.Static] = new Image[4];
            for (var i = 1; i <= 4; i++)
                Sets[ObjectType.Room][SpriteType.Static][i - 1] = Image.FromFile(Path + @"\Locations\Hall\TheHall" + i + ".jpeg");
            FillThePlayer(Sets);
        }

        static void FillThePlayer(Dictionary<ObjectType, Dictionary<SpriteType, Image[]>> Sets)
        {
            Sets.Add(ObjectType.Player, new Dictionary<SpriteType, Image[]>());
            Sets[ObjectType.Player][SpriteType.Stand] = new Image[4];
            Sets[ObjectType.Player][SpriteType.Walk] = new Image[4];
            var playerPath = Path + @"\Player";
            for (var i = 1; i <= 4; i++)
                Sets[ObjectType.Player][SpriteType.Stand][i - 1] = Image.FromFile(playerPath + @"\EmptySprite\Player" + i + ".png");
            for (var i = 1; i <= 4; i++)
                Sets[ObjectType.Player][SpriteType.Walk][i - 1] = Image.FromFile(playerPath + @"\Walk\PlayerWalk" + i + ".png");
        }
    }
}
