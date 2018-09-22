using System;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class Point
    {
        public static float epsiloError = 0.00001f;
        public float x { get; set; }
        public float y { get; set; }

        public Point(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public override string ToString()
        {
            return $"Point:( {x} , {y} )";
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

        public float DistanceToPoint(Point aPoint)
        {
            return Mathf.Sqrt(Mathf.Pow(aPoint.x - this.x,2) + Mathf.Pow(aPoint.y - this.y, 2));
        }
    }
}
