using Game.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Blot : GameObject
    {
        public Blot(int x, int y, Room thisRoom)
        {
            ObjType = ObjectType.Blot;
            CurrentSprite = SpriteType.Stand;
            X = x;
            Y = y;
            SpriteWidth = 96;
            Targets = new List<GameObject> { thisRoom.CurrentPlayer };
            XHitBox = 12;
            YHitBox = 96;
            YMin = 0;
            Damage = 20;
            MaxHP = 150;
            HP = MaxHP;
            ThisRoom = thisRoom;
            Interval = 200;
            AttackDistance = 55;
            Speed = 8;
            XCorrection = 0;
            YCorrection = 0;
            AttackAnimationDuration = 4;
            AttackTimings = new int[4] { 100, 100, 100, 200 };
            AttackMoment = 2;
            AttackCenter = new Point(0, 42);
            DeathAnimationDuration = 4;
            IsHitted = false;
            HitIterator = 0;
            HitState = 0;
            MaxHitState = 2;
        }
    }
}
