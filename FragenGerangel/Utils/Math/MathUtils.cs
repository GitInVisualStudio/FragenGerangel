using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using s = System;

namespace FragenGerangel.Utils.Math
{
    public class MathUtils
    {

        public const float PI = (float)s::Math.PI;

        public static float Sin(float angle) => (float)s::Math.Sin(ToRadians(angle));
        

        public static float Cos(float angle) => (float)s::Math.Cos(ToRadians(angle));
        

        public static float Tan(float angle) => (float)s::Math.Tan(ToRadians(angle));
        

        public static float Asin(float sin) => ToDegree((float)s::Math.Asin(sin));
        

        public static float Acos(float cos) => ToDegree((float)s::Math.Acos(cos));
        

        public static float Atan(float tan) => ToDegree((float)s::Math.Atan(tan));
        

        public static float ToRadians(float angle) => (float)(angle * s::Math.PI / 180.0f);
        

        public static float ToDegree(float angle) => (float)(angle * 180.0f / s::Math.PI);

        /// <summary>
        /// Rotiert die Position um den Winkel
        /// </summary>
        /// <param name="position"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector GetRotation(Vector position, float angle)
        {
            return new Vector(position.X * Cos(angle) - position.Y * Sin(angle), position.X * Sin(angle) + position.Y * Cos(angle));
        }

        /// <summary>
        /// Interpoliert zwei Werte auf zwischenwerte
        /// </summary>
        /// <param name="prev"></param>
        /// <param name="current"></param>
        /// <param name="partialTicks">Zeit bis zum nächsten Tick(0-1)</param>
        /// <returns></returns>
        public static dynamic Interpolate(dynamic prev, dynamic current, float partialTicks)
        {
            return current + (prev - current) * partialTicks;
        }

        public static float Sqrt(float d)
        {
            return (float)s::Math.Sqrt(d);
        }

        public static float Pow(float basis, float exponent)
        {
            return (float)s::Math.Pow(basis, exponent);
        }

        public static float Average(IEnumerable<float> list)
        {
            return list.Sum() / list.Count();
        }

        public static bool IsInPolygon(PointF[] poly, PointF point)
        {
            var coef = poly.Skip(1).Select((p, i) =>
                                            (point.Y - poly[i].Y) * (p.X - poly[i].X)
                                          - (point.X - poly[i].X) * (p.Y - poly[i].Y))
                                    .ToList();

            if (coef.Any(p => p == 0))
                return true;

            for (int i = 1; i < coef.Count(); i++)
            {
                if (coef[i] * coef[i - 1] < 0)
                    return false;
            }
            return true;
        }
    }
}
