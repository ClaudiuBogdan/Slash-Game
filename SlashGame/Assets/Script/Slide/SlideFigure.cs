using System;
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
        /**
         * ToDo: add a condition that ensure the cut point and the closest segment vertex isn't close enough.
         */

        /**
         * ToDo: start the polygon from the cut vertex.
         */
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
                Debug.Log("Distance Cut pointA: " + cutPoint.DistanceToPoint(segment.GetPointA()));
                Debug.Log("Distance Cut pointB: " + cutPoint.DistanceToPoint(segment.GetPointB()));
                if ((cutPoint.DistanceToPoint(segment.GetPointA()) > Point.epsiloError/*px*/) )
                {
                    containerFigureVertices.Add(cutPoint);
                }

                /*cutPoint = cutPoint.DistanceToPoint(segment.getPointA()) < Point.epsiloError /*px#1# ? segment.getPointA() : cutPoint;
                cutPoint = cutPoint.DistanceToPoint(segment.getPointB()) < Point.epsiloError /*px#1# ? segment.getPointB() : cutPoint;
                Debug.Log("Cut point: " + cutPoint);*/
                

                containerFigureVertices = containerFigureVertices == firstFigureVertices
                    ? secondFigureVertices
                    : firstFigureVertices;
                if ((cutPoint.DistanceToPoint(segment.GetPointB()) > Point.epsiloError) /*px*/)
                {
                    containerFigureVertices.Add(cutPoint);
                }
                   
                
            }
        }
        newPoligonA = new Poligon(firstFigureVertices);
        newPoligonB = new Poligon(secondFigureVertices);

    }

    public Poligon GetPoligon()
    {
        return this.figurePoligon;
    }

    public void setPoligo(Poligon poligon)
    {
        this.figurePoligon = poligon;
    }
}
