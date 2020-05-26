using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FragenGerangel.Utils.Math
{
    public struct Vector
    {
        //Werte des Vectors in mehreren Dimensionen
        private float[] values;

        public float[] Values
        {
            get
            {
                return values;
            }

            set
            {
                values = value;
            }
        }

        //Indexer um auf die einzelnen Dimensionen einfach zugreifen zu können
        public float this[int i]
        {
            get
            {
                return values[i];
            }

            set
            {
                values[i] = value;
            }
        }

        //Getter/Setter für die erste Dimension
        public float X
        {
            get
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                return values[0];
            }

            set
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                values[0] = value;
            }
        }

        //Getter/Setter für die zweite Dimension
        public float Y
        {
            get
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                return values[1];
            }

            set
            {
                if (values.Length < 2)
                    throw new VectorNot2DException();

                values[1] = value;
            }
        }

        //Default Vector
        public static Vector Null
        {
            get
            {
                return new Vector();
            }
        }

        //Konstruktor für einen Mehrdimensionalen Vector mit direkter Werteübergabe der Dimensionen
        public Vector(params float[] p)
        {
            values = new float[p.Length];
            for (int i = 0; i < p.Length; i++)
            {
                values[i] = p[i];
            }
        }

        //Konstruktor für einen Mehrdimensionalen
        public Vector(int dimensions)
        {
            values = new float[dimensions];
        }

        //Gibt die Länge des Vectors zurück
        public float Length
        {
            get
            {
                float value = 0;
                foreach (float f in values)
                    value += f * f;
                return (float)System.Math.Sqrt(value);
            }
        }

        //Gibt die Dimensionen zurück
        public int Dimensions => values.Length;

        //Normalisiert den Vector, so dass nur noch die Richtung vorhanden ist
        public Vector Normalize()
        {
            Vector newVector = Copy();
            return newVector / Length;
        }

        //Überschreibt die Operatoren für eine einfachere Syntax
        public static Vector operator +(Vector v1, Vector v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] + v2[i];
            return newVector;
        }

        public static Vector operator -(Vector v1, Vector v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] - v2[i];
            return newVector;
        }

        public static Vector operator *(Vector v1, Vector v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] * v2[i];
            return newVector;
        }

        public static Vector operator *(Vector v1, float v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] * v2;
            return newVector;
        }

        public static Vector operator /(Vector v1, float v2)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = v1[i] / v2;
            return newVector;
        }

        public static Vector operator -(Vector v1)
        {
            Vector newVector = new Vector(v1.Dimensions);
            for (int i = 0; i < v1.Values.Length; i++)
                newVector[i] = -v1[i];
            return newVector;
        }

        public static bool operator <(Vector v1, Vector v2)
        {
            return v1.Length < v2.Length;
        }

        public static bool operator >(Vector v1, Vector v2)
        {
            return v1.Length > v2.Length;
        }

        public override string ToString()
        {
            return "X: " + X + " Y: " + Y;
        }

        public Vector Copy()
        {
            return new Vector(values);
        }

        public void ForEach(Func<float, float> function)
        {
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = function(values[i]);
            }
        }

        public static bool operator ==(Vector v1, Vector v2)
        {
            if (v1.Values.Length != v2.Values.Length)
                return false;
            for (int i = 0; i < v1.Values.Length; i++)
                if (v1[i] != v2[i])
                    return false;
            return true;
        }   

        public static bool operator !=(Vector v1, Vector v2)
        {
            return !(v1 == v2);
        }

        public PointF ToPoint()
        {
            if (Dimensions != 2)
                throw new Exception("Vector did not have 2 Dimensions.");
            return new PointF(X, Y);
        }

        class VectorNot2DException : Exception
        {
            public VectorNot2DException() : base("Vector did not have 2 or more Dimensions")
            {

            }
        }
    }
}