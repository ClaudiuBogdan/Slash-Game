using System;
using System.Collections;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class Poligon
    {

        private ArrayList poligonVertices;
        private ArrayList poligonSides;
        private ArrayList poligonTriangles;

        public Poligon(ArrayList poligonVertices)
        {
            this.poligonVertices = poligonVertices;
            SetPoligonSides(poligonVertices);
            SetPoligonTriangles(poligonVertices);
        }

        //Vertices must be ordered and size grater than 2
        private void SetPoligonTriangles(ArrayList verticesList)
        {
            Point lateralPointA = verticesList[0] as Point;
            Point centerPoint = verticesList[2] as Point;
            Point lateralPointB = verticesList[1] as Point;
            float angleCenterPoint = GetSegmentsAngle(lateralPointA, centerPoint, lateralPointB);
        }

        private float GetSegmentsAngle(Point lateralPointA, Point centerPoint, Point lateralPointB)
        {
            Vector2 auxSegmentA = new Vector2(lateralPointA.x - centerPoint.x, lateralPointA.y - centerPoint.y);
            Vector2 auxSegmentB = new Vector2(lateralPointB.x - centerPoint.x, lateralPointB.y - centerPoint.y);
            return 0;

        }

        private void SetPoligonSides(ArrayList poligonVertices)
        {
            this.poligonSides = new ArrayList();
            for (int i = 1; i <= poligonVertices.Count; i++)
            {
                Point pointA;
                Point pointB;
                if (i == 1)
                {
                    //First segment
                    pointA = poligonVertices[0] as Point;
                    pointB = poligonVertices[1] as Point;
                }
                else if( i == poligonVertices.Count)
                {
                    //Last segment
                    pointA = poligonVertices[0] as Point;
                    pointB = poligonVertices[poligonVertices.Count - 1] as Point;
                }
                else
                {
                    //Intermediary segment
                    pointA = poligonVertices[i - 1] as Point;
                    pointB = poligonVertices[i] as Point;
                }
                Segment sideSegment = new Segment(pointA, pointB);
                this.poligonSides.Add(sideSegment);
            }

        }

        public ArrayList GetSegmentIntersectionPoints(Segment segmentA)
        {
            ArrayList intersectionPointsList = new ArrayList();
            foreach (Segment segmentSide in poligonSides)
            {
                Point intersectionPoint = segmentSide.Intersect(segmentA);
                if (intersectionPoint != null)
                {
                    intersectionPointsList.Add(intersectionPoint);
                    segmentSide.SetIsSegmentCut(true);
                }
            }

            return intersectionPointsList;
        }

        public ArrayList GetPoligonVertices()
        {
            return this.poligonVertices.Clone() as ArrayList;
        }

        public Vector3[] GetPoligonVerticesAsVectors()
        {
            ArrayList verticesList = new ArrayList();
            foreach (Point poligonVertex in this.GetPoligonVertices())
            {
                Vector3 verticeVector = new Vector3(poligonVertex.x, poligonVertex.y, 0);
                verticesList.Add(verticeVector);
            }

            return verticesList.ToArray(typeof(Vector3)) as Vector3[];
        }

        public void ResetCutSegments()
        {
            foreach (Segment poligonSide in poligonSides)
            {
                poligonSide.SetIsSegmentCut(false);
            }
        }

        public ArrayList GetPoligonSides()
        {
            return this.poligonSides;
        }
    }
}
