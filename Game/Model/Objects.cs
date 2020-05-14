using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Room
    {
        readonly int Height;
        readonly int Width;
        readonly string Name;
        public List<GameObject> Enemies;
        public Player CurrentPlayer;

        public Room (int width, int height, string name, Player player)
        {
            CurrentPlayer = player;
            Height = height;
            Width = width;
            Name = name;
            Enemies = new List<GameObject>();
            player.targets = Enemies;
        }

        public void AddObject(GameObject obj)
        {
            Enemies.Add(obj);
        }
    }

    public class Player : GameObject
    {
        public Player(int x, int y)
        {
            HP = 100;
            X = x;
            Y = y;
            XHitBox = 6;
            YHitBox = 28;
            YMin = 0;
        }
    }

    public class GameObject
    {
        public int X;
        public int Y;
        public int XHitBox;
        public int YHitBox;
        public int YMin;
        public int HP;
        public int Damage;
        public Point AttackCenter;
        public Point AttackDirection;
        public Room ThisRoom;
        public Image CurrentSprite;
        public List<GameObject> targets;

        public void RegisterHit(int x, int y, int damage)
        {
            if (x >= X - XHitBox && x <= X + XHitBox && y >= YMin && y <= Y + YHitBox)
            {
                HP -= damage;
            }
        }

        public void MoveTo(int dx, int dy)
        {
            X += dx;
            Y += dy;
        }

        public void Attack()
        {

        }

    }

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
            X = x;
            Y = y;
            targets = new List<GameObject> { thisRoom.CurrentPlayer };
            XHitBox = 10;
            YHitBox = 32;
            YMin = 0;
            AttackCenter = new Point(X, Y + 18);
            AttackDirection = new Point(22, 0);
            Damage = 15;
            HP = 100;
            ThisRoom = thisRoom;
            ThisRoom.AddObject(this);
        }

        public void NextAction()
        {
            //if (ThisRoom.Person.X > )
        }
    }
}
