using n_obiektow_mechanika_klasyczna_symulacja.scripts;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace n_obiektow_mechanika_klasyczna_symulacja
{
    public partial class Form1 : Form
    {
        private SilnikSymulacji silnikSymulacji;
        Brush brush = new SolidBrush(Color.White);

        public Form1()
        {
            InitializeComponent();
            silnikSymulacji = new SilnikSymulacji(pictureBox1);
            silnikSymulacji.liczbaObiektów = (int)numericUpDown1.Value;
            silnikSymulacji.deltaT = (double)numericUpDown2.Value;
            silnikSymulacji.liczbaWatkow = (int)numericUpDown6.Value;
            silnikSymulacji.czyOdpychanieWlaczone = checkBox3.Checked;
            silnikSymulacji.czyKolizjaWlaczona = checkBox2.Checked;
            silnikSymulacji.tlumienie = (double)numericUpDown7.Value;
            silnikSymulacji.czyOgraniczonaPrzestrzen = checkBox1.Checked;
            silnikSymulacji.opoznienie = (int)numericUpDown5.Value;
            silnikSymulacji.minMasa = (int)numericUpDown3.Value;
            silnikSymulacji.maxMasa = (int)numericUpDown4.Value;
            silnikSymulacji.minRadius = (int)numericUpDown8.Value;
            silnikSymulacji.maxRadius = (int)numericUpDown9.Value;
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

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            silnikSymulacji.czyOdpychanieWlaczone = checkBox3.Checked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            silnikSymulacji.czyKolizjaWlaczona = checkBox2.Checked;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.tlumienie = (double)numericUpDown7.Value;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            silnikSymulacji.czyOgraniczonaPrzestrzen = checkBox1.Checked;
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.opoznienie = (int)numericUpDown5.Value;
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.minMasa = (int)numericUpDown3.Value;
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.maxMasa = (int)numericUpDown4.Value;
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.maxRadius = (int)numericUpDown9.Value;
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            silnikSymulacji.minRadius = (int)numericUpDown8.Value;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }
    }
}
