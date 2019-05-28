using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        node[] diem;
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
           
          
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawLine(Pens.Red, 0, (float)vehinh(),200, 10);// sử dụng drawLine để vẽ đường
          
        }
        double vehinh ()
        {
            return Math.Sqrt(Math.Pow((Double)max(),2) + Math.Pow((Double)min(),2));
        }
        int min ()
        {
            int minn = diem[0].x;
            foreach(node d in diem)
            {
                if (minn >= d.x)
                    minn = d.x;
            }
            return minn;
        }
        int max ()
        {

            int maxx = diem[0].x;
            foreach (node d in diem)
            {
                if (maxx <= d.x)
                    maxx = d.x;
            }
            return maxx;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;
            node d = new node(x, y);
            diem = new[] { d };
            Button btn = new Button();
            btn.Location = new System.Drawing.Point(x, y);
            btn.Size = new System.Drawing.Size(10, 10);
            this.pictureBox1.Controls.Add(btn);
        }
    }
}
