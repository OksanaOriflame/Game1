using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public enum States
    {
        Menu,
        Room
    }

    public class TheGame
    {
        public bool IsDialog;
        public bool IsEnding;
        public States State;
        public int RoomI;
        public int MenuI;
        public Room[] Rooms;
        public Menu[] Menues;
        public Dialog CurrentText;
        public float[] Resolutions;
        public string[] ResolutionsStrings;
        public int CurrentResolution;
        public int MaxResolution;

        public void ResolutionUp()
        {
            CurrentResolution++;
            if (CurrentResolution > MaxResolution)
                CurrentResolution = 0;
            ChangeFormRes();
        }
        public void ResolutionDown()
        {
            CurrentResolution--;
            if (CurrentResolution < 0)
                CurrentResolution = MaxResolution;
            ChangeFormRes();
        }
        public void ChangeFormRes()
        {
            if (CurrentResolution < 0 || CurrentResolution > 3)
                CurrentResolution = 0;
            MyForm.Stretch = Resolutions[CurrentResolution];
        }
    }
}
