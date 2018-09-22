using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.Assertions.Comparers;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.XR;
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
            Point linePointIntersection = null;
            try
            {
                linePointIntersection = this.line.Intersect(segmentB.line);
            }
            catch (System.ArgumentException ex)
            {
                Debug.Log("Parallel lines" + ex.StackTrace);
            }
            if (!(this.ContainsPoint(linePointIntersection) && segmentB.ContainsPoint(linePointIntersection)))
            {
                linePointIntersection = null;
            }
            return linePointIntersection;
        }

        public Boolean ContainsPoint(Point pointX)
        {
            if (pointX == null)
                return false;

            bool isContained = false;
            //Check if the points are in the same line
            Vector3 vect1 = new Vector3(pointX.x - pointA.x, pointX.y - pointA.y, 0);
            Vector3 vect2 = new Vector3(pointX.x - pointB.x, pointX.y - pointB.y, 0);
            //Debug.Log("Angle points of line: " + Math.Abs(Vector3.Angle(vect1, vect2)));
            if (Math.Abs(Vector3.Angle(vect1, vect2)) > 180 - Point.epsiloError)
            {
                Point leftPoint = pointA.x > pointB.x ? pointB : pointA;
                Point rightPoint = leftPoint == pointA ? pointB : pointA;
                if (rightPoint.y > leftPoint.y)
                    {
                        isContained =
                            ((leftPoint.x < pointX.x && rightPoint.x > pointX.x) &&
                             (leftPoint.y < pointX.y && rightPoint.y > pointX.y))
                                ? true
                                : false;
                    }
                    else
                    {
                        isContained =
                            ((leftPoint.x < pointX.x && rightPoint.x > pointX.x) &&
                             (leftPoint.y > pointX.y && rightPoint.y < pointX.y))
                                ? true
                                : false;
                    }
            }

            
            return isContained;
        }

        public void SetIsSegmentCut(bool isCutSetter)
        {
            this.isCut = isCutSetter;
        }

        public Boolean IsSegmentCut()
        {
            return this.isCut;
        }

        public override string ToString()
        {
            return $"Segment {pointA} - {pointB}";
        }

        public Point GetPointA()
        {
            return this.pointA;
        }

        public Point GetPointB()
        {
            return this.pointB;
        }
    }
}
