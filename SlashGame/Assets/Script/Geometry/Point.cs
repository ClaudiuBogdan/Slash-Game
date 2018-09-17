using System;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class Point
    {
        public static float epsiloError = Mathf.Pow(1, -15);
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
    }
}
