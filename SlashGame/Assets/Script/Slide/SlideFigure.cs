using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Geometry;
using UnityEngine;

public class SlideFigure
{

    private Poligon figurePoligon;
    private ArrayList figureCutPoints;

    public SlideFigure(Poligon poligon)
    {
        this.figurePoligon = poligon;
    }

    public void resetPoints()
    {
        figureCutPoints.Clear();
        figurePoligon.ResetCutSegments();
    }

    public Boolean isCut()
    {
        return figureCutPoints.Count == 2;
    }

    public void CheckIntersection(Segment segment)
    {
        ArrayList intersectionPoints = figurePoligon.GetSegmentIntersectionPoints(segment);
        if (intersectionPoints.Count > 0)
        {
            figureCutPoints.Add(intersectionPoints[0]);
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

        Poligon newPoligonA = new Poligon(firstFigureVertices);
        Poligon newPoligonB = new Poligon(secondFigureVertices);

    }

}
