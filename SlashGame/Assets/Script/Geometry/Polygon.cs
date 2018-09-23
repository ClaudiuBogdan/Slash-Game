using System;
using System.Collections;
using Assets.Script.Mesh;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class Polygon
    {

        private ArrayList _polygonVertices;
        private ArrayList _polygonSides;
        private ArrayList _polygonTriangles;
        public Vector3[] verticesMesh { get; private set; }
        public int[] trianglesIndexVerticesMesh { get; private set; }

        public Polygon(ArrayList polygonVertices)
        {
            //Trow exeption if verices length is less than 3;
            if(polygonVertices.Count < 3)
                return;
            this._polygonTriangles = new ArrayList();
            this._polygonVertices = polygonVertices;
            SetPoligonSides(polygonVertices);
            SetPolygonTriangles();
        }

        //Vertices must be ordered and size grater than 2
        private void SetPolygonTriangles()
        {
            // Use the triangulator to get indices for creating triangles
            Point[] vertices2D = this.GetPolygonVertices().ToArray(typeof(Point)) as Point[];
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            this.verticesMesh = vertices;
            this.trianglesIndexVerticesMesh = indices;
            
            this._polygonTriangles.Clear();
            for (int i = 0; i < indices.Length; i = i + 3)
            {
                Point pointA = new Point(vertices[indices[i]].x, vertices[indices[i]].y);
                Point pointB = new Point(vertices[indices[i + 1]].x, vertices[indices[i + 1]].y);
                Point pointC = new Point(vertices[indices[i + 2]].x, vertices[indices[i + 2]].y);
                _polygonTriangles.Add(new Triangle(pointA, pointB, pointC));
            }
        }

        private void SetPoligonSides(ArrayList poligonVertices)
        {
            this._polygonSides = new ArrayList();
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
                this._polygonSides.Add(sideSegment);
            }

        }

        public ArrayList GetSegmentIntersectionPoints(Segment segmentA)
        {
            ArrayList intersectionPointsList = new ArrayList();
            foreach (Segment segmentSide in _polygonSides)
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

        public ArrayList GetPolygonVertices()
        { 
            return this._polygonVertices as ArrayList;
        }

        public Vector3[] GetPoligonVerticesAsVectors()
        {
            ArrayList verticesList = new ArrayList();
            foreach (Point poligonVertex in this.GetPolygonVertices())
            {
                Vector3 verticeVector = new Vector3(poligonVertex.x, poligonVertex.y, 0);
                verticesList.Add(verticeVector);
            }
            Vector3 verticeVectorLast = new Vector3(((Point)_polygonVertices[0]).x, ((Point)_polygonVertices[0]).y, 0);
            verticesList.Add(verticeVectorLast);

            return verticesList.ToArray(typeof(Vector3)) as Vector3[];
        }

        public void ResetCutSegments()
        {
            foreach (Segment poligonSide in _polygonSides)
            {
                poligonSide.SetIsSegmentCut(false);
            }
        }

        public ArrayList GetPolygonSides()
        {
            return this._polygonSides;
        }

        public float GetArea()
        {
            Point[] m_points = this.GetPolygonVerticesAsPointArray();
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

        public bool IsPointInsidePolygon(Point point)
        {
            foreach (Triangle polygonTriangle in _polygonTriangles)
            {
                if (!Triangulator.PointInTriangle(polygonTriangle.pointA, polygonTriangle.pointB, polygonTriangle.pointC, point))
                {
                    return false;
                }
            }
            return true;
        }


        private Point[] GetPolygonVerticesAsPointArray()
        {
            return this.GetPolygonVertices().ToArray(typeof(Point)) as Point[];
        }

        public override string ToString()
        {
            String str = "";
            int count = 0;
            /*foreach (Point vertex in polygonVertices)
            {
                str += "Vertex " + count + " : " + vertex.ToString();
                count++;
            }*/
            foreach (Segment side in _polygonSides)
            {
                str += "Side " + count + " : " + side;
                count++;
            }
            return str;
        }
    }
}
