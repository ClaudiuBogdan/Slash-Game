using System;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class Point
    {
        public static float epsiloError = 0.1f;
        public float x;
        public float y;

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"( {x} , {y} )";
        }

        protected bool Equals(Point other)
        {
            return x.Equals(other.x) && y.Equals(other.y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (x.GetHashCode() * 397) ^ y.GetHashCode();
            }
        }
    }
}
