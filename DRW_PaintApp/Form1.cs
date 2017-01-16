using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace DRW_PaintApp
{
    public partial class Form1 : Form
    {
        private Graphics contex;
        private Bitmap img;
        private ArrayList listOfPoints;
        private bool PencilDown;
        public Form1()
        {
            InitializeComponent();
            listOfPoints = new ArrayList();
            PencilDown = false;
            img = new Bitmap(this.Width,this.Height);
            contex = Graphics.FromImage(img);

            hScrollBar1.Visible = false;
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = new Point(e.X, e.Y);
                listOfPoints.Add(p);
                PencilDown = true;
                this.statusStrip1.Items[0].Text = "Mouse Pressing ON";
                Invalidate();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            PencilDown = false;
            this.statusStrip1.Items[0].Text = "Mouse Not Pressed";
            if(e.Button == MouseButtons.Left)
            {
                Point p = new Point(-1,-1);
                listOfPoints.Add(p);
                Invalidate();
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Graphics g = this.CreateGraphics();
            Point points = new Point(e.X, e.Y);
            Pen pencil = new Pen(Color.BlueViolet);

            if(PencilDown)
            {
                this.statusStrip1.Items[0].Text = "Mouse Moving";
                if (listOfPoints.Count > 1)
                    g.DrawLine(pencil, (Point)listOfPoints[listOfPoints.Count - 1],points);
                listOfPoints.Add(points);
            }
        }

        private void changeColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog dlg = new ColorDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
                this.ForeColor = dlg.Color;
        }

        private void penSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hScrollBar1.Visible = true;

        }
        private void Save(Control c, string file)
        {
            Graphics g = c.CreateGraphics();
            Bitmap picure = new Bitmap(c.Width, c.Height);
            c.DrawToBitmap(picure,new Rectangle(c.ClientRectangle.X,c.ClientRectangle.Y,Width,Height));
            picure.Save(file);
            picure.Dispose();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(this,"DRW Pictue.jpeg");
            MessageBox.Show("Image Saved Succesfully");
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = new Point(e.X, e.Y);
                listOfPoints.Add(p);
                Pen brush = new Pen(this.ForeColor);
                brush.Width = hScrollBar1.Value;
                if(PencilDown == true)
                {
                    contex.DrawLine(brush,(Point)listOfPoints[listOfPoints.Count - 2], (Point)listOfPoints[listOfPoints.Count - 1]);
                    Invalidate();
                }
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawImage(img, 0, 0);
        }
    }
}
