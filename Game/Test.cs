using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Game.Model;

namespace Game
{
    [TestFixture]
    class Test1
    {
        [Test]
        public void AttackTest()
        {
            var player = new Player(30, 20);
            var room = new Room(120, 70, "Hall1", player);
            var knight = new Knight(48, 20, room);
            knight.Attack(-1);
            Assert.AreEqual(85, player.HP);
        }

        [Test]
        public void MovementTest()
        {
            var player = new Player(30, 20);
            var room = new Room(120, 70, "Hall1", player);
            var knight = new Knight(48, 20, room);
            knight.MoveTo(20, 0);
            Assert.AreEqual(68, room.Enemies.FirstOrDefault().X);
        }
    }
}
