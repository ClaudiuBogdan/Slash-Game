using System.Collections;

namespace Assets.Script.Geometry
{
    public class Poligon
    {

        private ArrayList poligonVertices;
        private ArrayList poligonSides;

        public Poligon(ArrayList poligonVertices)
        {
            this.poligonVertices = poligonVertices;
            SetPoligonSides(poligonVertices);
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
                }
            }

            return intersectionPointsList;
        }
    }
}
