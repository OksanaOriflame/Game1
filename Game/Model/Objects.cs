using Game.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Model
{
    

    public class GameObject
    {
        
        public int XCorrection;
        public int YCorrection;
        public int Speed;
        public int HorizontalDirection;
        public string ObjectName;
        public int X;
        public int Y;
        public int XHitBox;
        public int YHitBox;
        public int YMin;
        public int HP;
        public int MaxHP;
        public int Damage;
        public int SpriteWidth;
        public int AttackDistance;
        public int AttackAnimationDuration;
        public int[] AttackTimings;
        public int AttackMoment;
        public Point AttackCenter;
        public Room ThisRoom;
        public List<GameObject> Targets;
        public ObjectType ObjType;
        public SpriteType CurrentSprite;
        public Direction CurrentDirection;
        public int State;
        public int CurrentInterval = 0;
        public int Interval;
        public bool IsImmortal;
        public int DeathAnimationDuration;
        public bool IsHitted;
        public int HitIterator;
        public int HitState;
        public int MaxHitState;
        public Direction OnHitDirection;
        public bool IsBoss;
        public void Update()
        {

            CurrentInterval += MyForm.Interval;

            if (ThisRoom.CurrentPlayer == null)
                return;

            if (CurrentSprite == SpriteType.Death)
            {
                if (CurrentInterval / Interval > 0)
                {
                    State++;
                    CurrentInterval = 0;
                }
                if (State == DeathAnimationDuration)
                {
                    Control.Control.DeadList.Push(this);
                }
                return;
            }

            if (IsHitted)
            {
                HitIterator += MyForm.Interval;
                if (HitIterator / Interval > 0)
                {
                    HitState++;
                    HitIterator = 0;
                }
                if (HitState >= MaxHitState)
                {
                    IsHitted = false;
                }
            }

            if (CurrentSprite == SpriteType.Attack)
            {
                if (CurrentInterval >= AttackTimings[State])
                {
                    State++;
                    CurrentInterval = 0;
                }
                if (State == AttackAnimationDuration)
                {
                    State = 0;
                    CurrentSprite = SpriteType.Stand;
                }
                else if (State == AttackMoment && CurrentInterval == 0)
                {
                    ThisRoom.CurrentPlayer.RegisterHit(X, Y, CurrentDirection, AttackCenter, this);
                }
            }
            else
            {
                var pX = ThisRoom.CurrentPlayer.X - (AttackCenter.X + X);
                if (CurrentInterval / Interval > 0)
                {
                    State = (State + 1) % 4;
                    CurrentInterval = 0;
                }
                if (Math.Abs(pX) > AttackDistance)
                {
                    CurrentSprite = SpriteType.Walk;
                    HorizontalDirection = pX > 0 ? 1 : -1;
                    CurrentDirection = pX > 0 ? Direction.Right : Direction.Left;
                }
                else if (Math.Abs(pX) <= AttackDistance && State == 3)
                {
                    CurrentSprite = SpriteType.Attack;
                    State = 0;
                    CurrentInterval = 0;
                    CurrentDirection = pX > 0 ? Direction.Right : Direction.Left;
                }
                else
                {
                    HorizontalDirection = 0;
                    CurrentSprite = SpriteType.Stand;
                }
                X += Speed * HorizontalDirection;
            }

        }

        public Tuple<bool, Direction, int> GetIntersection(Player player, int leftXPlayer, int rightXPlayer, int topYPlayer, int bottomYPlayer)
        {
            if (CurrentSprite == SpriteType.Death)
                return new Tuple<bool, Direction, int>(false, default, default);
            var leftX = X - XHitBox;
            var rightX = X + XHitBox;
            var bottomY = Y + YHitBox;
            if (!(leftX > rightXPlayer || rightX < leftXPlayer || bottomY < topYPlayer || Y > bottomYPlayer))
                return new Tuple<bool, Direction, int>(
                    true,
                    ((X - leftXPlayer + X - rightXPlayer) <= 0 ? Direction.Right : Direction.Left),
                    this.Damage);
            else
                return new Tuple<bool, Direction, int>(false, default, default);
        }

        public void RegisterHit(int x, int y, Direction dir, Point attackCenter, Weapon weapon)
        {
            var leftX = ThisRoom.CurrentPlayer.X + ThisRoom.CurrentPlayer.AttackCenter.X;
            var rightX = leftX + weapon.Length * (dir == Direction.Left ? -1 : 1);
            if (dir == Direction.Left)
            {
                var temp = leftX;
                leftX = rightX;
                rightX = temp;
            }
            if (y + attackCenter.Y >= this.Y &&
                y + attackCenter.Y <= this.Y + YHitBox)
            {
                if (!(this.X - XHitBox > rightX ||
                    this.X + XHitBox < leftX))
                    this.GetDamage(weapon.Damage);
            }
        }

        public void GetDamage(int damage)
        {
            if (CurrentSprite == SpriteType.Death)
                return;
            HP -= damage;
            if (HP <= 0)
            {
                State = 0;
                CurrentInterval = 0;
                CurrentSprite = SpriteType.Death;
            }
            else
            {
                IsHitted = true;
                HitIterator = 0;
                HitState = 0;
                OnHitDirection = CurrentDirection;
            }
        }

        public void RegisterHit(int x, int y, Direction dir, Point attackCenter, GameObject obj)
        {
            var player = ThisRoom.CurrentPlayer;
            var leftX = x + attackCenter.X;
            var rightX = leftX + (dir == Direction.Right ? 1 : -1) * obj.AttackDistance;
            if (dir == Direction.Left)
            {
                var temp = leftX;
                leftX = rightX;
                rightX = temp;
            }
            if (player.Y <= obj.Y + obj.AttackCenter.Y &&
                player.Y + player.YHitBox >= obj.Y + obj.AttackCenter.Y)
            {
                if (rightX < player.X - player.XHitBox || leftX > player.X + player.XHitBox)
                    return;
                ThisRoom.CurrentPlayer.GetDamage(obj.Damage, dir);
            }
        }
    }

    
}
