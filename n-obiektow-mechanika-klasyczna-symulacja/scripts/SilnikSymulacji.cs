using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace n_obiektow_mechanika_klasyczna_symulacja.scripts
{
    internal class SilnikSymulacji
    {
        public PunktMaterialny[] punktyMaterialne;
        private PictureBox pictureBox;
        private bool czySymulacjaTrwa;
        private double deltaT;
        private int liczbaObiektów;
        private const double G = 6.67408;
        public double t;
        

        public SilnikSymulacji(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            czySymulacjaTrwa = false;

            Reset();
        }

        public void Reset(int liczbaObiektów = 3, double dT = 0.01)
        {
            this.liczbaObiektów = liczbaObiektów;
            this.deltaT = dT;
            this.t = 0;

            punktyMaterialne = new PunktMaterialny[liczbaObiektów];
            Random random = new Random();

            /*
            this.liczbaObiektów = 3;
            punktyMaterialne[0] = new PunktMaterialny(1000000000000000000f, new Wektor(100, 100, 0), new Wektor(0, 0, 0));
            punktyMaterialne[1] = new PunktMaterialny(100000000000000000000f, new Wektor(50, 50, 0), new Wektor(0, 0, 0));
            punktyMaterialne[2] = new PunktMaterialny(1000000000000000000f, new Wektor(150, 150, 0), new Wektor(0, 0, 0));
            */

            for (int i = 0; i < liczbaObiektów; i++)
            {
               double masa = random.Next(1000,10000);
               double x =  random.NextDouble() * (pictureBox.Width - 10);
               double y = random.NextDouble() * (pictureBox.Height - 10);
               double vx = 100 * (0.5 - random.NextDouble());
               double vy = -100 * (0.5 - random.NextDouble());

               punktyMaterialne[i] = new PunktMaterialny(masa, new Wektor(x, y, 0), new Wektor(vx, vy, 0));
            }
        }

        public void Start()
        {
            czySymulacjaTrwa = true;

            Task.Run(() =>
            {
                while (czySymulacjaTrwa)
                {
                    for (int i = 0; i < liczbaObiektów; i++)
                    {
                        PunktMaterialny punkt1 = punktyMaterialne[i];
                        punkt1.f = new Wektor();
                        for (int j = 0; j < liczbaObiektów; j++)
                        {
                            if (i != j)
                            {
                                PunktMaterialny punkt2 = punktyMaterialne[j];
                                double masa = punkt1.m * punkt2.m;
                                Wektor r21 = punkt1.r - punkt2.r;
                                double d = r21.dlugosc();
                                r21.normuj();

                                punkt1.f = punkt1.f - r21 * G * (masa / (d * d));
                            }
                        }
                    }

                    for (int i = 0; i < liczbaObiektów; i++)
                    {
                        punktyMaterialne[i].AktualizujPozycje(deltaT);
                    }

                    t += deltaT;
                    pictureBox.Invalidate();
                    // System.Threading.Thread.Sleep((int)(1));
                }
            });
        }

        public void Stop()
        {
            czySymulacjaTrwa = false;
        }
    }
}
