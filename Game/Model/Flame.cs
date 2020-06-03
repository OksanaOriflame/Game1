using Game.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Flame : GameObject
    {
        public Flame(int x, int y, Room thisRoom)
        {
            ObjType = ObjectType.Knight;
            CurrentSprite = SpriteType.Stand;
            X = x;
            Y = y;
            SpriteWidth = 128;
            Targets = new List<GameObject> { thisRoom.CurrentPlayer };
            XHitBox = 28;
            YHitBox = 128;
            YMin = 0;
            Damage = 30;
            MaxHP = 300;
            HP = MaxHP;
            ThisRoom = thisRoom;
            Interval = 200;
            AttackDistance = 150;
            Speed = 6;
            XCorrection = -128;
            YCorrection = -128;
            AttackAnimationDuration = 5;
            AttackTimings = new int[5] { 100, 100, 100, 300, 400 };
            AttackMoment = 3;
            AttackCenter = new Point(0, 80);
            DeathAnimationDuration = 4;
            IsHitted = false;
            HitIterator = 0;
            HitState = 0;
            MaxHitState = 2;
        }
    }
}
