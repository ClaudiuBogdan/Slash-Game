using System.Collections;
using System.Collections.Generic;
using Assets.Script.Geometry;
using UnityEngine;

public class WorldRenderer : MonoBehaviour
{
    public GameObject MainLineRendererPrefab;
    public GameObject MainCamera;

    private SlideFigure MainSlideFigure;
    private Point fistSegmentPoint;
    private Point secondSegmentPoint;

	// Use this for initialization
	void Start () {
        //MainCamera = GameObject.Find("MainCamera");

		//Create a poligon
        ArrayList poligonVertices = new ArrayList(new Point[]{new Point(-1, 1), new Point(1, 1), new Point(1, -1), new Point(-1, -1), new Point(-1, 1) });
        Poligon poligon = new Poligon(poligonVertices);
        MainSlideFigure = new SlideFigure(poligon);
	    MainLineRendererPrefab = CreateSlideFigureObject(poligon);

	}
	
	// Update is called once per frame
	void Update () {
	    if (isFingerTouchFirstTime())
	    {
	        Debug.Log("Touched first time");
            fistSegmentPoint = new Point(GetMousePositionToWorld().x, GetMousePositionToWorld().y);

	    }
        if (isScreenTouched())
	    {
            //Debug.Log(GetMousePositionToWorld());
	        Debug.Log("Touching...");
            DetectFigureCut();
	    }

	    if (isFingerRelease())
	    {
	        Debug.Log("Release touch");
            fistSegmentPoint = null;


	    }
	}

    private void DetectFigureCut()
    {
        secondSegmentPoint = fistSegmentPoint;
        fistSegmentPoint = new Point(GetMousePositionToWorld().x, GetMousePositionToWorld().y);
        Segment segment = new Segment(fistSegmentPoint, secondSegmentPoint);
        MainSlideFigure.CheckIntersection(segment);
        if (MainSlideFigure.isReadyToCut())
        {
            MainSlideFigure.CutFigure();
            Debug.Log("Figure cut");
            /*MainSlideFigure.newPoligonA;
            MainSlideFigure.newPoligonB;*/
        }
    }

    private bool isFingerTouchFirstTime()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool isFingerRelease()
    {
        return Input.GetMouseButtonUp(0);
        OnFingerRelease();
    }

    private void OnFingerRelease()
    {
        MainSlideFigure.resetCutPoints();
    }

    private bool isScreenTouched()
    {
        return Input.GetMouseButton(0);
    }


    private GameObject CreateSlideFigureObject(Poligon poligon)
    {
        PoligonMesh poligonMesh = new PoligonMesh(poligon);

        //Render the poligon outline
        GameObject LineRendererObject = Instantiate(MainLineRendererPrefab, Vector3.zero, Quaternion.identity);
        LineRenderer lineRenderer = MainLineRendererPrefab.GetComponent<LineRenderer>();
        poligonMesh.SetPoligonOutline(lineRenderer);
        lineRenderer = poligonMesh.GetPoligonOutline();
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
