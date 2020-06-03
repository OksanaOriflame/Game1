using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game.Model
{
    public class Menu
    {
        public static Menu[] GetMenues()
        {
            var menues = new Menu[3];

            menues[0] = new Menu(new MenuToken[3]
            {
                new MenuToken() {Str = "Начать игру" , Act = () => {
                    MyForm.CurrentGame.State = States.Room;
                    MyForm.CurrentGame.MenuI = 1;
                    } },
                new MenuToken() {Str = "Настройки", Act = () => MyForm.CurrentGame.MenuI = 2},
                new MenuToken() {Str = "Выход", Act = () => MyForm.CurrentGame.IsEnding = true}
            }, "The Game");

            menues[1] = new Menu(new MenuToken[3]
            {
                new MenuToken() { Str = "Возобновить игру" , Act = () => MyForm.CurrentGame.State = States.Room},
                new MenuToken() { Str = "Вернуться в главное меню", Act = () =>
                {
                    var game = MyForm.CurrentGame;
                    game.State = States.Menu;
                    game.MenuI = 0;
                    
                } },
                new MenuToken(){ Str = "Выйти из игры", Act = () => MyForm.CurrentGame.IsEnding = true}
            }, "Пауза");

            menues[2] = new Menu(new MenuToken[2]
            {
                new MenuToken() {Str = "Разрешение", KeyAct = (e) =>
                    {
                        if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.D)
                            MyForm.CurrentGame.ResolutionUp();
                        if (e.KeyCode == Keys.A)
                            MyForm.CurrentGame.ResolutionDown();
                    }},
                new MenuToken() {Str = "Назад", Act = () => MyForm.CurrentGame.MenuI = 0}
            }, "Настройки");

            return menues;
        }

        public string Title;
        public int CurrentToken;
        public MenuToken[] Tokens;
        public Menu(MenuToken[] tokens, string title)
        {
            Title = title;
            CurrentToken = 0;
            Tokens = tokens;
        }
        
        public void Down()
        {
            CurrentToken++;
            if (CurrentToken >= Tokens.Length)
                CurrentToken = 0;
        }

        public void Up()
        {
            CurrentToken--;
            if (CurrentToken < 0)
                CurrentToken = Tokens.Length - 1;
        }
    }

    public class MenuToken
    {
        public string Str;
        public Action Act;
        public Action<KeyEventArgs> KeyAct;
    }

    
}
