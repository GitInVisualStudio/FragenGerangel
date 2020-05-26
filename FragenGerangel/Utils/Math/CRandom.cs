using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Math
{
    /// <summary>
    /// Eine Custom-Random Klasse, die die normale Random-Klasse um PickElements() und eine Gaussche Zufallsfunktion erweitert
    /// </summary>
    public class CRandom : Random
    {
        public CRandom() : base()
        {

        }

        public CRandom(int seed) : base(seed)
        {

        }

        public float NextFloat()
        {
            return (float)NextDouble();
        }

        /// <summary>
        /// Gibt einen zufälligen Funktionswert der Gausschen Funktion e^-x^2 auf dem Intervall [-1; 1] zurück
        /// </summary>
        /// <param name="factor">Der Faktor der Gausschen Funktion. Beeinflusst, welche Ergebnisse möglich sind und wie wahrscheinlich sie sind. Für eine möglichst große Abdeckung des Wertebereichs [0; 1] standardmäßig mit 4 gewählt.</param>
        public float NextFloatGaussian(float factor = 4)
        {
            return (float)System.Math.Pow(System.Math.E, -factor * System.Math.Pow(2 * NextDouble() - 1, 2));
        }

        public T[] PickElements<T>(IEnumerable<T> array, int count)
        {
            array = array.OrderBy(x => NextDouble()); // mischt das Array zufällig durch
            return array.Take(count).ToArray();
        }

        public T[] PickElements<T>(IEnumerable<T> array, float percentage)
        {
            List<T> res = new List<T>();
            foreach (T obj in array)
                if (NextFloat() < percentage)
                    res.Add(obj);
            return res.ToArray();
        }
    }
}
