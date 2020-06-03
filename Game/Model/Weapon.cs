using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.View;

namespace Game.Model
{
    public class Weapons
    {
        public static List<Weapon> WeaponsList = new List<Weapon>()
        {
            new Weapon(4, 100, 140, WeaponType.Crusher, new int[] { 100, 100, 100, 200 }, 2, -144, -96, 400)
        };
    }
    public class Weapon
    {
        public WeaponType Type;
        public int Damage;
        public int AnimationDuration;
        public int Length;
        public int[] Durations;
        public int DamageMoment;
        public int XCorrection;
        public int YCorrection;
        public int STReq;
        public Weapon(int animationDuration, int damage, int length, WeaponType type, int[] durs, int damageMoment, int xCor, int yCor, int stReq)
        {
            Type = type;
            Damage = damage;
            AnimationDuration = animationDuration;
            if (AnimationDuration != durs.Length)
                throw new Exception();
            Durations = durs;
            Length = length;
            DamageMoment = damageMoment;
            XCorrection = xCor;
            YCorrection = yCor;
            STReq = stReq;
        }
    }
}
