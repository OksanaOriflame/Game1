using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model;

namespace Game.View
{
    public static class View
    {
        public static Dictionary<ObjectType, Dictionary<SpriteType, Image[]>> ImageSets;
        public static Dictionary<WeaponType, Image[]> AttackSets;
        public static void InitializeSets()
        {
            ImageSets = new Dictionary<ObjectType, Dictionary<SpriteType, Image[]>>();
            AttackSets = new Dictionary<WeaponType, Image[]>();
            ImageFiller.FillSets(ImageSets, AttackSets);
        }
        public static IEnumerable<Sprite> Sprites(Room room)
        {
            yield return new Sprite(room.CurrentPlayer);
            foreach (var obj in room.Enemies)
            {
                yield return new Sprite(obj);
                if (obj.IsHitted)
                {
                    yield return new Sprite(obj, true);
                }
            }
        }
        public class Sprite
        {
            public Sprite(GameObject obj)
            {
                if (obj.CurrentSprite == SpriteType.Attack)
                {
                    if (obj.ObjType == ObjectType.Player)
                        SpriteImage = (Image)AttackSets[((Player)obj).CurrentWeapon.Type][obj.State].Clone();
                    else
                        SpriteImage = (Image)ImageSets[obj.ObjType][obj.CurrentSprite][obj.State].Clone();
                    Coordinates = new Point(obj.X - obj.SpriteWidth / 2 + obj.XCorrection, obj.Y + obj.YCorrection);
                }
                else
                {
                    SpriteImage = (Image)ImageSets[obj.ObjType][obj.CurrentSprite][obj.State].Clone();
                    Coordinates = new Point(obj.X - obj.SpriteWidth / 2, obj.Y);
                }
                if (obj.CurrentDirection == Direction.Left)
                    SpriteImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
                
            }
            public Sprite(GameObject obj, bool flag)
            {
                SpriteImage = (Image)ImageSets[obj.ObjType][SpriteType.Hitted][obj.HitState].Clone();
                Coordinates = new Point(obj.X - obj.SpriteWidth / 2, obj.Y);
                if (obj.OnHitDirection == Direction.Left)
                    SpriteImage.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            public Image SpriteImage;
            public Point Coordinates;
        }
    }

    
}
