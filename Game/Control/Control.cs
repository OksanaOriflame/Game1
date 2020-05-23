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
        public static void UpdateRoom(Room room)
        {
            room.CurrentPlayer.Update();
        }
    }
}
