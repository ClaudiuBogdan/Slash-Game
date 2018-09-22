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
            //Trow exeption if verices length is less than 3;
            if(poligonVertices.Count < 3)
                return;
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
                    pointA = poligonVertices[poligonVertices.Count - 1] as Point;
                    pointB = poligonVertices[0] as Point;
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
                if (intersectionPoint != null && !segmentSide.IsSegmentCut())
                {
                    intersectionPointsList.Add(intersectionPoint);
                    segmentSide.SetIsSegmentCut(true);
                }
            }

            return intersectionPointsList;
        }

        public ArrayList GetPoligonVertices()
        { 
            return this.poligonVertices as ArrayList;
        }

        public Vector3[] GetPoligonVerticesAsVectors()
        {
            ArrayList verticesList = new ArrayList();
            foreach (Point poligonVertex in this.GetPoligonVertices())
            {
                Vector3 verticeVector = new Vector3(poligonVertex.x, poligonVertex.y, 0);
                verticesList.Add(verticeVector);
            }
            Vector3 verticeVectorLast = new Vector3(((Point)poligonVertices[0]).x, ((Point)poligonVertices[0]).y, 0);
            verticesList.Add(verticeVectorLast);

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

        public float GetArea()
        {
            Point[] m_points = this.GetPoligonVerticesAsPointArray();
            int n = m_points.Length;
            float A = 0.0f;
            for (int p = n - 1, q = 0; q < n; p = q++)
            {
                Point pval = m_points[p];
                Point qval = m_points[q];
                A += pval.x * qval.y - qval.x * pval.y;
            }
            return (A * 0.5f);
        }

        private Point[] GetPoligonVerticesAsPointArray()
        {
            return this.GetPoligonVertices().ToArray(typeof(Point)) as Point[];
        }

        public override string ToString()
        {
            String str = "";
            int count = 0;
            /*foreach (Point vertex in poligonVertices)
            {
                str += "Vertex " + count + " : " + vertex.ToString();
                count++;
            }*/
            foreach (Segment side in poligonSides)
            {
                str += "Side " + count + " : " + side;
                count++;
            }
            return str;
        }
    }
}
