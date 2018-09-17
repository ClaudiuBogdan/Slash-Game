using Assets.Script.Geometry;
using UnityEngine;

namespace Assets.Script
{
    public class TestScript : MonoBehaviour {


        void Start () {
            //Test the segment intersection function:
            Point pointA = new Point(-1, 0);
            Point pointB = new Point(1, 0);
            Point pointC = new Point(0, -1);
            Point pointD = new Point(0, 1);

            Segment segmentA = new Segment(pointA, pointB);
            Segment segmentB = new Segment(pointC, pointD);

            Point intersectionPoint = segmentA.Intersect(segmentB); //Expexted Point(0, 0)

            Debug.Log("Expexted Point(0, 0), Point: " + intersectionPoint );
        }
	

    }
}
