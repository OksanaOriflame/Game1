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
        public static void FillSets(Dictionary<ObjectType, Dictionary<SpriteType, Image[]>> sets, Dictionary<WeaponType, Image[]> attackSets)
        {
            var N = 4;
            Path = Directory.GetCurrentDirectory() + @"\Sprites";
            sets.Add(ObjectType.Room, new Dictionary<SpriteType, Image[]>());
            sets[ObjectType.Room][SpriteType.Static] = new Image[4];
            FillThePlayer(sets, attackSets);
            var enemieList = File
                .ReadAllText(Path + @"\Enemies\Enemies.txt")
                .Split('|')
                .Where(x => x.Length > 0)
                .Select(x => x.Split(' '));
            foreach(var enemieData in enemieList)
            {
                var objType = (ObjectType)int.Parse(enemieData[0]);
                sets.Add(objType, new Dictionary<SpriteType, Image[]>());
                for (int i = 2; i < enemieData.Length; i++)
                {
                    var n = 0;
                    var str = enemieData[i];
                    if (int.TryParse(str[str.Length - 1].ToString(), out n))
                    {
                        N = n;
                        str = str.Substring(0, str.Length - 1);
                    }

                    var spriteType = Enums.GetType(str);
                    sets[objType].Add(spriteType, new Image[N]);
                    for (var j = 1; j <= N; j++)
                        sets[objType][spriteType][j - 1] = Image.FromFile(Path + @"\Enemies\" + enemieData[1] + @"\" + str + @"\" + enemieData[1] + j + ".png");
                    N = 4;
                }
            }
        }

        

        static void FillThePlayer(Dictionary<ObjectType, Dictionary<SpriteType, Image[]>> Sets, Dictionary<WeaponType, Image[]> attackSets)
        {
            var weaponList = File
                .ReadAllText(Path + @"\Player\Attack\Weapons.txt")
                .Split('(')
                .Where(str => str.Length > 0)
                .Select(str => str.Split(' '));

            foreach (var weapon in weaponList)
            {
                var type = (WeaponType)int.Parse(weapon[0]);
                var frameCount = int.Parse(weapon[1]);
                attackSets.Add(type, new Image[frameCount]);
                for (int i = 0; i < frameCount; i++)
                    attackSets[type][i] = Image.FromFile(Path + @"\Player\Attack\" + weapon[2] + @"\" + weapon[2] + (i + 1) + ".png");
            }

            Sets.Add(ObjectType.Player, new Dictionary<SpriteType, Image[]>());
            var playerPath = Path + @"\Player";
            
            Sets[ObjectType.Player][SpriteType.Stand] = new Image[4];
            for (var i = 1; i <= 4; i++)
                Sets[ObjectType.Player][SpriteType.Stand][i - 1] = Image.FromFile(playerPath + @"\EmptySprite\Player" + i + ".png");
            
            Sets[ObjectType.Player][SpriteType.Walk] = new Image[4];
            for (var i = 1; i <= 4; i++)
                Sets[ObjectType.Player][SpriteType.Walk][i - 1] = Image.FromFile(playerPath + @"\Walk\PlayerWalk" + i + ".png");

            Sets[ObjectType.Player][SpriteType.Dodge] = new Image[4];
            for (var i = 1; i <= 4; i++)
                Sets[ObjectType.Player][SpriteType.Dodge][i - 1] = Image.FromFile(playerPath + @"\Dodge\PlayerDodge" + i + ".png");

            Sets[ObjectType.Player][SpriteType.Hitted] = new Image[1];
            Sets[ObjectType.Player][SpriteType.Hitted][0] = Image.FromFile(playerPath + @"\Hitted\Player.png");

            Sets[ObjectType.Player][SpriteType.Healing] = new Image[2];
            for (var i = 1; i <= 2; i++)
                Sets[ObjectType.Player][SpriteType.Healing][i - 1] = Image.FromFile(playerPath + @"\Healing\Player" + i + ".png");
        }
    }
}
