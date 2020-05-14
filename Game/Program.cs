using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Game.Model;
using Game.View;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var player = new Player(100, 100);
            var room = new Room(1600, 900, "Hall1", player);
            var a = new PictureBox();
            
            a.Location = new Point(0, 0);
            a.Image = Image.FromFile(@"H:\IMKN\minder\graphics\concepts\sprites\big arts\hall.jpeg");
            

            var form = new Form();
            form.Width = 1600;
            form.Height = 900;
            form.Controls.Add(a);

            a.Dock = DockStyle.Fill;

            var timer = new Timer();

            timer.Interval = 100;
            timer.Tick += (x, y) => View.View.ReDraw(room);
            timer.Start();


            Application.Run(form);
        }
    }
}
