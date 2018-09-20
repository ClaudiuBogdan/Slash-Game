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
    private Point firstSegmentPoint;
    private Point secondSegmentPoint;

	// Use this for initialization
	void Start () {
        //MainCamera = GameObject.Find("MainCamera");

		//Create a poligon
        ArrayList poligonVertices = new ArrayList(new Point[]{new Point(-1, 1), new Point(1, 1), new Point(1, -1), new Point(-1, -1) });
	    //poligonVertices.Reverse();
        
        Poligon poligon = new Poligon(poligonVertices);
	    Debug.Log(poligon);
        MainSlideFigure = new SlideFigure(poligon);
	    lineRendererObject = CreateSlideFigureObject(poligon);

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
        /*Debug.Log("First point: " + firstSegmentPoint);
        Debug.Log("Second point: " + secondSegmentPoint);*/
        //Debug.Log("Poligon mesh: " + MainSlideFigure.GetPoligon());
        
        Segment segment = new Segment(firstSegmentPoint, secondSegmentPoint);
        MainSlideFigure.CheckIntersection(segment);
        if (MainSlideFigure.isReadyToCut())
        {
            MainSlideFigure.CutFigure();
            Debug.Log("Figure cut");
            /*MainSlideFigure.newPoligonA;
            MainSlideFigure.newPoligonB;*/
            GameObject.Destroy(lineRendererObject);
            lineRendererObject = CreateSlideFigureObject(MainSlideFigure.newPoligonA);
            CreateSlideFigureObject(MainSlideFigure.newPoligonB).GetComponent<Rigidbody>().useGravity = true;
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


    private GameObject CreateSlideFigureObject(Poligon poligon)
    {
        //Render the poligon outline
        GameObject LineRendererObject = Instantiate(MainLineRendererPrefab, Vector3.zero, Quaternion.identity);
        /*LineRenderer lineRenderer = MainLineRendererPrefab.GetComponent<LineRenderer>();
        poligonMesh.SetPoligonOutline(lineRenderer);
        lineRenderer = poligonMesh.GetPoligonOutline();*/
        LineRendererObject.GetComponent<MeshFilter>().mesh = PoligonMesh.GetPoligonMesh(poligon);
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
