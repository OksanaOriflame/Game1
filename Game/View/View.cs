using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model;

namespace Game.View
{
    public enum ObjectType
    {
        Player,
        Room,
        Knight
    }
    public enum SpriteType
    {
        Static,
        Stand,
        Walk,
        Attack,
        Hit,
        Hitted
    }
    public enum Direction
    {
        Left,
        Right
    }
    public static class View
    {
        public static Dictionary<ObjectType, Dictionary<SpriteType, Image[]>> ImageSets;
        public static void InitializeSets()
        {
            ImageSets = new Dictionary<ObjectType, Dictionary<SpriteType, Image[]>>();
            ImageFiller.FillSets(ImageSets);
        }
        public static IEnumerable<Sprite> Sprites(Room room)
        {
            if (room.CurrentPlayer.isHitted)
                yield return new Sprite(room.CurrentPlayer, true);
            yield return new Sprite(room.CurrentPlayer);
            foreach (var obj in room.Enemies)
                yield return new Sprite(obj);
        }
        public class Sprite
        {
            static int HitState = 0;
            public Sprite(GameObject obj)
            {
                Image = (Image)ImageSets[obj.ObjType][obj.CurrentSprite][obj.State].Clone();
                if (obj.CurrentDirection == Direction.Left)
                    Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                Coordinates = new Point(obj.X - obj.SpriteWidth / 2, obj.Y);
            }
            public Sprite(Player player, bool flag)
            {
                Image = ImageSets[ObjectType.Player][SpriteType.Hitted][HitState];
                Coordinates = new Point(player.X - player.SpriteWidth / 2, player.Y);
            }
            public Image Image;
            public Point Coordinates;
        }
    }

    
}
