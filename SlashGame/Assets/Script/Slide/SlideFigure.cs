﻿using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Geometry;
using UnityEngine;

public class SlideFigure
{

    private Poligon figurePoligon;
    private ArrayList figureCutPoints;

    public Poligon newPoligonA;
    public Poligon newPoligonB;

    public SlideFigure(Poligon poligon)
    {
        this.figurePoligon = poligon;
        this.figureCutPoints = new ArrayList();
    }

    public void resetCutPoints()
    {
        figureCutPoints.Clear();
        figurePoligon.ResetCutSegments();
    }

    public Boolean isReadyToCut()
    {
        return figureCutPoints.Count == 2;
    }

    public void CheckIntersection(Segment segment)
    {
        ArrayList intersectionPoints = figurePoligon.GetSegmentIntersectionPoints(segment);
        if (intersectionPoints.Count > 0)
        {
            figureCutPoints.Add(intersectionPoints[0]);
            Debug.Log($"Intersection point: {((Point)intersectionPoints[0])}, arrLength: {intersectionPoints.Count}");
        }
    }

    public void CutFigure()
    {
        ArrayList poligonSides = figurePoligon.GetPoligonSides();
        ArrayList poligonVertices = figurePoligon.GetPoligonVertices();
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
        newPoligonA = new Poligon(ValidatePoligon(firstFigureVertices));
        newPoligonB = new Poligon(ValidatePoligon(secondFigureVertices));

    }

    private ArrayList ValidatePoligon(ArrayList polygonVertices)
    {
        float minDistanceBetweenVertices = 0.1f;
        bool firstTime = true;
        Point firstPoint = null;
        Point secondPoint= null;
        int verticesCount = polygonVertices.Count;
        for (int i=0; i<verticesCount ; i++)
        {
            if (firstTime)
            {
                secondPoint = polygonVertices[i] as Point;
                firstTime = false;
            }
            else
            {
                firstPoint = secondPoint;
                secondPoint = polygonVertices[i] as Point;
                if (secondPoint.DistanceToPoint(firstPoint) < minDistanceBetweenVertices)
                {
                    polygonVertices.Remove(secondPoint);
                    verticesCount--;
                    Debug.Log("Distance between points: " + secondPoint.DistanceToPoint(firstPoint));
                }
            }
            

        }
        return polygonVertices;
    }

    public Poligon GetPoligon()
    {
        return this.figurePoligon;
    }

    public void setPoligo(Poligon poligon)
    {
        this.figurePoligon = poligon;
    }

    public Vector3 GetForceDirection()
    {
        Vector3 tangencialDirection = new Vector3(((Point)figureCutPoints[1]).x - ((Point)figureCutPoints[0]).x, ((Point)figureCutPoints[1]).y - ((Point)figureCutPoints[0]).y);
        Vector3 normalDirection = Vector3.Cross(Vector3.back, tangencialDirection);
        return tangencialDirection.normalized * 5;
    }
}
