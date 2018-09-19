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
            Debug.Log("Intersection point: " + ((Point)intersectionPoints[0]));
        }
    }

    public void CutFigure()
    {
        if(figureCutPoints.Count < 2)
            return;
        ArrayList poligonSides = figurePoligon.GetPoligonSides();
        ArrayList poligonVertices = figurePoligon.GetPoligonVertices();
        Point firstCutPoint = null;
        ArrayList firstFigureVertices = new ArrayList();
        ArrayList secondFigureVertices = new ArrayList();
        ArrayList containerFigureVertices = firstFigureVertices;
        for (int i = 0; i < poligonSides.Count; i++)
        { 
            containerFigureVertices.Add(poligonVertices[i]);
            if (((Segment) poligonSides[i]).isSegmentCut())
            {
                Point cutPoint = ((Segment) poligonSides[i]).ContainsPoint(figureCutPoints[0] as Point)
                    ? figureCutPoints[0] as Point
                    : figureCutPoints[1] as Point;
                int lastPoligonSegmentsIndex = poligonVertices.Count - 1;
                
                containerFigureVertices.Add(cutPoint);
                if (containerFigureVertices == firstFigureVertices)
                {
                    firstCutPoint = cutPoint;
                }
                else
                {
                    containerFigureVertices.Add(firstCutPoint);
                }

                containerFigureVertices = containerFigureVertices == firstFigureVertices
                    ? secondFigureVertices
                    : firstFigureVertices;
                containerFigureVertices.Add(cutPoint);
                
            }
        }
        firstFigureVertices.Add(poligonVertices[0]);

        newPoligonA = new Poligon(firstFigureVertices);
        newPoligonB = new Poligon(secondFigureVertices);

    }

}
