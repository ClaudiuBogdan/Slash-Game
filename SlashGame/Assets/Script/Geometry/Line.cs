using UnityEngine;
using UnityEngine.Experimental.XR;

namespace Assets.Script.Geometry
{
    public class Line
    {
        private Point pointA;
        private Point pointB;
        private float Slope;
        private float abscissaCutPoint;

        public Line(Point pointA, Point pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.Slope = (pointB.y - pointA.y) / (pointB.x - pointA.x);
            this.abscissaCutPoint = ( - pointA.x) * Slope + pointA.y;
           
        }

        public float GetSlope()
        {
            return this.Slope;
        }

        public float GetAbscissaCutPoint()
        {
            return abscissaCutPoint;
        }

        public Point Intersect(Line lineB)
        {
            if (this.GetSlope() > float.MaxValue || float.IsInfinity(this.GetSlope()))
            {
                return new Point(this.pointA.x, lineB.GetYForX(this.pointA.x));
            }
            if (lineB.GetSlope() > float.MaxValue || float.IsInfinity(lineB.GetSlope()))
            {
                return new Point(lineB.pointA.x, this.GetYForX(lineB.pointA.x));
            }
            double[][] m = MatrixInverseProgram.MatrixCreate(2, 2);
            m[0][0] = -this.GetSlope(); m[0][1] = 1;
            m[1][0] = -lineB.GetSlope(); m[1][1] = 1;

            double[][] b = MatrixInverseProgram.MatrixCreate(2, 1);
            b[0][0] = this.GetAbscissaCutPoint(); 
            b[1][0] = lineB.GetAbscissaCutPoint();

            double[][] inversMatrix = MatrixInverseProgram.MatrixInverse(m);
            double[][] intersectionPoint = MatrixInverseProgram.MatrixProduct(inversMatrix, b);
            return new Point((float)intersectionPoint[0][0],(float)intersectionPoint[1][0]);
        }

        private float GetYForX(float xValue)
        {
            return this.Slope * xValue + this.abscissaCutPoint;
        }
    }
}
