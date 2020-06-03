using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Game.Model;
using Game.View;
using System.IO;
using System.Drawing.Text;

namespace Game
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            View.View.InitializeSets();
            var game = new TheGame()
            { 
                Rooms = Room.GetRooms(),
                State = States.Menu,
                RoomI = 0,
                MenuI = 0,
                IsDialog = false,
                IsEnding = false,
                Resolutions = new float[] { 0.91f, 1f, 1.27f, 0.85f },
                ResolutionsStrings = new string[]
                {
                    "1366x768",
                    "1440x840",
                    "1920x1080",
                    "720x1080"
                },
                CurrentResolution = 0,
                MaxResolution = 3
            };
            GameEvent.FillEvents(game);
            MyForm.CurrentGame = game;
            game.Menues = Model.Menu.GetMenues();
            var form = new MyForm(game);
            Control.Control.DeadList = new Stack<GameObject>();
            

            Application.Run(form);
        }
    }

    public class MyForm : Form
    {
        public static bool IsLeft;
        public static bool IsRight;
        Timer timer;
        public static int Interval = 20;
        public static Room CurrentRoom;
        public static float Stretch;
        public static TheGame CurrentGame;

        public MyForm(TheGame game)
        {
            CurrentGame = game;
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            BackColor = Color.Black;
            DoubleBuffered = true;
            IsLeft = false;
            IsRight = false;
            CurrentRoom = game.Rooms[game.RoomI];
            CurrentRoom.AddNewPlayer(new Player(1030, 528, CurrentRoom));
            Stretch = 1f;
            Cursor.Hide();

            timer = new Timer();
            timer.Interval = Interval;
            timer.Tick += (sender, args) => Invalidate();
            timer.Tick += (sender, args) =>
                {
                    if (CurrentGame.State == States.Room)
                        Control.Control.UpdateRoom(CurrentRoom);
                };
            timer.Start();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (CurrentGame.State == States.Room)
            {
                switch (e.KeyCode)
                {
                    case Keys.R:
                        CurrentRoom.CurrentPlayer.Heal();
                        break;
                    case Keys.D:
                        IsRight = true;
                        CurrentRoom.CurrentPlayer.SpeedUp(1);
                        break;
                    case Keys.A:
                        IsLeft = true;
                        CurrentRoom.CurrentPlayer.SpeedUp(-1);
                        break;
                    case Keys.Space:
                        CurrentRoom.CurrentPlayer.Jump();
                        break;
                    case Keys.Escape:
                        CurrentGame.State = States.Menu;
                        break;
                    case Keys.E:
                        if (CurrentRoom.Enemies.Count == 0)
                        {
                            var gameEvents = CurrentRoom.Events.Where(eve =>
                            {
                                var deltaX = Math.Abs(CurrentRoom.CurrentPlayer.X - eve.X);
                                return deltaX < 70;
                            })
                        .OrderBy(eve => eve.X)
                        .ToArray();
                            if (gameEvents.Length != 0)
                            {
                                var eve = gameEvents.First();
                                eve.TheEvent(CurrentGame);
                            }
                        }
                        break;
                }
            }
            else
            {
                if ((e.KeyCode == Keys.A || e.KeyCode == Keys.D || e.KeyCode == Keys.Enter) &&
                    CurrentGame.Menues[CurrentGame.MenuI].CurrentToken == 0 && CurrentGame.MenuI == 2)
                {
                    CurrentGame.Menues[CurrentGame.MenuI].Tokens[CurrentGame.Menues[CurrentGame.MenuI].CurrentToken].KeyAct(e);
                    return;
                }
                switch (e.KeyCode)
                {
                    case Keys.S:
                        CurrentGame.Menues[CurrentGame.MenuI].Down();
                        break;
                    case Keys.W:
                        CurrentGame.Menues[CurrentGame.MenuI].Up();
                        break;
                    case Keys.Enter:
                        CurrentGame.Menues[CurrentGame.MenuI].Tokens[CurrentGame.Menues[CurrentGame.MenuI].CurrentToken].Act();
                        break;
                }
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (CurrentGame.IsDialog)
            {
                CurrentGame.CurrentText.Next(CurrentGame);
                return;
            }
            if (CurrentGame.State == States.Room)
            {
                if (e.Button == MouseButtons.Left)
                    CurrentRoom.CurrentPlayer.Attack();
                else if (e.Button == MouseButtons.Right)
                    CurrentRoom.CurrentPlayer.Dodge();
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (CurrentGame.State == States.Room)
            {
                switch (e.KeyCode)
                {
                    case Keys.D:
                        IsRight = false;
                        CurrentRoom.CurrentPlayer.SpeedUp(0);
                        break;
                    case Keys.A:
                        IsLeft = false;
                        CurrentRoom.CurrentPlayer.SpeedUp(0);
                        break;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var player = CurrentRoom.CurrentPlayer;
            var graphics = e.Graphics;
            var roomX = (ClientSize.Width - CurrentRoom.Width * Stretch) / 2;
            var roomY = (ClientSize.Height - CurrentRoom.Height * Stretch) / 2;
            var font = new PrivateFontCollection();
            font.AddFontFile(@"Fonts\18812.ttf");
            Drawer.Draw(font, graphics, Stretch, roomX, roomY, timer, player, CurrentGame, this);
            if (CurrentGame.IsEnding)
            {
                timer.Stop();
                this.Close();
            }
        }

        public static void SwitchRooms(int roomNumber, Direction dir, bool isNewGame)
        {
            CurrentGame.Rooms[roomNumber].AddPlayer(CurrentRoom.CurrentPlayer, dir, isNewGame);
            CurrentRoom.RemovePlayer();
            CurrentRoom = CurrentGame.Rooms[roomNumber];
            CurrentGame.RoomI = roomNumber;
        }

        public static void RefreshGame(Player player)
        {
            CurrentGame.Rooms[CurrentGame.RoomI].RemovePlayer();
            player.HP = player.MaxHP;
            CurrentGame.Rooms = Room.GetRooms();
            GameEvent.FillEvents(CurrentGame);
            CurrentGame.Rooms[player.CheckPointRoom].AddPlayer(player, default, true);
            CurrentGame.RoomI = player.CheckPointRoom;
            CurrentRoom = CurrentGame.Rooms[player.CheckPointRoom];
        }
    }
}
