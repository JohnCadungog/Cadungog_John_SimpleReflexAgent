using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Cadungog_John_SimpleReflexAgent
{
    enum Location
    {
        A, B, C, D
    }

    internal class Room
    {
        public State roomState;
        public Point roomPoint = new Point();
        public Location roomName { get; set; }
        public Timer DirtTimer { get; set; }
        public int time = 0;

        public Room(Location location)
        {
            DirtTimer = new Timer();
            DirtTimer.Interval = 20;
            DirtTimer.Tick += CallBack;
            roomName = location;
        }

        public void CallBack(object o, EventArgs e)
        {
            if (roomState == State.Clean)
                time++;

            if (time == 180)
            {
                DirtTimer.Stop();
                roomState = State.Dirty;
            }
        }

        public override string ToString()
        {
            return roomName.ToString();
        }
    }
}
