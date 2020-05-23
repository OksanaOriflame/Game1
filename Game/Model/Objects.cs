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
    public class Room
    {
        readonly int Height;
        readonly int Width;
        readonly string Name;
        public List<GameObject> Enemies;
        public List<Floor> Floors;
        public Player CurrentPlayer;
        public int State;

        public Room (int width, int height, string name, Player player)
        {
            State = 0;
            CurrentPlayer = player;
            CurrentPlayer.ThisRoom = this;
            Height = height;
            Width = width;
            Name = name;
            Enemies = new List<GameObject>();
            Floors = new List<Floor>();
            player.targets = Enemies;
        }

        public void AddObject(GameObject obj)
        {
            Enemies.Add(obj);
        }
        public void AddFloor(int y, int leftX, int rightX)
        {
            Floors.Add(new Floor() { Y = y, LeftX = leftX, RightX = rightX });
        }
    }

    public class Floor
    {
        public int Y;
        public int LeftX;
        public int RightX;
    }

    public class GameObject
    {
        public int HorizontalDirection;
        public string ObjectName;
        public int X;
        public int Y;
        public int XHitBox;
        public int YHitBox;
        public int YMin;
        public int HP;
        public int Damage;
        public int SpriteWidth;
        public Point AttackCenter;
        public Point AttackDirection;
        public Room ThisRoom;
        public List<GameObject> targets;
        public ObjectType ObjType;
        public SpriteType CurrentSprite;
        public Direction CurrentDirection;
        public int State;

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

    
}
