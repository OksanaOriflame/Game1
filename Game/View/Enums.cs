using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.View
{
    public enum ObjectType
    {
        Player = 0,
        Room = 1,
        Knight = 2,
        Blot = 3,
        Statue = 4,
        Wrath = 5
    }
    public enum SpriteType
    {
        Static,
        Stand,
        Walk,
        Attack,
        Hit,
        Hitted,
        Dodge,
        Death,
        Healing,
        A,
        B,
        C
    }
    public enum Direction
    {
        Left = -1,
        Right = 1
    }
    public enum WeaponType
    {
        Crusher = 0
    }
    
    public static class Enums
    {
        public static SpriteType GetType(string str)
        {
            switch (str)
            {
                case "Stand":
                    return SpriteType.Stand;
                case "Walk":
                    return SpriteType.Walk;
                case "Attack":
                    return SpriteType.Attack;
                case "Death":
                    return SpriteType.Death;
                case "Hitted":
                    return SpriteType.Hitted;
                case "A":
                    return SpriteType.A;
                case "B":
                    return SpriteType.B;
                case "C":
                    return SpriteType.C;
            }
            throw new Exception();
        }
    }
}
