using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Geometry;
using Assets.Script.Slide;
using UnityEngine;

public class SlideFigure
{

    private Polygon _figurePolygon;
    private Cutter cutter;

    public Polygon BigPolygon;
    public Polygon SmallPolygon;

    public SlideFigure(Polygon polygon)
    {
        SetPolygon(polygon);
        cutter = new Cutter();
    }

    public void resetCutPoints()
    {
        cutter.Clear();
        _figurePolygon.ResetCutSegments();
    }

    public Boolean isReadyToCut()
    {
        return cutter.IsCutterReady();
    }

    public void CheckIntersection(Segment segment)
    {
        ArrayList intersectionPoints = _figurePolygon.GetSegmentIntersectionPoints(segment);
        if (intersectionPoints.Count > 0)
{
            if (!cutter.IsEmpty())
            {
                Segment cuttingSegment = new Segment(cutter.FirstCutPoint, intersectionPoints[0] as Point);
                if (_figurePolygon.IsPointInsidePolygon(cuttingSegment.GetMiddlePoint()))
                {
                    cutter.SetCutPoint(intersectionPoints[0] as Point, 0);
                }
            }
            else
            {
                cutter.SetCutPoint(intersectionPoints[0] as Point, 0) ;
            }


        }
    }

    public void CutFigure()
    {
        ArrayList poligonSides = _figurePolygon.GetPolygonSides();
        ArrayList poligonVertices = _figurePolygon.GetPolygonVertices();
        Point firstCutPoint = null;
        ArrayList firstFigureVertices = new ArrayList();
        ArrayList secondFigureVertices = new ArrayList();
        ArrayList containerFigureVertices = firstFigureVertices;
        for (int i = 0; i < poligonSides.Count; i++)
        { 
            containerFigureVertices.Add(poligonVertices[i]);
            Segment segment = ((Segment) poligonSides[i]);
            if (segment.IsSegmentCut())
            {
                Point cutPoint = segment.ContainsPoint(cutter.FirstCutPoint)
                    ? cutter.FirstCutPoint
                    : cutter.SecondCutPoint;
                containerFigureVertices.Add(cutPoint);
                //Switch polygon array.
                containerFigureVertices = containerFigureVertices == firstFigureVertices
                    ? secondFigureVertices
                    : firstFigureVertices;
                containerFigureVertices.Add(cutPoint);
            }
        }
        Polygon polygonA  = new Polygon(ValidatePoligon(firstFigureVertices));
        Polygon polygonB = new Polygon(ValidatePoligon(secondFigureVertices));
        BigPolygon = polygonA.GetArea() < polygonB.GetArea() ? polygonA : polygonB;
        SmallPolygon = BigPolygon == polygonA ? polygonB : polygonA;

    }

    /**
     * Method that checks if the consecutive vertices are separated. 
     */
    private ArrayList ValidatePoligon(ArrayList polygonVertices)
    {
        float minDistanceBetweenVertices = 0.1f;
        bool firstTime = true;
        Point firstPoint = null;
        Point secondPoint= null;
        ArrayList validatedPolygonVertices = new ArrayList();
        int verticesCount = polygonVertices.Count;
        for (int i=0; i<verticesCount ; i++)
        {
            if (firstTime)
            {
                secondPoint = polygonVertices[i] as Point;
                validatedPolygonVertices.Add(secondPoint);
                firstTime = false;
            }
            else
            {
                firstPoint = secondPoint;
                secondPoint = polygonVertices[i] as Point;
                if (secondPoint.DistanceToPoint(firstPoint) > minDistanceBetweenVertices)
                {
                    validatedPolygonVertices.Add(secondPoint);
                }
            }
        }

        if (validatedPolygonVertices.Count == 2)
        {
            validatedPolygonVertices.Add(new Point((validatedPolygonVertices[1] as Point).x + minDistanceBetweenVertices,
                (validatedPolygonVertices[1] as Point).y + minDistanceBetweenVertices));
        }
        return validatedPolygonVertices;
    }

    public Polygon GetPoligon()
    {
        return this._figurePolygon;
    }

    public void SetPolygon(Polygon polygon)
    {
        this._figurePolygon = polygon;
    }



    public Vector3 GetForceDirection()
    {
        Vector3 tangencialDirection = new Vector3(cutter.SecondCutPoint.x - cutter.FirstCutPoint.x, cutter.SecondCutPoint.y - cutter.FirstCutPoint.y, 15);
        Vector3 normalDirection = Vector3.Cross(Vector3.back, tangencialDirection);
        return  - tangencialDirection.normalized * 5;
    }

    public Vector3 GetForceApplicationPoint()
    {
        Point middlPoint = new Point((cutter.SecondCutPoint.x + cutter.FirstCutPoint.x)/2.0f, (cutter.SecondCutPoint.y + cutter.FirstCutPoint.y)/2.0f);
        return new Vector3(middlPoint.x, middlPoint.y, 0);
    }
}
