using Game.View;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class GameEvent
    {
        public int X;
        public int Y;
        public Action<TheGame> TheEvent;
        public GameEvent(int x, int y, Action<TheGame> gameEvent)
        {
            X = x;
            Y = y;
            TheEvent = gameEvent;
        }
        public static void FillEvents(TheGame game)
        {
            var rooms = game.Rooms;

            rooms[0].AddEvent(new GameEvent(240, 564, (x) => rooms[0].Dialogs[0].StartDialog(x)));
            rooms[0].AddEvent(new GameEvent(700, 564, (x) =>
            {
                rooms[0].Dialogs[1].StartDialog(x);
                game.Rooms[game.RoomI].CurrentPlayer.CreateCheckPoint();
            }));
            rooms[0].AddEvent(new GameEvent(1290, 564, (x) => MyForm.SwitchRooms(1, Direction.Right, false)));

            rooms[1].AddEvent(new GameEvent(100, 564, (x) => MyForm.SwitchRooms(0, Direction.Left, false)));
            rooms[1].AddEvent(new GameEvent(720, 564, (x) => rooms[1].Dialogs[0].StartDialog(x)));
            rooms[1].AddEvent(new GameEvent(1290, 564, (x) => MyForm.SwitchRooms(2, Direction.Right, false)));

            rooms[2].AddEvent(new GameEvent(100, 564, (x) => MyForm.SwitchRooms(1, Direction.Left, false)));
            rooms[2].AddEvent(new GameEvent(1290, 564, (x) => MyForm.SwitchRooms(3, Direction.Right, false)));

            rooms[3].AddEvent(new GameEvent(100, 564, (x) => MyForm.SwitchRooms(2, Direction.Left, false)));
            rooms[3].AddEvent(new GameEvent(1290, 564, (x) => MyForm.SwitchRooms(4, Direction.Right, false)));

            rooms[4].AddEvent(new GameEvent(100, 564, (x) => MyForm.SwitchRooms(3, Direction.Left, false)));
            rooms[4].AddEvent(new GameEvent(700, 564, (x) => rooms[4].Dialogs[0].StartDialog(x)));
            rooms[4].AddEvent(new GameEvent(950, 564, (x) => rooms[4].Dialogs[1].StartDialog(x)));
            rooms[4].AddEvent(new GameEvent(1290, 564, (x) => MyForm.SwitchRooms(5, Direction.Right, false)));

            rooms[5].AddEvent(new GameEvent(100, 564, (x) => MyForm.SwitchRooms(4, Direction.Left, false)));
            rooms[5].AddEvent(new GameEvent(700, 564, (x) =>
            {
                rooms[5].Dialogs[0].StartDialog(x);
                game.Rooms[game.RoomI].CurrentPlayer.CreateCheckPoint();
            }));
            rooms[5].AddEvent(new GameEvent(1290, 564, (x) => MyForm.SwitchRooms(6, Direction.Right, false)));
            
            rooms[6].AddEvent(new GameEvent(100, 564, (x) => MyForm.SwitchRooms(5, Direction.Left, false)));
        }
    }

    public class Dialog
    {
        public int CurrentLine;
        public int CurrentBigLine;
        public string[] Text;
        public int[] LinesCapacity;

        public Dialog(string[] text, int[] capas)
        {
            Text = text;
            CurrentLine = 0;
            CurrentBigLine = 0;
            LinesCapacity = capas;
        }

        public void StartDialog(TheGame game)
        {
            game.IsDialog = true;
            game.CurrentText = this;
        }
        public void Next(TheGame game)
        {
            CurrentLine++;
            var curLine = 0;
            for (var i = 0; i < CurrentBigLine; i++)
            {
                curLine += LinesCapacity[i];
            }
            if (CurrentLine >= curLine + LinesCapacity[CurrentBigLine])
            {
                CurrentBigLine++;
                if (CurrentBigLine >= LinesCapacity.Length)
                {
                    CurrentBigLine = 0;
                    CurrentLine = 0;
                }
                game.IsDialog = false;
                game.CurrentText = null;
            }
        }
    }
}
