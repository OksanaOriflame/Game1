using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Game.Model;
using Game.View;
using System.IO;

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
            var room = new Room(1600, 900, "Hall", new Player(900, 528));
            room.AddFloor(624, 0, 1440);
            //room.AddObject(new Knight(300, 300, room));
            var form = new MyForm(room, 1440, 840);

            

            Application.Run(form);
        }
    }

    public class MyForm : Form
    {
        string imagePath = @"\Sprites\Locations\Hall";
        int i = 1;
        private Room CurrentRoom;
        public MyForm(Room room, int height, int width)
        {
            DoubleBuffered = true;
            Width = height;
            Height = width;
            CurrentRoom = room;

            var timer = new Timer();
            timer.Interval = 50;
            timer.Tick += (sender, args) => Invalidate();
            timer.Tick += (sender, args) => Control.Control.UpdateRoom(CurrentRoom);

            timer.Start();
        }

        //protected override void OnKeyPress(KeyPressEventArgs e)
        //{
        //    switch (e.KeyChar)
        //    {
        //        case 'd':
        //            CurrentRoom.CurrentPlayer.SpeedUp(1);
        //            break;
        //        case 'a':
        //            CurrentRoom.CurrentPlayer.SpeedUp(-1);
        //            break;
        //    }

        //}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D)
                CurrentRoom.CurrentPlayer.SpeedUp(1);
            switch (e.KeyCode)
            {
                //case Keys.D:
                //    CurrentRoom.CurrentPlayer.SpeedUp(1);
                //    break;
                case Keys.A:
                    CurrentRoom.CurrentPlayer.SpeedUp(-1);
                    break;
                case Keys.Space:
                    CurrentRoom.CurrentPlayer.Jump();
                    break;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D:
                    CurrentRoom.CurrentPlayer.SpeedUp(0);
                    break;
                case Keys.A:
                    CurrentRoom.CurrentPlayer.SpeedUp(0);
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            i = (i % 8 + 1);
            var path = Directory.GetCurrentDirectory();
            var imageSet = View.View.ImageSets;
            var bckgr = Image.FromFile(path + imagePath + @"\TheHall" + (i + 1) / 2 + ".jpeg");
            graphics.DrawImage(View.View.ImageSets[ObjectType.Room][SpriteType.Static][(i + 1) / 2 - 1], new Point(0, 0));
            graphics.DrawLine(new Pen(Color.White, 10), 0, CurrentRoom.Floors.First().Y, 1600, CurrentRoom.Floors.First().Y);
            foreach (var sprite in View.View.Sprites(CurrentRoom))
            {
                graphics.DrawImage(sprite.Image, sprite.Coordinates);
            }
        }
    }
}
