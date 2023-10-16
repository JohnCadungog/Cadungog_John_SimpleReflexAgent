using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace Cadungog_John_SimpleReflexAgent
{
    public partial class Form1 : Form
    {
        private List<Room> rooms = new List<Room>();
        private List<Point> roomPoints = new List<Point>();
        private VacuumCleaner vacuumCleaner;
        private Random random = new Random();
        private int centerX;
        private int centerY;
        private Grid grid;

        public Form1()
        {
            InitializeComponent();
            InitializeComponents();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DrawRoomGrid(g);



            grid = new Grid(this.ClientSize.Width, this.ClientSize.Height);
            DrawRooms(g);
            DrawVacuumCleaner(g);
        }

        private void InitializeComponents()
        {
            centerX = this.ClientSize.Width / 2;
            centerY = this.ClientSize.Height / 2;

            InitializeRoomPoints();
            CreateRooms();
            InitializeRoomStates();
            InitializeVacuumCleaner();

            vacuumCleaner.NoOpTimer.Tick += (sender, e) => Refresh();
            vacuumCleaner.CleanTimer.Tick += (sender, e) => Refresh();


        }

        private void InitializeRoomPoints()
        {
            roomPoints.Add(new Point(centerX / 2, centerY / 2));
            roomPoints.Add(new Point(this.ClientSize.Width - this.ClientSize.Width / 4, centerY / 2));
            roomPoints.Add(new Point(centerX / 2, this.ClientSize.Height - this.ClientSize.Height / 4));
            roomPoints.Add(new Point(this.ClientSize.Width - this.ClientSize.Width / 4, this.ClientSize.Height - this.ClientSize.Height / 4));
        }

        private void CreateRooms()
        {
            for (int i = 0; i < 4; i++)
            {
                rooms.Add(new Room((Location)i)
                {
                    roomPoint = roomPoints[i]
                });
            }
        }

        private void InitializeRoomStates()
        {
            foreach (Room room in rooms)
            {
                room.roomState = (State)random.Next(0, 2);
                if (room.roomState == State.Clean)
                    room.DirtTimer.Start();
            }
        }

        private void InitializeVacuumCleaner()
        {
            int randomNumber = random.Next(0, 4);
            vacuumCleaner = new VacuumCleaner(rooms, rooms[randomNumber]);
        }

        private void DrawRoomGrid(Graphics g)
        {
            Pen myPen = new Pen(Color.Gray);
            g.DrawLine(myPen, centerX, 0, centerX, this.ClientSize.Height);
            g.DrawLine(myPen, 0, centerY, this.ClientSize.Width, centerY);
        }


        private void DrawRooms(Graphics g)
        {
            Bitmap dirtyRoomImage = Properties.Resources.dirty;
            Brush labelBrush = new SolidBrush(Color.Black);
            Font font = new Font(new FontFamily("Open Sans"), 20, FontStyle.Bold);
            Font roomStateFont = new Font(new FontFamily("Open Sans"), 11, FontStyle.Regular);

            foreach (Room room in rooms)
            {
                if (room.roomState == State.Dirty)
                {

                    int desiredWidth = 100;
                    int desiredHeight = 100;


                    int imageX = room.roomPoint.X - (desiredWidth / 2);
                    int imageY = room.roomPoint.Y - (desiredHeight / 2);


                    int maxX = this.ClientSize.Width - desiredWidth;
                    int maxY = this.ClientSize.Height - desiredHeight;
                    imageX = Math.Max(imageX, 0);
                    imageY = Math.Max(imageY, 0);
                    imageX = Math.Min(imageX, maxX);
                    imageY = Math.Min(imageY, maxY);


                    g.DrawImage(dirtyRoomImage, new Rectangle(imageX, imageY, desiredWidth, desiredHeight));
                }

                g.DrawString(" " + room.roomName, font, labelBrush, room.roomPoint);
                g.DrawString("\n\n" + room.roomState, roomStateFont, labelBrush, room.roomPoint);

            }
        }





        private void DrawVacuumCleaner(Graphics g)
        {

            Bitmap vacuumImage;

            if (vacuumCleaner.NoOpTimer.Enabled)
            {
                vacuumImage = Properties.Resources.NoOP;
            }
            else if (vacuumCleaner.CleanTimer.Enabled)
            {
                vacuumImage = Properties.Resources.Cleaned;
            }
            else
            {
                vacuumImage = Properties.Resources.Transfering;
            }

            int desiredWidth = 100;
            int desiredHeight = 100;


            int imageX = vacuumCleaner.Position.X - (desiredWidth / 2);
            int imageY = vacuumCleaner.Position.Y - (desiredHeight / 2);

            g.DrawImage(vacuumImage, new Rectangle(imageX, imageY, desiredWidth, desiredHeight));
        }




        private void Update(object sender, EventArgs e)
        {
            label2.Text = vacuumCleaner.Act(rooms).ToString();
            Refresh();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        
    }
}
