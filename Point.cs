using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geometryclass;

namespace laba12._2
{
    class Point<T> where T : Geometryfigure1
    {
        public T Data { get; set; }
        public Point<T> Next { get; set; }

        public Point()
        {
            Data = default(T);
            Next = null;
        }

        public Point(T info)
        {
            Data = info;
            Next = null;
        }

        public override string ToString()
        {
            if (Data != null)
                return Data.ToString();
            else
                return "null";
        }

        public override int GetHashCode()
        {
            return Data?.GetHashCode() ?? 0; // оператор объединения с нулевым значением
        }
    }
}
