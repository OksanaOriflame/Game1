using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Game.Model;

namespace Game.View
{
    public static class Drawer
    {
        static string imagePath = @"\Sprites\Locations\";
        public static int CurrentInterval;
        public static int i = 1;
        public static void Draw(PrivateFontCollection font, Graphics graphics, float stretch, float roomX, float roomY, Timer timer, Player player, TheGame game, Form form)
        {
            graphics.TranslateTransform(roomX, roomY);
            graphics.ScaleTransform(stretch, stretch);
            var path = Directory.GetCurrentDirectory();
            if (game.State == States.Room)
            {
                graphics.ResetTransform();
                graphics.TranslateTransform(roomX, roomY);
                graphics.ScaleTransform(stretch, stretch);
                CurrentInterval += timer.Interval;
                if (CurrentInterval / 50 > 0)
                {
                    i = (i % 8 + 1);
                    CurrentInterval = 0;
                }

                var imageSet = View.ImageSets;
                var bckgr = Image.FromFile(path + imagePath + player.ThisRoom.Name + @"\" + player.ThisRoom.Name + (i + 1) / 2 + ".jpeg");
                graphics.DrawImage(bckgr, new Point(0, 0));

                //for (var j = 0; j < game.Rooms[game.RoomI].Enemies.Count; j++)
                //{
                //    var x = game.Rooms[game.RoomI].Enemies[j].X + game.Rooms[game.RoomI].Enemies[j].AttackCenter.X;
                //    var y = game.Rooms[game.RoomI].Enemies[j].Y + game.Rooms[game.RoomI].Enemies[j].AttackCenter.Y;
                //    var enemy = game.Rooms[game.RoomI].Enemies[j];
                //    graphics.DrawLine(Pens.White, 
                //        enemy.X + enemy.AttackCenter.X, 
                //        enemy.Y + enemy.AttackCenter.Y, 
                //        enemy.X + enemy.AttackCenter.X + 500, 
                //        enemy.Y + enemy.AttackCenter.Y);
                //    graphics.DrawLine(Pens.White, x, y, x, y + 150);
                //}

                foreach (var sprite in View.Sprites(game.Rooms[game.RoomI]))
                {
                    graphics.DrawImage(sprite.SpriteImage, sprite.Coordinates);
                }

                if (player.ThisRoom.Enemies.Count == 0 && !game.IsDialog)
                {
                    var gameEvents = player.ThisRoom.Events.Where(e =>
                    {
                        var deltaX = Math.Abs(player.X - e.X);
                        return deltaX < 70;
                    })
                        .OrderBy(e => e.X)
                        .ToArray();
                    if (gameEvents.Length != 0)
                    {
                        var e = gameEvents.First();
                        graphics.DrawImage(Image.FromFile(path + @"\Sprites\Buttons\E.png"), new PointF(e.X, e.Y + 100));
                    }
                }

                if (game.IsDialog)
                {
                    
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;
                    var rect = new Rectangle(210, 690, player.ThisRoom.Width - (210 * 2), player.ThisRoom.Height - 740);
                    graphics.DrawImage(Image.FromFile(path + @"\Sprites\Buttons\LMB.png"), new PointF(rect.X - 64, rect.Y));
                    graphics.DrawString(game.CurrentText.Text[game.CurrentText.CurrentLine], new Font(font.Families[0], 35), Brushes.White, rect, format);
                }

                var point = new Point(100, 77);
                var HPpercent = (float)player.HP / (float)player.MaxHP;
                var STpercent = (float)player.ST / (float)player.MaxST;

                graphics.DrawImage(Image.FromFile(path + @"\Sprites\Interface\Bars.png"), point);
                graphics.FillRectangle(Brushes.Red, new RectangleF(point.X + 35, point.Y + 26, 224f * HPpercent, 8));
                graphics.FillRectangle(Brushes.Green, new RectangleF(point.X + 35, point.Y + 53, 224f * STpercent, 8));
                graphics.DrawString(player.CurrentHealingBooks.ToString(), new Font(font.Families[0], 35), Brushes.White, point.X + 35, point.Y + 100);
            }
            else if (game.State == States.Menu)
            {
                graphics.ResetTransform();
                graphics.DrawImage(Image.FromFile(path + @"\Sprites\BackGround\BackGround.jpg"), new PointF(0, 0));
                var itemsCount = game.Menues[game.MenuI].Tokens.Length;
                var menu = game.Menues[game.MenuI];

                StringFormat format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                var currentToken = game.Menues[game.MenuI].CurrentToken;
                var first = new Rectangle(100, 100, form.ClientSize.Width - 200, form.ClientSize.Height / 4);
                var top = form.ClientSize.Height - 200 - first.Height;
                var tokenHeight = (top) / menu.Tokens.Length;

                graphics.DrawString(menu.Title, new Font(font.Families[0], 120), Brushes.White, first, format);

                var others = new Rectangle[menu.Tokens.Length];
                for (var i = 0; i < others.Length; i++)
                {
                    var str = menu.Tokens[i].Str;
                    if (game.MenuI == 2 && i == 0)
                        str += "   " + game.ResolutionsStrings[game.CurrentResolution];
                    others[i] = new Rectangle(100, 100 + first.Height + i * tokenHeight, form.ClientSize.Width - 200, tokenHeight);
                    graphics.DrawString(str, new Font(font.Families[0], 70), Brushes.White, others[i], format);
                }
                graphics.FillEllipse(Brushes.White, others[currentToken].X + 100, others[currentToken].Y + (tokenHeight / 3), tokenHeight / 3, tokenHeight / 3);
            }
        }
    }
}
