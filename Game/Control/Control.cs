using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.Model;

namespace Game.Control
{
    public static class Control
    {
        public static Stack<GameObject> DeadList;
        public static void UpdateRoom(Room room)
        {
            room.CurrentPlayer.Update();
            foreach (var enemie in room.Enemies.Where(enemy => !enemy.IsBoss))
                enemie.Update();
            foreach (var boss in room.Enemies.Where(enemy => enemy.IsBoss))
                ((Wrath)boss).UpdateBoss();
            while (DeadList.Count > 0)
                room.Enemies.Remove(DeadList.Pop());
        }
        
    }
}
