using System.Collections;
using System.Collections.Generic;
using Assets.Script.Geometry;
using UnityEngine;

public class PoligonMesh
{

    private Poligon poligon;
    private Mesh poligonMesh;

    public PoligonMesh(Poligon poligon)
    {
        this.poligon = poligon;
        SetPoligonMesh(poligon);
    }

    private void SetPoligonMesh(Poligon aPoligon)
    {
        ArrayList meshElementList = new ArrayList();
        /*
         * 1. Select one of the polygon vertices.
         * 2. Check if the angle between sibling vertices is less than 180 degrees
         *      a. if is equal or grater, go to 1.
         * 3. Create a mesh with the three vertices
         * 4.Eliminate the selected vertices from the arrayList
         * 5.Check if the arrayList size is grater the two
         *      a.If it is, back to 1
         *      b.If not, end.
         */

    }
}
