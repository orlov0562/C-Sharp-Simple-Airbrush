using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAirbrush
{
    public partial class Form1 : Form
    {

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern Int32 ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        public System.Drawing.Color GetPixelColor(IntPtr hwnd, int x, int y)
        {
            IntPtr hdc = GetDC(hwnd);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);
            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                        (int)(pixel & 0x0000FF00) >> 8,
                        (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }

        private Random _rnd = new Random();
        private SolidBrush sb = new SolidBrush(Color.FromArgb(128,0,0,255));
        private int panelBottomMargin;
        private int panelLeftMargin;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int radius = trackBar3.Value;
            int radius2 = radius * 2;

            Point mp = panel1.PointToClient(MousePosition);

            using (Graphics g = panel1.CreateGraphics())
            {
                double x;
                double y;

                for (int i = 0; i < trackBar2.Value; ++i)
                {
                    do
                    {
                        // Randomy select x,y so that 
                        // x falls between -radius..radius
                        // y falls between -radius..radius
                        x = (_rnd.NextDouble() * radius2) - radius;
                        y = (_rnd.NextDouble() * radius2) - radius;

                        // If x^2 + y^2 > r2 the point is outside the circle
                        // and a new point needs to be selected
                    } while ((x * x + y * y) > (radius * radius));

                    // Translate the point so that the center is at the mouse
                    // position
                    x += mp.X;
                    y += mp.Y;
                    g.FillEllipse(sb, new Rectangle((int)x - 1, (int)y - 1, trackBar4.Value, trackBar4.Value));

                }


                blur();

            }
        }

        private void blur()
        {
            /*
            int radius = trackBar3.Value;
            int radius2 = radius * 2;

            Point mp = this.PointToClient(MousePosition);

            using (Graphics g = this.CreateGraphics())
            {
// look at every pixel in the blur rectangle

                int brX = mp.X - radius2;
                int brW = mp.X + radius2;
                int brY = mp.Y - radius2;
                int brH = mp.Y + radius2;

                int blurSize = 10;

                for (Int32 xx = brX; xx < brX + brW; xx++)
                {
                    for (Int32 yy = brY; yy < brY + brH; yy++)
                    {
                        Int32 avgR = 0, avgG = 0, avgB = 0;
                        Int32 blurPixelCount = 0;

                        // average the color of the red, green and blue for each pixel in the
                        // blur size while making sure you don't go outside the image bounds
                        for (Int32 x = xx; (x < xx + blurSize && x < brW); x++)
                        {
                            for (Int32 y = yy; (y < yy + blurSize && y < brH); y++)
                            {
                                Color pixel = GetPixelColor(Handle, x, y);

                                avgR += pixel.R;
                                avgG += pixel.G;
                                avgB += pixel.B;

                                blurPixelCount++;
                            }
                        }

                        avgR = avgR/blurPixelCount;
                        avgG = avgG/blurPixelCount;
                        avgB = avgB/blurPixelCount;

                        // now that we know the average for the blur size, set each pixel to that color
                        SolidBrush bsb = new SolidBrush(Color.Black);
                        for (Int32 x = xx; x < xx + blurSize && x < brW && x < brW; x++)
                            for (Int32 y = yy; y < yy + blurSize && y < brH && y < brH; y++)
                            {

                                bsb.Color = Color.FromArgb(avgR, avgG, avgB);
                                g.FillEllipse(bsb, new Rectangle(x, y, 2, 2));

                            }
                    }
                }
            }
             */
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trackBar1_Scroll(null, null);
            trackBar2_Scroll(null, null);
            trackBar3_Scroll(null, null);
            trackBar4_Scroll(null, null);
            trackBar5_Scroll(null, null);
            sb.Color = Color.FromArgb(trackBar5.Value, sb.Color.R, sb.Color.G, sb.Color.B);
            updateColorLabel();
            panelBottomMargin = ClientSize.Height - panel1.Size.Height;
            panelLeftMargin = ClientSize.Width - panel1.Size.Width;
            panel1.BackColor = Color.White;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value;
            label1.Text = "Delay: " + trackBar1.Value;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            label2.Text = "Dots amount: " + trackBar2.Value;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            label3.Text = "Brush radius: " + trackBar3.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            label4.Text = "Dot size: " + trackBar4.Value;
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            label5.Text = "Transparency: " + trackBar5.Value;
            sb.Color = Color.FromArgb(trackBar5.Value, sb.Color.R, sb.Color.G, sb.Color.B);
            updateColorLabel();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                sb.Color = Color.FromArgb(trackBar5.Value, colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
                updateColorLabel();
            }
        }

        private void updateColorLabel()
        {
            label6.Text = sb.Color.ToString();
            label6.BackColor = sb.Color;
            label7.BackColor = Color.FromArgb(255, sb.Color);        
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear canvas?", "Clear canvas", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Graphics g = panel1.CreateGraphics();
                g.Clear(panel1.BackColor);
            }
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            string file = files[0];

            panel1.BackgroundImage = Image.FromFile(file);

        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            panel1.Size = new Size(ClientSize.Width - panelLeftMargin, ClientSize.Height - panelBottomMargin);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                timer1.Enabled = true;
            }
            else if (e.Button == MouseButtons.Right)
            {
                pickupColor();
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (panel1.ClientRectangle.Contains(panel1.PointToClient(Control.MousePosition)))
            {
                if ((Control.MouseButtons & MouseButtons.Right) != 0)
                {
                    pickupColor();
                }
            }

        }

        private void pickupColor()
        {
            Point mp = panel1.PointToClient(MousePosition);
            int x = mp.X;
            int y = mp.Y;
            Color c = GetPixelColor(panel1.Handle, x, y);
            sb.Color = Color.FromArgb(trackBar5.Value, c.R, c.G, c.B); ;
            updateColorLabel();
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clear canvas?", "Clear canvas", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                
                panel1.Invalidate();
                panel1.Refresh();
            }
        }
    }
}
