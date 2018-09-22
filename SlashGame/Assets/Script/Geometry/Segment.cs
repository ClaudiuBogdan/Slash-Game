using System;
using System.Collections;
using System.Runtime.Remoting.Messaging;
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
            if ((float.IsInfinity(this.line.GetSlope()) || this.line.GetSlope() > float.MaxValue) && (float.IsInfinity(segmentB.line.GetSlope()) || segmentB.line.GetSlope() > float.MaxValue))
                return null;
            //Check if the two segments are equal
            if (Mathf.Abs(this.line.GetSlope() - segmentB.line.GetSlope()) < Point.epsiloError)
                return null;
            Point linePointIntersection = this.line.Intersect(segmentB.line);
            if(float.IsNaN(linePointIntersection.x) || float.IsNaN(linePointIntersection.y))
            {
                return null;
            }
            if (this.ContainsPoint(linePointIntersection) && segmentB.ContainsPoint(linePointIntersection))
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
            bool isContained = false;
            //Check if the points are in the same line
            Vector3 vect1 = new Vector3(pointX.x - pointA.x, pointX.y - pointA.y, 0);
            Vector3 vect2 = new Vector3(pointX.x - pointB.x, pointX.y - pointB.y, 0);
            //Debug.Log("Angle points of line: " + Math.Abs(Vector3.Angle(vect1, vect2)));
            if (Math.Abs(Vector3.Angle(vect1, vect2)) > 180 - Point.epsiloError)
            {
                if (pointA.x > pointB.x)
                {

                    if (pointA.y > pointB.y)
                    {
                        isContained =
                            ((pointB.x <= pointX.x && pointA.x >= pointX.x) &&
                             (pointB.y <= pointX.y && pointA.y >= pointX.y))
                                ? true
                                : false;
                    }
                    else
                    {
                        isContained =
                            ((pointB.x <= pointX.x && pointA.x >= pointX.x) &&
                             (pointB.y >= pointX.y && pointA.y <= pointX.y))
                                ? true
                                : false;
                    }
                }
                else
                {
                    if (pointA.y > pointB.y)
                    {
                        isContained =
                            ((pointB.x >= pointX.x && pointA.x <= pointX.x) &&
                             (pointB.y <= pointX.y && pointA.y >= pointX.y))
                                ? true
                                : false;
                    }
                    else
                    {
                        isContained =
                            ((pointB.x >= pointX.x && pointA.x <= pointX.x) &&
                             (pointB.y >= pointX.y && pointA.y <= pointX.y))
                                ? true
                                : false;
                    }
                }
            }

            
            return isContained;
        }

        public void SetIsSegmentCut(bool isCut)
        {
            this.isCut = isCut;
        }

        public Boolean isSegmentCut()
        {
            return this.isCut;
        }

        public override string ToString()
        {
            return "PointA: " + this.pointA + " PointB: " + pointB;
        }

        public Point getPointA()
        {
            return this.pointA;
        }

        public Point getPointB()
        {
            return this.pointB;
        }
    }
}
