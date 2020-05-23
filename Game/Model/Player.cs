using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.View;

namespace Game.Model
{
    public class Player : GameObject
    {
        public int Speed;
        public int VerticalSpeed;
        public bool isHitted;
        public bool isJumping;
        public int JumpingIterator;
        public Player(int x, int y)
        {
            ObjType = ObjectType.Player;
            CurrentSprite = SpriteType.Stand;
            HP = 100;
            X = x;
            Y = y;
            XHitBox = 6;
            YHitBox = 28;
            YMin = 0;
            isHitted = false;
            isJumping = false;
            SpriteWidth = 96;
            ObjectName = "Player";
            State = 0;
            Speed = 20;
            HorizontalDirection = 0;
            CurrentDirection = Direction.Right;
            VerticalSpeed = 0;
        }

        public void Update()
        {
            State = (State + 1) % 4;
            X = X + HorizontalDirection * Speed;
            if (isJumping)
            {
                VerticalSpeed += JumpingIterator * 3;
                JumpingIterator++;
                var floors = ThisRoom.Floors
                    .Where(floor => (floor.LeftX < X + 48) && (floor.RightX > X + 48))
                    .Where(floor => (floor.Y >= Y + 96) && (floor.Y <= Y + 96 + VerticalSpeed))
                    .OrderBy(floor => floor.Y)
                    .ToArray();
                if (floors.Length != 0)
                {
                    VerticalSpeed = 0;
                    Y = floors[0].Y - 96;
                    isJumping = false;
                }
                else
                    Y += VerticalSpeed;
            }
        }

        public void SpeedUp(int dir)
        {
            CurrentDirection = dir != 0 ? (dir > 0 ? Direction.Right : Direction.Left) : CurrentDirection;
            HorizontalDirection = dir;
            CurrentSprite = dir == 0 ? SpriteType.Stand : SpriteType.Walk;
        }
        public void Jump()
        {
            VerticalSpeed = -50;
            isJumping = true;
            JumpingIterator = 0;
        }
    }
}
