using System;
using UnityEngine;
using UnityEngine.Experimental.XR;

namespace Assets.Script.Geometry
{
    public class Line
    {
        private readonly Point pointA;
        private readonly Point pointB;
        private readonly float Slope;
        private readonly float abscissaCutPoint;
        //Fields for line equation: Ax + By = C
        private readonly float _coefA;
        private readonly float _coefB;
        private readonly float _coefC;

        public Line(Point pointA, Point pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.Slope = Mathf.Abs(pointB.x - pointA.x) < Point.epsiloError ? float.PositiveInfinity : (pointB.y - pointA.y) / (pointB.x - pointA.x);
            this.abscissaCutPoint = ( - pointA.x) * Slope + pointA.y;
            this._coefA = -Slope;
            this._coefB = float.IsInfinity(Slope) ? 0 : 1;
            this._coefC = abscissaCutPoint;
        }

        /**
         * Method that returns the inclination of the line.
         */
        public float GetSlope()
        {
            return this.Slope;
        }

        public float GetAbscissaCutPoint()
        {
            return abscissaCutPoint;
        }

        /**
         * Method that returns the point of the intersection between this line and another line.
         * return The point of the intersection.
         */
        public Point Intersect(Line lineB)
        {
            float delta = this._coefA * lineB._coefB - lineB._coefA * this._coefB;

            if (Mathf.Abs(delta) < Point.epsiloError)
                throw new ArgumentException("Lines are parallel");

            float x = (lineB._coefB * this._coefC - this._coefB * lineB._coefC) / delta;
            float y = (this._coefA * lineB._coefC - lineB._coefA * this._coefC) / delta;
            return new Point(x, y);
        }

        private float GetYForX(float xValue)
        {
            return this.Slope * xValue + this.abscissaCutPoint;
        }
    }
}
