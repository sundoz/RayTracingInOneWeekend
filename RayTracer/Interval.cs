using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracer
{
    internal class Interval
    {
        public double Min { get; }
        public double Max { get; }

        public Interval()
        {
            Max = double.NegativeInfinity;
            Min = double.PositiveInfinity;
        }

        public Interval(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Size => Max - Min;
        public bool Contains(double value) => value >= Min && value <= Max;

        public bool Surrounds(double value) => value > Min && value < Max;

        public static Interval Empty() => new Interval(double.PositiveInfinity, double.NegativeInfinity);
        public static Interval Universe() => new Interval(double.NegativeInfinity, double.PositiveInfinity);

        public double Clamp (double x)
        {
            if (x < Min) return Min;
            if (x > Max) return Max;
            return x;
        }
    }
}
