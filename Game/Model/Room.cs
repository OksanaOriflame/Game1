using Game.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Room
    {
        public readonly int Height;
        public readonly int Width;
        public readonly string Name;
        public List<GameObject> Enemies;
        public List<Floor> Floors;
        public List<Wall> Walls;
        public Player CurrentPlayer;
        public int State;
        public List<GameEvent> Events;
        public Dialog[] Dialogs;

        public Room(int width, int height, string name, Dialog[] dialogs)
        {
            Dialogs = dialogs;
            State = 0;
            Height = height;
            Width = width;
            Name = name;
            Enemies = new List<GameObject>();
            Floors = new List<Floor>();
            Walls = new List<Wall>();
            Events = new List<GameEvent>();
        }

        public void AddEvent(GameEvent gameEvent)
        {
            Events.Add(gameEvent);
        }
        public void AddObject(GameObject obj)
        {
            Enemies.Add(obj);
        }
        public void AddFloor(int y, int leftX, int rightX)
        {
            Floors.Add(new Floor() { Y = y, LeftX = leftX, RightX = rightX });
        }
        public void AddWall(int x, int topY, int bottomY)
        {
            Walls.Add(new Wall { X = x, TopY = topY, BottomY = bottomY });
        }
        public void AddNewPlayer(Player player)
        {
            CurrentPlayer = player;
            player.Targets = Enemies;
        }
        public void AddPlayer(Player player, Direction dir, bool isCheckPoint)
        {
            if (isCheckPoint)
            {

            }
            else if (dir == Direction.Right)
                player.X = 100;
            else
                player.X = 1300;
            CurrentPlayer = player;
            player.ThisRoom = this;
            player.Targets = Enemies;
        }
        public void RemovePlayer()
        {
            CurrentPlayer = null;
        }
        public static Room[] GetRooms()
        {
            var rooms = new Room[7];

            #region
            rooms[0] = new Room(1440, 840, "TheHall", new Dialog[2] 
            { 
                new Dialog(new string[] { 
                    "Ммммммм....", 
                    "...", 
                    "Мдааа", 
                    "Новая идея, значит...", 
                    "В этом древнем месте вы всегда исчезаете, не успев стать хоть сколько-нибудь состоятельными",
                    "А те, что остаются, сводят с ума своих создателей и занимают их место...",
                    "Какой милый фонарик у тебя в руках",
                    "Ты замечал, что люди всегда идут за тем, у кого свет?",
                    "Мы завидовали им. Нам даже в голову не приходило, что они могут вовсе не знать, куда нужно идти",
                    "Тогда я захотел найти свой свет, чтобы люди шли за мной",
                    "Я отдал всё, что у меня есть, в поисках этого света",
                    "...",
                    "Теперь я прикован к этой люстре",
                    "Иронично??? Иди к чёрту...",
                    "Полагаю, ты в замешательстве",
                    "Создатель этого места хотел дать тебе много возможностей...",
                    "Но он успел только пару комнат помимо этой",
                    "Пожалуйста, покинь меня"
                },
                new int[] {6, 8, 4 }),
                new Dialog(new string[]
                {
                    "Перемещение - клавиши \"A\" и \"D\"",
                    "Прыжок - клавиша \"Space\"",
                    "Атака - левая кнопка мыши",
                    "Уклонение - правая кнопка мыши",
                    "Пауза - клавиша \"Escape\"",
                    "Использовать медицинский трактат - клавиша \"R\"",
                    "Точка Возрождения"
                }, new int[2] {6, 1})
            });
            rooms[0].AddFloor(624, 0, 1500);
            rooms[0].AddWall(-40, 0, 900);
            rooms[0].AddWall(1440, 0, 900);
            
            #endregion  //room[0]

            rooms[1] = new Room(1440, 840, "TheHallRight", new Dialog[1]
                {
                    new Dialog(new string[]
                    {
                        "Неприятные парни. Ходят тут вечно",
                        "В буквальном смысле...",
                        "Местные мудрецы боятся перемен...",
                        "Любая новая идея подвергается сомнениям",
                        "Они настолько трусливы, что каждая их мысль обращается в оружие",
                        "Это место обречено на стагнацию...",
                        "Сам я родился не здесь",
                        "Меня поймали, когда я пытался стащить книгу",
                        "Теперь я за этой решёткой",
                        "В отличие от них я не настолько умен, чтобы жить вечно",
                        "Скорее бы смерть забрала меня...",
                        "Дальше проход закрыт",
                        "Создатель очень хотел сделать ещё много комнат",
                        "Несомненно, они вышли бы неплохо...",
                        "если бы он не уснул..."
                    }, new int[] { 2, 4, 3, 2, 4})
                });
            rooms[1].AddObject(new Knight(1200, 500, rooms[1]));
            rooms[1].AddObject(new Knight(900, 500, rooms[1]));
            rooms[1].AddFloor(624, 0, 1500);
            rooms[1].AddWall(-40, 0, 900);
            rooms[1].AddWall(1440, 0, 900);

            rooms[2] = new Room(1440, 840, "SecondRightHall", null);
            rooms[2].AddObject(new Blot(1200, 510, rooms[2]));
            rooms[2].AddObject(new Blot(700, 510, rooms[2]));
            rooms[2].AddObject(new Knight(900, 500, rooms[2]));
            rooms[2].AddFloor(624, 0, 1500);
            rooms[2].AddWall(-40, 0, 900);
            rooms[2].AddWall(1440, 0, 900);

            rooms[3] = new Room(1440, 840, "ThirdRightHall", null);
            rooms[3].AddFloor(624, 0, 1500);
            rooms[3].AddWall(-40, 0, 900);
            rooms[3].AddWall(1440, 0, 900);
            rooms[3].AddObject(new Statue(1250, 468, rooms[3]));
            rooms[3].AddObject(new Blot(850, 510, rooms[3]));
            rooms[3].AddObject(new Knight(675, 500, rooms[3]));
            rooms[3].AddObject(new Knight(1050, 500, rooms[3]));

            rooms[4] = new Room(1440, 840, "FourthRightHall", new Dialog[]
                {
                    new Dialog(new string[]
                    {
                        "Я больше не хочу мыслить",
                        "Не мыслю, следовательно, не существую",
                        "Молчание - золото"
                    }, new int[] { 1, 1, 1 }),
                    new Dialog(new string[]
                    {
                        "Мой хозяин решил, что это место устарело",
                        "Они не могли позволить ему уйти...",
                        "Тогда мой хозяин решил, что перемены возможны и здесь",
                        "Очень быстро ему объяснили, что это не так...",
                        "Он решил довольствоваться свободой своих собственных мыслей",
                        "Но в этом месте идеи материальны",
                        "На этот раз они заткнули голос его разума...",
                        "Я слабею вместе с рассудком моего хозяина",
                        "В этом месте я тебе не помощник",
                        "Дальше по коридору ты встретишь гнев местной художницы",
                        "Творческие неудачи оживили ярость в этих стенах",
                        "Если бы её цели были воплотимы..."
                    }, new int[] {2, 2, 3, 2, 3})
                });
            rooms[4].AddFloor(624, 0, 1500);
            rooms[4].AddWall(-40, 0, 900);
            rooms[4].AddWall(1440, 0, 900);

            rooms[5] = new Room(1440, 840, "FifthRightHall", new Dialog[]
                {
                    new Dialog( new string[]
                    { "Точка возрождения"}, new int[] { 1 })
                });
            rooms[5].AddFloor(624, 0, 1500);
            rooms[5].AddWall(-40, 0, 900);
            rooms[5].AddWall(1440, 0, 900);

            rooms[6] = new Room(1440, 840, "SixthRightHall", null);
            rooms[6].AddFloor(624, 0, 1500);
            rooms[6].AddWall(-40, 0, 900);
            rooms[6].AddWall(1440, 0, 900);
            rooms[6].AddObject(new Wrath(720, 0, rooms[6]));
            return rooms;
        }
    }

    public class Floor
    {
        public int Y;
        public int LeftX;
        public int RightX;
    }

    public class Wall
    {
        public int X;
        public int TopY;
        public int BottomY;
    }
}
