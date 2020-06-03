using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.View;
using System.Drawing;

namespace Game.Model
{
    public class Player : GameObject
    {
        public int VerticalSpeed;
        public bool IsJumping;
        public bool WillJump;
        public int JumpingIterator;
        public int DodgeInterval;
        public int JumpCounter;
        public int MaxJumps;
        public Weapon CurrentWeapon;
        SpriteType PreviousSprite;
        Direction PreviousDirection;
        bool IsInvulnerable;
        int InvulnerabilityIterator;
        public int ST;
        public int MaxST;
        int DodgeST;
        public int STRegen;
        public int CheckPointRoom;
        public int JumpST;
        public int MaxHealingBooks;
        public int CurrentHealingBooks;
        public int[] HealingDurations;
        public int HealAmount;
        public Player(int x, int y, Room room)
        {
            ThisRoom = room;
            ObjType = ObjectType.Player;
            CurrentSprite = SpriteType.Stand;
            CurrentDirection = Direction.Right;
            PreviousSprite = CurrentSprite;
            PreviousDirection = CurrentDirection;
            MaxHP = 200;
            HP = MaxHP;
            MaxHealingBooks = 2;
            CurrentHealingBooks = MaxHealingBooks;
            HealingDurations = new int[2] { 400, 400 };
            HealAmount = 40;
            ST = 1000;
            MaxST = 1000;
            STRegen = 14;
            DodgeST = 300;
            X = x;
            Y = y;
            MaxJumps = 2;
            XHitBox = 15;
            YHitBox = 96;
            YMin = 0;
            IsJumping = false;
            WillJump = false;
            SpriteWidth = 96;
            ObjectName = "Player";
            State = 0;
            Speed = 15;
            HorizontalDirection = 0;
            VerticalSpeed = 0;
            Interval = 200;
            DodgeInterval = 60;
            AttackCenter = new Point(0, 48);
            IsImmortal = false;
            CurrentWeapon = Weapons.WeaponsList[0];
            XCorrection = CurrentWeapon.XCorrection;
            YCorrection = CurrentWeapon.YCorrection;
            CheckPointRoom = 0;
            JumpST = 450;
        }

