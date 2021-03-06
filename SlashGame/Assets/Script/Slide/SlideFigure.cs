﻿using System;
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

    public void ResetCutPoints()
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
        ArrayList intersectionPointsList = _figurePolygon.GetSegmentIntersectionPoints(segment);
        if (intersectionPointsList.Count > 0)
        {
            Point intersectionPoints = intersectionPointsList[0] as Point;
            if (!cutter.IsEmpty())
            {
                Segment cuttingSegment = new Segment(cutter.FirstCutPoint, intersectionPoints);
                if (_figurePolygon.IsPointInsidePolygon(cuttingSegment.GetMiddlePoint()))
                {
                    int indexSegmentCut = _figurePolygon.GetSegmentIntersectionIndex(intersectionPoints);
                    cutter.SetCutPoint(intersectionPoints, indexSegmentCut);
                }
            }
            else
            {
                int indexSegmentCut = _figurePolygon.GetSegmentIntersectionIndex(intersectionPoints);
                cutter.SetCutPoint(intersectionPoints, indexSegmentCut);
            }


        }
    }

    public void CutFigure()
    {
        ArrayList polygonSides = _figurePolygon.GetPolygonSides();
        ArrayList poligonVertices = _figurePolygon.GetPolygonVertices();

        ArrayList firstFigureVertices = new ArrayList();
        ArrayList secondFigureVertices = new ArrayList();

        firstFigureVertices.Add(cutter.GetFirstIndexCutPoint()); //Start with the fist cut point to form the new figure
        int indexPolygonSide = cutter.GetLeastIndexCut();
        indexPolygonSide = indexPolygonSide + 1 < polygonSides.Count ? indexPolygonSide + 1 : 0;
        while (indexPolygonSide != cutter.GetLargestIndexCut()) //Stop when reached the second segment cut.
        {
            firstFigureVertices.Add(poligonVertices[indexPolygonSide]);
            indexPolygonSide = indexPolygonSide + 1 < polygonSides.Count ? indexPolygonSide + 1 : 0;
        }
        firstFigureVertices.Add(poligonVertices[indexPolygonSide]);
        firstFigureVertices.Add(cutter.GetSecondIndexCutPoint());

        secondFigureVertices.Add(cutter.GetSecondIndexCutPoint()); //Start with the second cut point to form the new figure
        indexPolygonSide = cutter.GetLargestIndexCut();
        indexPolygonSide = indexPolygonSide + 1 < polygonSides.Count ? indexPolygonSide + 1 : 0;
        while (indexPolygonSide != cutter.GetLeastIndexCut()) //Stop when reached the first segment cut.
        {
            secondFigureVertices.Add(poligonVertices[indexPolygonSide]);
            indexPolygonSide = indexPolygonSide + 1 < polygonSides.Count ? indexPolygonSide + 1 : 0;
        }
        secondFigureVertices.Add(poligonVertices[indexPolygonSide]);
        secondFigureVertices.Add(cutter.GetFirstIndexCutPoint());

        Polygon polygonA = new Polygon(ValidatePoligon(firstFigureVertices));
        Polygon polygonB = new Polygon(ValidatePoligon(secondFigureVertices));
        BigPolygon = polygonA.GetArea() < polygonB.GetArea() ? polygonA : polygonB;
        SmallPolygon = BigPolygon == polygonA ? polygonB : polygonA;

        SetPolygon(this.BigPolygon);
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
        Vector3 tangencialDirection = new Vector3(cutter.SecondCutPoint.x - cutter.FirstCutPoint.x, cutter.SecondCutPoint.y - cutter.FirstCutPoint.y, 40);
        Vector3 normalDirection = Vector3.Cross(Vector3.back, tangencialDirection);
        return  - tangencialDirection.normalized * 5;
    }

    public Vector3 GetForceApplicationPoint()
    {
        Point middlPoint = new Point((cutter.SecondCutPoint.x + cutter.FirstCutPoint.x)/2.0f, (cutter.SecondCutPoint.y + cutter.FirstCutPoint.y)/2.0f);
        return new Vector3(middlPoint.x, middlPoint.y, 0);
    }
}
