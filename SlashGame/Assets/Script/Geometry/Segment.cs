using System;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngineInternal;

namespace Assets.Script.Geometry
{
    public class Segment
    {
        private bool isCut;
        private Point pointA { get; }
        private Point pointB { get; }
        private Line line { get; }

        public Segment(Point pointA, Point pointB)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.line = new Line(pointA,pointB);
        }

        /**
         * Method that return the intersection point if the to segments intersect.
         */
        public Point Intersect(Segment segmentB)
        {
            //Check if the two segments are vertical
            if ((float.IsInfinity(this.line.GetSlope()) || this.line.GetSlope() > float.MaxValue) && (float.IsInfinity(this.line.GetSlope()) || segmentB.line.GetSlope() > float.MaxValue))
                return null;
            if(Mathf.Abs(this.line.GetSlope() - segmentB.line.GetSlope()) < Point.epsiloError)
                return null;
            Point linePointIntersection = this.line.Intersect(segmentB.line);
            if (this.ContainsPoint(linePointIntersection))
            {
                return linePointIntersection;
            }
            else
            {
                return null;
            }
        }

        public Boolean ContainsPoint(Point pointX)
        {
            //Are in the same line
            Line auxLine = new Line(pointA, pointX);
            if (Mathf.Abs(auxLine.GetSlope() - this.line.GetSlope()) > Point.epsiloError &&
                Mathf.Abs(auxLine.GetAbscissaCutPoint() - this.line.GetAbscissaCutPoint()) > Point.epsiloError)
                return false;

            //The module sum
            float mod1 = new Vector2(pointX.x - pointA.x, pointX.y - pointA.y).magnitude;
            float mod2 = new Vector2(pointX.x - pointB.x, pointX.y - pointB.y).magnitude;
            float mod3 = new Vector2(pointB.x - pointA.x, pointB.y - pointA.y).magnitude;
            if (Mathf.Abs(mod1 + mod2 - mod3) > Point.epsiloError)
                return false;
            return true;
        }

        public void SetIsSegmentCut(bool isCut)
        {
            this.isCut = isCut;
        }

        public Boolean isSegmentCut()
        {
            return this.isCut;
        }
    }
}
