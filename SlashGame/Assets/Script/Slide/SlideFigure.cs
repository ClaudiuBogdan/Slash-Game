using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Geometry;
using UnityEngine;

public class SlideFigure
{

    private Polygon _figurePolygon;
    private ArrayList figureCutPoints;

    public Polygon BigPolygon;
    public Polygon SmallPolygon;

    public SlideFigure(Polygon polygon)
    {
        SetPolygon(polygon);
        this.figureCutPoints = new ArrayList();
    }

    public void resetCutPoints()
    {
        figureCutPoints.Clear();
        _figurePolygon.ResetCutSegments();
    }

    public Boolean isReadyToCut()
    {
        return figureCutPoints.Count == 2;
    }

    public void CheckIntersection(Segment segment)
    {
        ArrayList intersectionPoints = _figurePolygon.GetSegmentIntersectionPoints(segment);
        if (intersectionPoints.Count > 0)
{
            Debug.Log($"0 Intersection point: {((Point)intersectionPoints[0])}, arrLength: {figureCutPoints.Count}");
            if (figureCutPoints.Count > 0)
            {
                Debug.Log($"2 Intersection point: {((Point)intersectionPoints[0])}, arrLength: {figureCutPoints.Count}");
                Segment cuttingSegment = new Segment(figureCutPoints[0] as Point, intersectionPoints[0] as Point);
                if (_figurePolygon.IsPointInsidePolygon(cuttingSegment.GetMiddlePoint()))
                {
                    figureCutPoints.Add(intersectionPoints[0]);
                }
            }
            else
            {
                figureCutPoints.Add(intersectionPoints[0]);
                Debug.Log($"1 Intersection point: {((Point)intersectionPoints[0])}, arrLength: {figureCutPoints.Count}");
            }


        }
    }

    public void CutFigure()
    {
        Debug.Log($"Cut point 1: {figureCutPoints[0] as Point}");
        Debug.Log($"Cut point 2: {figureCutPoints[1] as Point}");
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
                Point cutPoint = segment.ContainsPoint(figureCutPoints[0] as Point)
                    ? figureCutPoints[0] as Point
                    : figureCutPoints[1] as Point;
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
        Vector3 tangencialDirection = new Vector3(((Point)figureCutPoints[1]).x - ((Point)figureCutPoints[0]).x, ((Point)figureCutPoints[1]).y - ((Point)figureCutPoints[0]).y, 15);
        Vector3 normalDirection = Vector3.Cross(Vector3.back, tangencialDirection);
        return  - tangencialDirection.normalized * 5;
    }

    public Vector3 GetForceApplicationPoint()
    {
        Point middlPoint = new Point((((Point)figureCutPoints[1]).x + ((Point)figureCutPoints[0]).x)/2.0f, (((Point)figureCutPoints[1]).y + ((Point)figureCutPoints[0]).y)/2.0f);
        return new Vector3(middlPoint.x, middlPoint.y, 0);
    }
}
