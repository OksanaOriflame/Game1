using Game.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Wrath : GameObject
    {
        public Random Rnd;
        int[] ASet;
        int[] BSet;
        int[] CSet;
        public Wrath(int x, int y, Room thisRoom)
        {
            IsBoss = true;
            ASet = new int[] { 80, 100, 100, 100 };
            BSet = new int[] { 80, 50, 50, 100 };
            CSet = new int[] { 80, 80, 80, 80 };
            Rnd = new Random();
            ObjType = ObjectType.Wrath;
            CurrentSprite = SpriteType.Stand;
            X = x;
            Y = y;
            SpriteWidth = 1440;
            Targets = new List<GameObject> { thisRoom.CurrentPlayer };
            XHitBox = 59;
            YHitBox = 1000;
            YMin = 0;
            Damage = 50;
            MaxHP = 600;
            HP = MaxHP;
            ThisRoom = thisRoom;
            Interval = 180;
            AttackDistance = 720;
            Speed = 0;
            XCorrection = 0;
            YCorrection = 0;
            AttackAnimationDuration = 4;
            AttackMoment = 3;
            AttackCenter = new Point(0, 550);
            DeathAnimationDuration = 6;
            IsHitted = false;
            HitIterator = 0;
            HitState = 0;
            MaxHitState = 2;
        }

        public void UpdateBoss()
        {
            if (IsHitted)
            {
                HitIterator += MyForm.Interval;
                if (HP <= 0)
                {
                    CurrentSprite = SpriteType.Death;
                    CurrentInterval = 0;
                    State = 0;
                    return;
                }
                if (HitIterator > 200)
                {
                    HitState++;
                    HitIterator = 0;
                    if (HitState > 1)
                    {
                        HitState = 0;
                        HitIterator = 0;
                        IsHitted = false;
                    }
                }
            }
            CurrentInterval += MyForm.Interval;
            if (CurrentSprite == SpriteType.Death)
            {
                if (CurrentInterval / 140 > 0)
                {
                    State++;
                    CurrentInterval = 0;
                }
                if (State == DeathAnimationDuration)
                    Control.Control.DeadList.Push(this);
                return;
            }
            else if (CurrentSprite == SpriteType.Stand)
            {
                if (CurrentInterval / Interval > 0)
                {
                    State++;
                    CurrentInterval = 0;
                }
                if (State == 4 )
                {
                    var dist = ThisRoom.CurrentPlayer.X - 720;
                    CurrentDirection = dist < 0 ? Direction.Left : Direction.Right;
                    if (Math.Abs(dist) < 310 && Rnd.Next(10) < 7)
                    {
                        CurrentSprite = SpriteType.B;
                        AttackDistance = 320;
                        CurrentInterval = 0;
                        State = 0;
                    }
                    else if (Math.Abs(dist) >= 310 && Rnd.Next(10) < 8 || Rnd.Next(10) < 5)
                    {
                        CurrentSprite = SpriteType.A;
                        AttackDistance = 720;
                        CurrentInterval = 0;
                        State = 0;
                    }
                    else if (Rnd.Next(10) < 8)
                    {
                        CurrentSprite = SpriteType.C;
                        CurrentInterval = 0;
                        State = 0;
                    }
                    else
                    {
                        CurrentInterval = 0;
                        State = 0;
                    }
                }
            }
            else if (CurrentSprite == SpriteType.A)
            {
                if (CurrentInterval / ASet[State] > 1)
                {
                    State++;
                    CurrentInterval = 0;
                    if (State == 2)
                    {
                        ThisRoom.CurrentPlayer.RegisterHit(X, Y, CurrentDirection, AttackCenter, this);
                    }
                    if (State == 4)
                    {
                        State = 0;
                        CurrentInterval = 0;
                        CurrentSprite = SpriteType.Stand;
                    }
                }
            }
            else if (CurrentSprite == SpriteType.B)
            {
                if (CurrentInterval / BSet[State] > 1)
                {
                    State++;
                    CurrentInterval = 0;
                    if (State == 3)
                    {
                        ThisRoom.CurrentPlayer.RegisterHit(X, Y, CurrentDirection, AttackCenter, this);
                    }
                    if (State == 4)
                    {
                        State = 0;
                        CurrentInterval = 0;
                        CurrentSprite = SpriteType.Stand;
                    }
                }
            }
            else if (CurrentSprite == SpriteType.C)
            {
                if (CurrentInterval / CSet[State] > 1)
                {
                    State++;
                    CurrentInterval = 0;
                    if (State == 3)
                    {
                        ThisRoom.CurrentPlayer.RegisterHit(X, Y, CurrentDirection, AttackCenter, this);
                    }
                    if (State == 4)
                    {
                        State = 0;
                        CurrentInterval = 0;
                        CurrentSprite = SpriteType.Stand;
                    }
                }
            }
        }
    }
}