        new public void Update()
        {
            if (CurrentInterval % (MyForm.Interval * 2) == 0)
            {
                var leftXPlayer = X - XHitBox;
                var rightXPlayer = X + XHitBox;
                var bottomYPlayer = Y + YHitBox;
                var intersections = Targets
                    .Select(target => target.GetIntersection(this, leftXPlayer, rightXPlayer, Y, bottomYPlayer))
                    .Where(target => target.Item1)
                    .ToArray();
                if (intersections.Length != 0)
                {
                    GetDamage(intersections[0].Item3, intersections[0].Item2);
                }
            }
            if (ST < MaxST && CurrentSprite != SpriteType.Attack && CurrentSprite != SpriteType.Dodge && !IsJumping)
            {
                ST = ST + STRegen;
                if (ST > MaxST)
                    ST = MaxST;
            }
            CurrentInterval += MyForm.Interval;
            if (IsInvulnerable)
            {
                InvulnerabilityIterator++;
                if (InvulnerabilityIterator < 15)
                {
                    var walls = ThisRoom.Walls
                    .Where(wall => (wall.TopY < Y + YHitBox && wall.BottomY > Y))
                    .Where(wall =>
                    {
                        var leftX = X;
                        var rightX = X + 5 * -(int)CurrentDirection;
                        if (leftX > rightX)
                        {
                            var temp = leftX;
                            leftX = rightX;
                            rightX = temp;
                        }
                        leftX -= 40;
                        rightX += 40;
                        return (leftX < wall.X && rightX > wall.X);
                    })
                    .ToArray();
                    if (walls.Length == 0)
                        X = X + 5 * -(int)CurrentDirection;
                }
                if (InvulnerabilityIterator == 15)
                {
                    CurrentSprite = SpriteType.Stand;
                    CurrentDirection = PreviousDirection;
                    HorizontalDirection = 0;
                    switch (PreviousSprite)
                    {
                        case SpriteType.Attack:
                            Attack();
                            break;
                        case SpriteType.Stand:
                            CurrentDirection = PreviousDirection;
                            SpeedUp(0);
                            break;
                        case SpriteType.Walk:
                            CurrentDirection = PreviousDirection;
                            SpeedUp(CurrentDirection == Direction.Left ? -1 : 1);
                            break;
                        case SpriteType.Dodge:
                            Dodge();
                            break;
                    }
                    if (WillJump)
                    {
                        WillJump = false;
                        Jump();
                    }
                }
                if (InvulnerabilityIterator == 35)
                    IsInvulnerable = false;
            }
            if (CurrentSprite == SpriteType.Hitted)
            {
                if (HP <= 0)
                {
                    MyForm.RefreshGame(this);
                }
            }
            else if (CurrentSprite == SpriteType.Healing)
            {
                if (CurrentInterval / HealingDurations[State] > 0)
                {
                    State++;
                    CurrentInterval = 0;
                    if (State == 2 || CurrentHealingBooks == 0)
                    {
                        State = 0;
                        CurrentSprite = SpriteType.Stand;
                        switch (PreviousSprite)
                        {
                            case SpriteType.Dodge:
                                Dodge();
                                break;
                            case SpriteType.Stand:
                                CurrentDirection = PreviousDirection;
                                SpeedUp(0);
                                break;
                            case SpriteType.Walk:
                                CurrentDirection = PreviousDirection;
                                SpeedUp(CurrentDirection == Direction.Left ? -1 : 1);
                                break;
                            case SpriteType.Attack:
                                Attack();
                                break;
                        }
                    }
                    if (State == 1)
                    {
                        CurrentHealingBooks--;
                        HP += HealAmount;
                        if (HP > MaxHP)
                            HP = MaxHP;
                    }
                }
            }
            else if (CurrentSprite == SpriteType.Attack)
            {
                if (CurrentInterval / CurrentWeapon.Durations[State] > 0)
                {
                    State++;
                    CurrentInterval = 0;
                    if (State >= CurrentWeapon.AnimationDuration)
                    {
                        State = 0;
                        CurrentSprite = SpriteType.Stand;
                        switch (PreviousSprite)
                        {
                            case SpriteType.Dodge:
                                Dodge();
                                break;
                            case SpriteType.Stand:
                                CurrentDirection = PreviousDirection;
                                SpeedUp(0);
                                break;
                            case SpriteType.Walk:
                                CurrentDirection = PreviousDirection;
                                SpeedUp(CurrentDirection == Direction.Left ? -1 : 1);
                                break;
                            case SpriteType.Attack:
                                Attack();
                                break;
                        }
                        if (WillJump)
                        {
                            WillJump = false;
                            Jump();
                        }
                    }
                    if (State == CurrentWeapon.DamageMoment && CurrentInterval == 0)
                        foreach (var enemy in ThisRoom.Enemies)
                            enemy.RegisterHit(X, Y, CurrentDirection, AttackCenter, CurrentWeapon);
                }
            }
            else if (CurrentSprite == SpriteType.Dodge)
            {
                if (CurrentInterval / DodgeInterval > 0)
                {
                    State++;
                    CurrentInterval = 0;
                }
                if (State == 4)
                {
                    IsImmortal = false;
                    State = 0;
                    Speed = 20;
                    HorizontalDirection = 0;
                    CurrentSprite = SpriteType.Stand;
                    switch (PreviousSprite)
                    {
                        case SpriteType.Attack:
                            Attack();
                            break;
                        case SpriteType.Stand:
                            CurrentDirection = PreviousDirection;
                            SpeedUp(0);
                            break;
                        case SpriteType.Walk:
                            CurrentDirection = PreviousDirection;
                            SpeedUp(CurrentDirection == Direction.Left ? -1 : 1);
                            break;
                    }
                    if (WillJump)
                    {
                        WillJump = false;
                        Jump();
                    }
                }
                
            }
            else
            {
                if (CurrentInterval / Interval > 0)
                {
                    State = (State + 1) % 4;
                    CurrentInterval = 0;
                }
            }
            if (CurrentSprite != SpriteType.Hitted)
            {
                var walls = ThisRoom.Walls
                    .Where(wall => (wall.TopY < Y + YHitBox && wall.BottomY > Y))
                    .Where(wall =>
                    {
                        var leftX = X;
                        var rightX = X + HorizontalDirection * Speed;
                        if (leftX > rightX)
                        {
                            var temp = leftX;
                            leftX = rightX;
                            rightX = temp;
                        }
                        leftX -= 40;
                        rightX += 40;
                        return (leftX < wall.X && rightX > wall.X);
                    })
                    .ToArray();
                if (walls.Length == 0)
                    X = X + HorizontalDirection * Speed;
            }
            if (IsJumping && (CurrentSprite!= SpriteType.Dodge))
            {
                VerticalSpeed += JumpingIterator / 2;
                JumpingIterator++;
                var floors = ThisRoom.Floors
                    .Where(floor => (floor.LeftX < X + 48) && (floor.RightX > X + 48))
                    .Where(floor => (floor.Y >= Y + YHitBox) && (floor.Y <= Y + 96 + VerticalSpeed))
                    .OrderBy(floor => floor.Y)
                    .ToArray();
                if (floors.Length != 0)
                {
                    VerticalSpeed = 0;
                    Y = floors[0].Y - 96;
                    IsJumping = false;
                    JumpCounter = 0;
                }
                else
                    Y += VerticalSpeed;
            }
        }
        #region
        public void SpeedUp(int dir)
        {
            if (CurrentSprite == SpriteType.Walk && dir == 0)
            {
                if (MyForm.IsLeft && CurrentDirection == Direction.Right)
                {
                    CurrentDirection = Direction.Left;
                    HorizontalDirection = -1;
                }
                else if (MyForm.IsRight && CurrentDirection == Direction.Left)
                {
                    CurrentDirection = Direction.Right;
                    HorizontalDirection = 1;
                }
                else if ((MyForm.IsLeft && CurrentDirection == Direction.Left) || (MyForm.IsRight && CurrentDirection == Direction.Right))
                {

                }
                else
                {
                    HorizontalDirection = 0;
                    CurrentSprite = SpriteType.Stand;
                }
            }
            else if (CurrentSprite == SpriteType.Attack || CurrentSprite == SpriteType.Dodge || CurrentSprite == SpriteType.Hitted || CurrentSprite == SpriteType.Healing)
            {
                PreviousSprite = dir == 0 ? SpriteType.Stand : SpriteType.Walk;
                PreviousDirection = dir == 0 ? CurrentDirection : (dir < 0 ? Direction.Left : Direction.Right);
            }
            else
            {
                CurrentDirection = dir != 0 ? (dir > 0 ? Direction.Right : Direction.Left) : CurrentDirection;
                HorizontalDirection = dir;
                CurrentSprite = dir == 0 ? SpriteType.Stand : SpriteType.Walk;
            }
        }
        public void Jump()
        {
            if (ST < JumpST / 2 || JumpCounter >= MaxJumps)
                return;
            if (CurrentSprite == SpriteType.Attack || CurrentSprite == SpriteType.Dodge || CurrentSprite == SpriteType.Hitted || CurrentSprite == SpriteType.Healing)
            {
                WillJump = true;
                return;
            }
            VerticalSpeed = -30;
            JumpCounter++;
            IsJumping = true;
            JumpingIterator = 0;
            ST -= JumpST;
            if (ST < 0)
                ST = 0;
        }
        
