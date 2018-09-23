using System.Collections;
using System.Collections.Generic;
using Assets.Script.Geometry;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
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
        //MainCamera = GameObject.Find("MainCamera");

        /*		//Create a polygon
                ArrayList poligonVertices = new ArrayList(new Point[]{new Point(-1, 1), new Point(1, 1), new Point(1, -1), new Point(-1, -1) });*/

	    ArrayList poligonVertices = new ArrayList();
        float outerRadius = 2f;
        float innerRadius = outerRadius * 0.866025404f;
        /*Vector3[] corners = new Vector3[6];

        corners[0] = new Vector3(0, 0, 0);
        corners[1] = new Vector3(outerRadius * 0.5f, -innerRadius, 0);
        corners[2] = new Vector3(-outerRadius * 0.5f, -innerRadius, 0);
        corners[3] = new Vector3(-outerRadius, 0, 0);
        corners[4] = new Vector3(-outerRadius * 0.5f, innerRadius, 0);
        corners[5] = new Vector3(outerRadius * 0.5f, innerRadius, 0);*/
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
        foreach (Vector3 corner in corners)
	    {
	        poligonVertices.Add(new Point(corner.x, corner.y));
	    }

        Polygon polygon = new Polygon(poligonVertices);
	    Debug.Log(polygon);
        MainSlideFigure = new SlideFigure(polygon);
	    lineRendererObject = Instantiate(MainLineRendererPrefab, Vector3.zero, Quaternion.identity);//CreateSlideFigureObject(polygon);
        GameObject LineRendererObject = Instantiate(MainLineRendererPrefab, Vector3.zero, Quaternion.identity);
	    LineRendererObject.AddComponent<MeshFilter>();
        /*LineRenderer lineRenderer = MainLineRendererPrefab.GetComponent<LineRenderer>();
        poligonMesh.SetPoligonOutline(lineRenderer);
        lineRenderer = poligonMesh.GetPoligonOutline();*/
        LineRendererObject.GetComponent<MeshFilter>().mesh = PolygonMesh.GetPolygonMesh(polygon);
	    lineRendererObject = LineRendererObject;

        CutSlideFigureList = new ArrayList();

	}
	
	// Update is called once per frame
	void Update () {
	    if (isFingerTouchFirstTime())
	    {
	        //Debug.Log("Touched first time");
	        firstSegmentPoint = new Point(GetMousePositionToWorld().x, GetMousePositionToWorld().y);

	    }
	    if (isScreenTouched())
	    {
	        //Debug.Log(GetMousePositionToWorld());
	        //Debug.Log("Touching...");
	        DetectFigureCut();
	    }

	    if (isFingerRelease())
	    {
	        // Debug.Log("Release touch");
	        //firstSegmentPoint = null;
	        CleanSlideFigure();

	    }

	    DeleteCutFigureElement();
	}

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
        MainSlideFigure.resetCutPoints();
    }

    private void DetectFigureCut()
    {
        Point detectedPoint = new Point(GetMousePositionToWorld().x, GetMousePositionToWorld().y);
        if(detectedPoint.Equals(firstSegmentPoint))
            return;
        secondSegmentPoint = firstSegmentPoint;
        firstSegmentPoint = detectedPoint;
        
        Segment segment = new Segment(firstSegmentPoint, secondSegmentPoint);
        MainSlideFigure.CheckIntersection(segment);

        if (MainSlideFigure.isReadyToCut())
        {
            MainSlideFigure.CutFigure();

            GameObject.Destroy(lineRendererObject);
            lineRendererObject = CreateSlideFigureObject(MainSlideFigure.BigPolygon);
            GameObject secondFig = CreateSlideFigureObject(MainSlideFigure.SmallPolygon);
            CutSlideFigureList.Add(secondFig);
            secondFig.GetComponent<Rigidbody>().useGravity = true;
            secondFig.GetComponent<MeshCollider>().sharedMesh = secondFig.GetComponent<MeshFilter>().mesh;
            secondFig.GetComponent<Rigidbody>().ResetCenterOfMass();
            secondFig.GetComponent<Rigidbody>().AddForceAtPosition(MainSlideFigure.GetForceDirection(), MainSlideFigure.GetForceApplicationPoint(),ForceMode.Impulse);

            MainSlideFigure.SetPolygon(MainSlideFigure.BigPolygon);
            CleanSlideFigure();
        }
    }

    private bool isFingerTouchFirstTime()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool isFingerRelease()
    {
        return Input.GetMouseButtonUp(0);
    }

    private bool isScreenTouched()
    {
        return Input.GetMouseButton(0);
    }


    private GameObject CreateSlideFigureObject(Polygon polygon)
    {
        //Render the polygon outline
        GameObject LineRendererObject = Instantiate(MainLineRendererPrefab, Vector3.zero, Quaternion.identity);
        LineRendererObject.AddComponent<MeshFilter>();
        LineRendererObject.GetComponent<MeshFilter>().mesh = PolygonMesh.GetPolygonMesh(polygon);

        return LineRendererObject;
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
