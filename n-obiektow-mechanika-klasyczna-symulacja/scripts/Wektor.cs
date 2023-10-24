using System;

namespace n_obiektow_mechanika_klasyczna_symulacja.scripts
{
    class Wektor
    {
        public double x, y, z;

        public Wektor(double x = 0, double y = 0, double z = 0)
        {
            this.x = x; this.y = y; this.z = z;
        }

        #region Metody

        public double dlugosc()
        {
            return Math.Sqrt(this*this);
        }

        public Wektor normuj()
        {
            double dl = dlugosc();
            if (dl>0)
            {
                Wektor w = this / dl;

                this.x = w.x;
                this.y = w.y;
                this.z = w.z;

                return w;
            }
            
            return new Wektor();
        }

        public static double odlegloscDwochPunktow(Wektor a, Wektor b)
        {
            return (a - b).dlugosc();
        }

        #endregion

        #region Operatory

        public static Wektor operator +(Wektor a, Wektor b)
        {
            return new Wektor(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Wektor operator +(Wektor a, double b)
        {
            return new Wektor(a.x + b, a.y + b, a.z + b);
        }

        public static Wektor operator +(double b, Wektor a)
        {
            return new Wektor(a.x + b, a.y + b, a.z + b);
        }

        public static Wektor operator -(Wektor a, Wektor b)
        {
            return new Wektor(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Wektor operator -(Wektor a, double b)
        {
            return new Wektor(a.x - b, a.y - b, a.z - b);
        }

        public static Wektor operator -(double b, Wektor a)
        {
            return new Wektor(a.x - b, a.y - b, a.z - b);
        }

        public static Wektor operator -(Wektor a)
        {
            return new Wektor(-a.x, -a.y, -a.z);
        }

        public static double operator *(Wektor a, Wektor b) // iloczyn skalarny
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Wektor operator *(Wektor a, double b)
        {
            return new Wektor(a.x * b, a.y * b, a.z * b);
        }

        public static Wektor operator *(double b, Wektor a)
        {
            return new Wektor(a.x * b, a.y * b, a.z * b);
        }

        public static Wektor operator /(Wektor a, double b)
        {
            return new Wektor(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(Wektor a, Wektor b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Wektor a, Wektor b)
        {
            return !(a == b);
        }

        public static Wektor operator ^(Wektor a, Wektor b) // iloczyn wektorowy
        {
            return new Wektor(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }

        #endregion

    }
}