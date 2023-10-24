using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace n_obiektow_mechanika_klasyczna_symulacja.scripts
{
    internal class SilnikSymulacji
    {
        public PunktMaterialny[] punktyMaterialne;
        private PictureBox pictureBox;
        private bool czySymulacjaTrwa;
        public double deltaT;
        public int liczbaObiektów;
        public int liczbaWatkow;
        private const double G = 6.67408;
        public double t;
        private Barrier bariera;
        private Barrier bariera2;
        private Thread[] watki;
        private ManualResetEvent manualResetEvent;
        

        public SilnikSymulacji(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            czySymulacjaTrwa = false;
        }

        public void Reset()
        {
            czySymulacjaTrwa = false;
            this.t = 0;
            this.bariera = new Barrier(liczbaWatkow);
            this.bariera2 = new Barrier(liczbaWatkow, barrier =>
            {
                this.t += deltaT;
                pictureBox.Invalidate();
            });
            this.manualResetEvent = new ManualResetEvent(false);


            punktyMaterialne = new PunktMaterialny[liczbaObiektów];
            Random random = new Random();

            for (int i = 0; i < liczbaObiektów; i++)
            {
               double masa = random.Next(1000,10000);
               double x =  random.NextDouble() * (pictureBox.Width - 10);
               double y = random.NextDouble() * (pictureBox.Height - 10);
               double vx =  (0.5 - random.NextDouble());
               double vy = -(0.5 - random.NextDouble());

               punktyMaterialne[i] = new PunktMaterialny(masa, new Wektor(x, y, 0), new Wektor(vx, vy, 0));
            }
            pictureBox.Invalidate();

            przerwijWatki();
            this.watki = new Thread[liczbaWatkow];
            for (int i = 0; i < liczbaWatkow; i++)
            {
                int indexStart = i * (liczbaObiektów / liczbaWatkow);
                int indexEnd = (i + 1) * (liczbaObiektów / liczbaWatkow);
                if (i == liczbaWatkow - 1)
                {
                    indexEnd = liczbaObiektów;
                }
                watki[i] = new Thread(() => ThreadWork(indexStart, indexEnd));
                watki[i].Start();
            }
        }

        public void Start()
        {
            czySymulacjaTrwa = true;
            manualResetEvent.Set();
        }

        public void Stop()
        {
            manualResetEvent.Reset();
            czySymulacjaTrwa = false;
        }

        private void ThreadWork(int indexStart, int indexEnd)
        {
            while (true)
            {
                if (!czySymulacjaTrwa)
                {
                    manualResetEvent.WaitOne();
                }

                for (int i = indexStart; i < indexEnd; i++)
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

                bariera.SignalAndWait();

                for (int i = indexStart; i < indexEnd; i++)
                {
                    punktyMaterialne[i].AktualizujPozycje(deltaT);
                }

                bariera2.SignalAndWait();
            }
        }

        private void przerwijWatki()
        {
            if (watki != null)
            {
                foreach (var watek in watki)
                {
                    watek.Abort();
                }
            }
        }

        public void destroy()
        {
            przerwijWatki();
        }
    }
}
