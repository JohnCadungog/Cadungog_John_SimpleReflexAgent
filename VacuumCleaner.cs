using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cadungog_John_SimpleReflexAgent
{

    enum Action
    {
        Left, Right, Up, Down, Clean, NoOp
    }

    internal class VacuumCleaner
    {
        public Room currentRoom { get; set; }
        public Timer NoOpTimer;
        public Timer MoveTimer;
        public Timer CleanTimer;
        public Point Position;
        public Action CurrentAction;
        public List<Room> rooms;
        public int MoveSpeed = 10;
        int time = 0;

        public VacuumCleaner(List<Room> rooms, Room currentRoom)
        {
            this.currentRoom = currentRoom;
            Position = currentRoom.roomPoint;
            this.rooms = rooms;

            NoOpTimer = new Timer();
            NoOpTimer.Interval = 20;
            NoOpTimer.Tick += AnimateSleep;
            NoOpTimer.Enabled = false;

            MoveTimer = new Timer();
            MoveTimer.Interval = 20;
            MoveTimer.Tick += AnimateMove;
            MoveTimer.Enabled = false;

            CleanTimer = new Timer();
            CleanTimer.Interval = 20;
            CleanTimer.Tick += AnimateClean;
            CleanTimer.Enabled = false;
        }

        Random random = new Random();

        public Action Act(List<Room> rooms)
        {
            bool hasTimerEnabled = MoveTimer.Enabled ||
                                    NoOpTimer.Enabled ||
                                    CleanTimer.Enabled;
            if (!hasTimerEnabled)
            {
                if (currentRoom.roomState == State.Dirty)
                {
                    StartClean();
                }
                else if (currentRoom.roomState == State.Clean)
                {
                    if (random.Next(0, 10) <= 6)
                    {
                        StartMove();
                    }
                    else
                        StartNoOp();
                }
            }

            return CurrentAction;
        }

        private Action Clean()
        {
            currentRoom.roomState = State.Clean;
            currentRoom.DirtTimer.Start();
            currentRoom.time = 0;

            return Action.Clean;
        }

        // Clean ---------------------------------
        private Action StartClean()
        {
            time = 0;
            CleanTimer.Start();
            CleanTimer.Enabled = true;
            CurrentAction = Action.Clean;

            return CurrentAction;
        }

        private void AnimateClean(object sender, EventArgs e)
        {
            time++;
            if (time >= 10)
            {
                CleanTimer.Stop();
                CleanTimer.Enabled = false;
                Clean();
            }
        }

        // Sleep ---------------------------------
        // starter for NoOpTimer 
        private Action StartNoOp()
        {
            time = 0;
            NoOpTimer.Start();
            NoOpTimer.Enabled = true;
            CurrentAction = Action.NoOp;

            return CurrentAction;
        }

        // callback for NoOpTimer
        private void AnimateSleep(object o, EventArgs a)
        {
            time++;

            if (time == 30)
            {
                NoOpTimer.Stop();
                NoOpTimer.Enabled = false;
            }
        }

        // Move ---------------------------------
        // starter for MoveTimer
        private Action StartMove()
        {
            // choose direction

            time = 0;
            MoveTimer.Start();
            MoveTimer.Enabled = true;

            if (currentRoom.roomName == Location.A)
            {
                if (random.Next(0, 2) == 0)
                {
                    CurrentAction = Action.Right;
                    currentRoom = rooms[(int)Location.B];
                }
                else
                {
                    CurrentAction = Action.Down;
                    currentRoom = rooms[(int)Location.C];
                }
            }
            else if (currentRoom.roomName == Location.B)
            {
                if (random.Next(0, 2) == 0)
                {
                    CurrentAction = Action.Left;
                    currentRoom = rooms[(int)Location.A];
                }
                else
                {
                    CurrentAction = Action.Down;
                    currentRoom = rooms[(int)Location.D];
                }
            }
            else if (currentRoom.roomName == Location.C)
            {
                if (random.Next(0, 2) == 0)
                {
                    CurrentAction = Action.Up;
                    currentRoom = rooms[(int)Location.A];
                }
                else
                {
                    CurrentAction = Action.Right;
                    currentRoom = rooms[(int)Location.D];
                }
            }
            else if (currentRoom.roomName == Location.D)
            {
                if (random.Next(0, 2) == 0)
                {
                    CurrentAction = Action.Up;
                    currentRoom = rooms[(int)Location.B];
                }
                else
                {
                    CurrentAction = Action.Left;
                    currentRoom = rooms[(int)Location.C];
                }
            }

            return CurrentAction;
        }

        // callback for MoveTimer
        private void AnimateMove(object sender, EventArgs e)
        {
            time++;
            if (time == 20)
            {
                MoveTimer.Stop();
                MoveTimer.Enabled = false;
            }

            double movementFactor = 0.05;

            if (CurrentAction == Action.Left)
            {
                AnimateLeft(movementFactor);
            }
            else if (CurrentAction == Action.Right)
            {
                AnimateRight(movementFactor);
            }
            else if (CurrentAction == Action.Down)
            {
                AnimateDown(movementFactor);
            }
            else if (CurrentAction == Action.Up)
            {
                AnimateUp(movementFactor);
            }
        }

        private void AnimateDown(double movementFactor)
        {
            int distance = rooms[2].roomPoint.Y - rooms[0].roomPoint.Y;
            Position = new Point(Position.X, Position.Y + (int)(distance * movementFactor));
        }

        private void AnimateUp(double movementFactor)
        {
            int distance = rooms[2].roomPoint.Y - rooms[0].roomPoint.Y;
            Position = new Point(Position.X, Position.Y - (int)(distance * movementFactor));
        }

        private void AnimateRight(double movementFactor)
        {
            int distance = rooms[1].roomPoint.X - rooms[0].roomPoint.X;
            Position = new Point(Position.X + (int)(distance * movementFactor), Position.Y);
        }

        private void AnimateLeft(double movementFactor)
        {
            int distance = rooms[1].roomPoint.X - rooms[0].roomPoint.X;
            Position = new Point(Position.X - (int)(distance * movementFactor), Position.Y);
        }

    }
}