        public void Attack()
        {
            if (ST < CurrentWeapon.STReq / 5)
                return;
            if (IsJumping)
                return;
            if (CurrentSprite == SpriteType.Dodge || CurrentSprite == SpriteType.Attack || CurrentSprite == SpriteType.Hitted || CurrentSprite == SpriteType.Healing)
            {
                PreviousSprite = SpriteType.Attack;
                return;
            }
            State = 0;
            CurrentInterval = 0;
            HorizontalDirection = 0;
            PreviousSprite = CurrentSprite;
            PreviousDirection = CurrentDirection;
            CurrentSprite = SpriteType.Attack;
            ST -= CurrentWeapon.STReq;
            if (ST < 0)
                ST = 0;
        }

        public void Heal()
        {
            if (CurrentSprite == SpriteType.Attack || CurrentSprite == SpriteType.Death || CurrentSprite == SpriteType.Dodge || IsJumping)
                return;
            CurrentSprite = SpriteType.Healing;
            if (CurrentSprite == SpriteType.Walk)
                PreviousSprite = SpriteType.Walk;
            else
                PreviousSprite = SpriteType.Stand;
            HorizontalDirection = 0;
            PreviousDirection = CurrentDirection;
            CurrentInterval = 0;
            State = 0;
        }

        public void Dodge()
        {
            if (ST < DodgeST / 5)
                return;
            if (CurrentSprite == SpriteType.Attack || CurrentSprite == SpriteType.Hitted || CurrentSprite == SpriteType.Healing)
            {
                PreviousSprite = SpriteType.Dodge;
                return;
            }
            if (CurrentSprite == SpriteType.Walk)
                PreviousSprite = SpriteType.Walk;
            else
                PreviousSprite = SpriteType.Stand;
            PreviousDirection = CurrentDirection;
            CurrentSprite = SpriteType.Dodge;
            State = 0;
            HorizontalDirection = CurrentDirection == Direction.Right ? 1 : -1;
            Speed = 25;
            IsImmortal = true;
            ST -= DodgeST;
            if (ST < 0)
                ST = 0;
        }
        #endregion

        public void GetDamage(int damage, Direction dir)
        {
            if (IsImmortal || IsInvulnerable)
                return;
            if (CurrentSprite == SpriteType.Walk)
                PreviousSprite = SpriteType.Walk;
            else
                PreviousSprite = SpriteType.Stand;
            PreviousDirection = CurrentDirection;
            HP -= damage;
            InvulnerabilityIterator = 0;
            IsInvulnerable = true;
            CurrentSprite = SpriteType.Hitted;
            State = 0;
            CurrentInterval = 0;
            CurrentDirection =  (Direction)((int)dir * (-1));
        }

        public void CreateCheckPoint()
        {
            CheckPointRoom = MyForm.CurrentGame.RoomI;
            HP = MaxHP;
            CurrentHealingBooks = MaxHealingBooks;
            MyForm.RefreshGame(this);
        }
    }
}