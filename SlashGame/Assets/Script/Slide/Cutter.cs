using Assets.Script.Geometry;

namespace Assets.Script.Slide
{
    public class Cutter
    {

        public Point FirstCutPoint;
        public int IndexSegmentFirstCut;
        public Point SecondCutPoint;
        public int IndexSegmentSecondCut;

        public Cutter()
        {
            IndexSegmentFirstCut = -1;
            IndexSegmentSecondCut = -1;
        }

        public bool IsCutterReady()
        {
            return FirstCutPoint != null && SecondCutPoint != null;
        }

        public void SetCutPoint(Point cutPoint, int indexSegmentCut)
        {
            if (FirstCutPoint == null)
            {
                FirstCutPoint = cutPoint;
                IndexSegmentFirstCut = indexSegmentCut;
            }
            else
            {
                SecondCutPoint = cutPoint;
                IndexSegmentSecondCut = indexSegmentCut;
            }
        }

        public Point GetFirstIndexCutPoint()
        {
            return IndexSegmentFirstCut < IndexSegmentSecondCut ? FirstCutPoint : SecondCutPoint;
        }


        public Point GetSecondIndexCutPoint()
        {
            return IndexSegmentFirstCut > IndexSegmentSecondCut ? FirstCutPoint : SecondCutPoint;
        }

        public bool IsEmpty()
        {
            return FirstCutPoint == null;
        }

        public void Clear()
        {
            this.FirstCutPoint = null;
            this.SecondCutPoint = null;
            this.IndexSegmentFirstCut = -1;
            this.IndexSegmentSecondCut = -1;
        }
    }
}
