using System;
using System.Threading;
using System.Windows.Forms;

namespace n_obiektow_mechanika_klasyczna_symulacja.scripts
{
    internal class SilnikSymulacji
    {
        public PunktMaterialny[] punktyMaterialne;
        private PictureBox pictureBox;
        private bool czySymulacjaTrwa;
        public bool czyOdpychanieWlaczone;
        public bool czyKolizjaWlaczona;
        public bool czyOgraniczonaPrzestrzen;
        public double deltaT;
        public int minMasa;
        public int maxMasa;
        public int liczbaObiektów;
        public int liczbaWatkow;
        public int opoznienie;
        public double tlumienie;
        private const double G = 6.67408e-3;
        private const double K = 100;
        public double t;
        private Barrier bariera;
        private Barrier bariera2;
        private Thread[] watki;
        private ManualResetEvent manualResetEvent;
        public int minRadius;
        public int maxRadius;



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
                if (opoznienie > 0)
                {
                    Thread.Sleep(opoznienie);
                }
                this.t += deltaT;
            });
            this.manualResetEvent = new ManualResetEvent(false);


            punktyMaterialne = new PunktMaterialny[liczbaObiektów];
            Random random = new Random();

            for (int i = 0; i < liczbaObiektów; i++)
            {
                double masa = random.Next(minMasa, maxMasa);
                double x = random.NextDouble() * (pictureBox.Width - 10);
                double y = random.NextDouble() * (pictureBox.Height - 10);
                double vx = 0; // (0.5 - random.NextDouble());
                double vy = 0; // -(0.5 - random.NextDouble());

                double radius = ((masa - minMasa) / maxMasa) * (maxRadius - minRadius) + minRadius;

                punktyMaterialne[i] = new PunktMaterialny(masa, new Wektor(x, y, 0), new Wektor(vx, vy, 0), radius);
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
                            d = d < 1 ? 1 : d;
                            r21.normuj();

                            Wektor przyciaganie = (-G * (masa / (d * d))) * r21;
                            double dd = punkt1.radius + punkt2.radius;
                            Wektor odpychanie = czyOdpychanieWlaczone && d < dd ? (masa / K) * r21 : new Wektor();

                            punkt1.f = punkt1.f + przyciaganie + odpychanie;

                            if (czyKolizjaWlaczona && d < punkt1.radius + punkt2.radius + 2)
                            {
                                Wektor n = -r21;
                                Wektor vn = n * (n * punkt1.v);
                                Wektor vs = punkt1.v - vn;
                                punkt1.v = vs - vn * tlumienie;
                            }

                            if (czyOgraniczonaPrzestrzen)
                            {
                                if (punkt1.r.x < 0 || punkt1.r.x > pictureBox.Width)
                                {
                                    punkt1.v.x = -punkt1.v.x;
                                }
                                if (punkt1.r.y < 0 || punkt1.r.y > pictureBox.Height)
                                {
                                    punkt1.v.y = -punkt1.v.y;
                                }
                            }
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
