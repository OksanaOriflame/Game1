using Game.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    class Statue : GameObject
    {
        public Statue(int x, int y, Room thisRoom)
        {
            ObjType = ObjectType.Statue;
            CurrentSprite = SpriteType.Stand;
            X = x;
            Y = y;
            SpriteWidth = 160;
            Targets = new List<GameObject> { thisRoom.CurrentPlayer };
            XHitBox = 35;
            YHitBox = 160;
            YMin = 0;
            Damage = 25;
            MaxHP = 250;
            HP = MaxHP;
            ThisRoom = thisRoom;
            Interval = 200;
            AttackDistance = 550;
            Speed = 3;
            XCorrection = -480;
            YCorrection = 0;
            AttackAnimationDuration = 6;
            AttackTimings = new int[6] { 200, 200, 400, 400, 400, 400 };
            AttackMoment = 4;
            AttackCenter = new Point(0, 80);
            DeathAnimationDuration = 4;
            IsHitted = false;
            HitIterator = 0;
            HitState = 0;
            MaxHitState = 2;
        }
    }
}
