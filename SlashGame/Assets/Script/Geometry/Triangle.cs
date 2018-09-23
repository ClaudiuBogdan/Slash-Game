namespace Assets.Script.Geometry
{
    public class Triangle
    {

        public Point pointA { get; private set; }
        public Point pointB { get; private set; }
        public Point pointC { get; private set; }

        public Triangle(Point pointA, Point pointB, Point pointC)
        {
            this.pointA = pointA;
            this.pointB = pointB;
            this.pointC = pointC;
        }
    }
}
