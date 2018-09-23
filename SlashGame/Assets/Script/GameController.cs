using System.Collections;
using Assets.Script.Geometry;
using UnityEngine;

namespace Assets.Script
{
    public class GameController : MonoBehaviour
    {
        public GameObject MainLineRendererPrefab;
        public GameObject MainCamera;

        private GameObject lineRendererObject;

        private SlideFigure MainSlideFigure;
        private ArrayList CutSlideFigureList;
        private Point firstSegmentPoint;
        private Point secondSegmentPoint;

        // Use this for initialization
        void Start () {

            //Create a polygon
            ArrayList polygonVertices = new ArrayList();

            //Figure square
            //ArrayList poligonVertices = new ArrayList(new Point[]{new Point(-1, 1), new Point(1, 1), new Point(1, -1), new Point(-1, -1) });

            //Figure arrow
            /*float outerRadius = 2f;
            float innerRadius = outerRadius * 0.866025404f;
            Vector3[] corners = new Vector3[6];
            corners[0] = new Vector3(0, 0, 0);
            corners[1] = new Vector3(outerRadius * 0.5f, -innerRadius, 0);
            corners[2] = new Vector3(-outerRadius * 0.5f, -innerRadius, 0);
            corners[3] = new Vector3(-outerRadius, 0, 0);
            corners[4] = new Vector3(-outerRadius * 0.5f, innerRadius, 0);
            corners[5] = new Vector3(outerRadius * 0.5f, innerRadius, 0);*/

            //Figure start
            Vector3[] corners= new Vector3[10];
            corners[0] = new Vector3(0, 8.16f, 0);//J
            corners[1] = new Vector3(2, 2, 0);//C
            corners[2] = new Vector3(8.47f, 2, 0);//K
            corners[3] = new Vector3(3.24f, -1.8f, 0);//I
            corners[4] = new Vector3(5.31f, -8.16f, 0);//L
            corners[5] = new Vector3(0, -4.16f, 0);//H
            corners[6] = new Vector3(-5.31f, -8.16f, 0);//M
            corners[7] = new Vector3(-3.24f, -1.8f, 0);//G
            corners[8] = new Vector3(-8.47f, 2, 0);//N
            corners[9] = new Vector3(-2, 2, 0);//B

            //Fill polygon vertices with generated vertices.
            foreach (Vector3 corner in corners)
            {
                polygonVertices.Add(new Point(corner.x, corner.y));
            }
            Polygon polygon = new Polygon(polygonVertices);

            MainSlideFigure = new SlideFigure(polygon);
            //CreateSlideFigureObject(polygon) to display the figure;
            lineRendererObject = CreateSlideFigureObject(polygon);

            CutSlideFigureList = new ArrayList();

        }
	
        // Update is called once per frame
        void Update () {
            if (IsFingerTouchFirstTime())
            {
                firstSegmentPoint = new Point(GetMousePositionToWorld().x, GetMousePositionToWorld().y);
            }
            if (IsScreenTouched())
            {
                DetectFigureCut();
            }

            if (IsFingerRelease())
            {
                CleanSlideFigure();
            }
            DeleteCutFigureElement();
        }

        /**
         * Method that eliminates the falling parts.
         */
        private void DeleteCutFigureElement()
        {
            float lowestDistance = -20f;
            if (CutSlideFigureList.Count > 0)
            {
                for (int i = CutSlideFigureList.Count - 1; i >= 0; i--)
                {
                    GameObject cutFigureObject = CutSlideFigureList[i] as GameObject;
                    if (cutFigureObject.transform.position.y < lowestDistance)
                    {
                        GameObject.Destroy(cutFigureObject);
                        CutSlideFigureList.Remove(cutFigureObject);
                    }
                }
            }
        }


        private void CleanSlideFigure()
        {
            MainSlideFigure.ResetCutPoints();
        }

        private void DetectFigureCut()
        {
            Point detectedPoint = new Point(GetMousePositionToWorld().x, GetMousePositionToWorld().y);
            if(detectedPoint.Equals(firstSegmentPoint)) //If the pointer hasn't moved, exit the function.
                return;

            secondSegmentPoint = firstSegmentPoint;
            firstSegmentPoint = detectedPoint;
        
            Segment segment = new Segment(firstSegmentPoint, secondSegmentPoint);
            MainSlideFigure.CheckIntersection(segment);

            if (MainSlideFigure.isReadyToCut())
            {
                MainSlideFigure.CutFigure();
                GameObject.Destroy(lineRendererObject); //Destroy the old gameObject figure
                lineRendererObject = CreateSlideFigureObject(MainSlideFigure.BigPolygon); //Create a new gameObject figure with the new polygon.
                GameObject secondFig = CreateSlideFigureObject(MainSlideFigure.SmallPolygon); 
                CutSlideFigureList.Add(secondFig);
                ConfigFallingFigure(secondFig);
            }
        }

        private bool IsFingerTouchFirstTime()
        {
            return Input.GetMouseButtonDown(0);
        }

        private bool IsFingerRelease()
        {
            return Input.GetMouseButtonUp(0);
        }

        private bool IsScreenTouched()
        {
            return Input.GetMouseButton(0);
        }


        /**
         * Method that creates a GameObject with the mesh defined by a polygon.
         */
        private GameObject CreateSlideFigureObject(Polygon polygon)
        {
            //Create the polygon GameObject
            GameObject LineRendererObject = Instantiate(MainLineRendererPrefab, Vector3.zero, Quaternion.identity);
            LineRendererObject.AddComponent<MeshFilter>();
            LineRendererObject.GetComponent<MeshFilter>().mesh = PolygonMesh.GetPolygonMesh(polygon);

            return LineRendererObject;
        }

        private void ConfigFallingFigure(GameObject secondFig)
        {
            secondFig.GetComponent<Rigidbody>().useGravity = true;
            secondFig.GetComponent<MeshCollider>().sharedMesh = secondFig.GetComponent<MeshFilter>().mesh;
            secondFig.GetComponent<Rigidbody>().ResetCenterOfMass();
            secondFig.GetComponent<Rigidbody>().AddForceAtPosition(MainSlideFigure.GetForceDirection(), MainSlideFigure.GetForceApplicationPoint(), ForceMode.Impulse);
        }

        private Vector3 GetMousePositionToWorld()
        {
            Vector3 point = new Vector3();

            Vector2 mousePos = new Vector2();

            // Get the mouse position from Event.
            // Note that the y position from Event is inverted.
            mousePos = Input.mousePosition;

            point = MainCamera.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10));
            return point;
        }
    }
}
