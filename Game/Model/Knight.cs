using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.View;
using System.Drawing;

namespace Game.Model
{
    public class Knight : GameObject
    {
        #region
        public void Attack(int direction)
        {
            ThisRoom.CurrentPlayer.RegisterHit(AttackCenter.X + AttackDirection.X * direction, AttackCenter.Y + AttackDirection.Y, Damage);
        }

        #endregion

        public Knight(int x, int y, Room thisRoom)
        {
            ObjType = ObjectType.Knight;
            CurrentSprite = SpriteType.Stand;
            X = x;
            Y = y;
            SpriteWidth = 32;
            targets = new List<GameObject> { thisRoom.CurrentPlayer };
            XHitBox = 10;
            YHitBox = 32;
            YMin = 0;
            AttackCenter = new Point(X, Y + 18);
            AttackDirection = new Point(22, 0);
            Damage = 15;
            HP = 100;
            ThisRoom = thisRoom;
        }

        public void NextAction()
        {
            //if (ThisRoom.Person.X > )
        }
    }
}
