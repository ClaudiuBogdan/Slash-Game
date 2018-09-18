using System.Collections;
using Assets.Script.Geometry;
using UnityEngine;

namespace Assets.Script
{
    public class TestScript : MonoBehaviour {


        void Start()
        {
            TestSegmentIntersection();
            TestPoligonFigure();

        }

        private void TestPoligonFigure()
        {
            //Poligon points 
            Point A = new Point(-6.5f, 0);
            Point B = new Point(0, 6.5f);
            Point C = new Point(6.5f, 0);
            Point D = new Point(0, -6.5f);

            //Segment point
            Point E = new Point(-10, 3);
            Point F = new Point(10, 3);

            ArrayList poligonVertices = new ArrayList();
            poligonVertices.Add(A); poligonVertices.Add(B); poligonVertices.Add(C); poligonVertices.Add(D);
            Poligon poligonFigure = new Poligon(poligonVertices);

            Segment segmentA = new Segment(E, F);

            ArrayList intersectionPoints = poligonFigure.GetSegmentIntersectionPoints(segmentA);
            foreach (Point point in intersectionPoints)
            {
                Debug.Log("Intersection point: " + point);
            }

        }

        private void TestSegmentIntersection()
        {
            {
                /*//Test the segment intersection function:
                Point pointA = new Point(-1, 0);
                Point pointB = new Point(1, 0);
                Point pointC = new Point(0, -1);
                Point pointD = new Point(0, 1);

                Segment segmentA = new Segment(pointA, pointB);
                Segment segmentB = new Segment(pointC, pointD);

                Point intersectionPoint = segmentA.Intersect(segmentB); //Expexted Point(0, 0)

                Debug.Log("Expexted Point(0, 0), Point: " + intersectionPoint);*/
            }
            //A(-8.02f, 3.16f) B(0.38, 4.14)
            //C(-5.3, -6) D(0.38, 4.14)
            {
                Segment segmentA = new Segment(new Point(-8.02f, 3.16f), new Point(1.75f, -1.75f));
                Segment segmentB = new Segment(new Point(-5.3f, -6f), new Point(0.38f, 4.14f));
                Point intersectionPoint = segmentA.Intersect(segmentB); //Expexted Point(-1.89,0.08)
                Debug.Log("Expexted Point(-1.89,0.08), Point: " + intersectionPoint);
            }
        }
    }
}
