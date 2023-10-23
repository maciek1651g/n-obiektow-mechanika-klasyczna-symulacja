using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace n_obiektow_mechanika_klasyczna_symulacja.scripts
{
    internal class PunktMaterialny
    {
        public double m;
        public double radius;

        public Wektor f;
        public Wektor v;
        public Wektor r;

        public Wektor dr;
        public Wektor dv;

        public PunktMaterialny(double masa, Wektor polozenie, Wektor predkosc){
            m = masa;
            r = polozenie;
            v = predkosc;
            f = new Wektor();
            radius = masa/1000;
        }

        public void AktualizujPozycje(double dt)
        {
            dv = (f / m)*dt;
            v = v + dv;
            dr = v * dt;
            r = r + dr;
        }
    }
}
