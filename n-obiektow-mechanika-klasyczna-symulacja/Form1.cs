using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using n_obiektow_mechanika_klasyczna_symulacja.scripts;

namespace n_obiektow_mechanika_klasyczna_symulacja
{
    public partial class Form1 : Form
    {
        private SilnikSymulacji silnikSymulacji;

        public Form1()
        {
            InitializeComponent();
            silnikSymulacji = new SilnikSymulacji(pictureBox1);
            silnikSymulacji.liczbaObiektów = (int)numericUpDown1.Value;
            silnikSymulacji.deltaT = (double)numericUpDown2.Value;
            silnikSymulacji.liczbaWatkow = (int)numericUpDown6.Value;
            silnikSymulacji.Reset();
        }

        private void start_stop_button_Click(object sender, EventArgs e)
        {
            if (start_stop_button.Text == "Stop")
            {
                start_stop_button.Text = "Start";
                reset_button.Enabled = true;
                silnikSymulacji.Stop();
            }
            else
            {
                start_stop_button.Text = "Stop";
                reset_button.Enabled = false;
                silnikSymulacji.Start();
            }
        }

        private void reset_button_Click(object sender, EventArgs e)
        {
            silnikSymulacji.Stop();
            silnikSymulacji.Reset();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (var punkt in silnikSymulacji.punktyMaterialne)
            {
                rysujKolo(g, punkt.r.x, punkt.r.y, punkt.radius);
            }
            label4.Text = silnikSymulacji.t.ToString();
        }

        private void rysujKolo(Graphics graphics, double centerX, double centerY, double radius)
        {
            Brush brush = new SolidBrush(Color.White);

            double x = centerX - radius;
            double y = centerY - radius;

            graphics.FillEllipse(brush, (float)x, (float)y, 2 * (float)radius, 2 * (float)radius);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.Stop();
            silnikSymulacji.liczbaObiektów = (int)numericUpDown1.Value;
            silnikSymulacji.Reset();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.liczbaWatkow = (int)numericUpDown6.Value;
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.deltaT = (double)numericUpDown2.Value;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            silnikSymulacji.destroy();
        }
    }
}
