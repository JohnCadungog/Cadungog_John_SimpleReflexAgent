using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cadungog_John_SimpleReflexAgent
{
    public class Grid
    {
        private int width;
        private int height;

        public Grid(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Draw(Graphics g)
        {


            Pen pen = new Pen(Color.Gray, 6);

            int middleX = width / 2;
            int middleY = height / 2;

            Point vStartPoint = new Point(middleX, 0);
            Point vEndPoint = new Point(middleX, height);
            Point hStartPoint = new Point(0, middleY - 6);
            Point hEndPoint = new Point(width, middleY - 6);

            g.DrawLine(pen, hStartPoint, hEndPoint);
            g.DrawLine(pen, vStartPoint, vEndPoint);

            pen.Dispose();
        }
    }
}
