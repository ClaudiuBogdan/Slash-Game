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
            Point linePointIntersection = this.line.Intersect(segmentB.line);
            
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
            float errorEpsillon = Point.epsiloError * 10;
            bool isContained = false;
            //Check if the points are in the same line
            Vector3 vect1 = new Vector3(pointX.x - pointA.x, pointX.y - pointA.y, 0);
            Vector3 vect2 = new Vector3(pointX.x - pointB.x, pointX.y - pointB.y, 0);
            //Debug.Log("Angle points of line: " + Math.Abs(Vector3.Angle(vect1, vect2)));
            if (Math.Abs(Vector3.Angle(vect1, vect2)) > 180 - errorEpsillon)
            {
                Point leftPoint = pointA.x > pointB.x ? pointB : pointA;
                Point rightPoint = leftPoint == pointA ? pointB : pointA;
                if (rightPoint.y > leftPoint.y)
                    {
                        isContained =
                            ((leftPoint.x - errorEpsillon <= pointX.x && rightPoint.x + errorEpsillon >= pointX.x) &&
                             (leftPoint.y - errorEpsillon <= pointX.y && rightPoint.y + errorEpsillon >= pointX.y))
                                ? true
                                : false;
                    }
                    else
                    {
                        isContained =
                            ((leftPoint.x - errorEpsillon <= pointX.x && rightPoint.x + errorEpsillon >= pointX.x) &&
                             (leftPoint.y + errorEpsillon >= pointX.y && rightPoint.y - errorEpsillon <= pointX.y))
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

        public float DistanceToPoint(Point pointP)
        {
            Vector3 vecV = new Vector3(this.pointB.x - this.pointA.x, this.pointB.y - this.pointA.y, 0);
            Vector3 vecP = new Vector3(pointP.x - this.pointA.x, pointP.y - this.pointA.y, 0);
            float dotProduct = Vector3.Dot(vecP, vecV);
            if (Vector3.Angle(vecP, vecV) <= 90 && dotProduct <= vecV.magnitude)
            {
                return Vector3.Cross(vecP, vecV).magnitude;
            }
            else
            {
                float distanceToPointA = pointA.DistanceToPoint(pointP);
                float distanceToPointB = pointB.DistanceToPoint(pointP);
                return distanceToPointA < distanceToPointB ? distanceToPointA : distanceToPointB;
            }
        } 

        public Point GetMiddlePoint()
        {
            return new Point((pointA.x + pointB.x)/2.0f,(pointA.y + pointB.y)/ 2.0f);
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
