using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Script.Geometry;
using Assets.Script.Mesh;
using UnityEngine;

public class PoligonMesh
{

    private Poligon poligon;
    private Mesh poligonMesh;
    private Triangle triangle;
    private LineRenderer poligonOutline;

    public PoligonMesh(Poligon poligon)
    {
        this.poligon = poligon;
        //SetPoligonMesh(poligon);
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

        // Use the triangulator to get indices for creating triangles
        Point[] vertices2D = aPoligon.GetPoligonVertices().ToArray(typeof(Point)) as Point[];
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        this.poligonMesh = msh;
        /*        int i = aPoligon.GetPoligonVertices().Count;
                poligonOutline.positionCount = i;
                poligonOutline.SetPositions(aPoligon.GetPoligonVerticesAsVectors()); //Generates the outline of the polygon with the given vertices*/

    }

    public LineRenderer GetPoligonOutline()
    {
        return poligonOutline;
    }

    public void SetPoligonOutline(LineRenderer lineRenderer)
    {
        this.poligonOutline = lineRenderer;
        SetPoligonMesh(this.poligon);
    }

    public static Mesh GetPoligonMesh(Poligon poligonFig)
    {
        // Use the triangulator to get indices for creating triangles
        Point[] vertices2D = poligonFig.GetPoligonVertices().ToArray(typeof(Point)) as Point[];
        Triangulator tr = new Triangulator(vertices2D);
        int[] indices = tr.Triangulate();
        //indices.Reverse();
        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[vertices2D.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
        }

        // Create the mesh
        Mesh msh = new Mesh();
        msh.vertices = vertices;
        msh.triangles = indices;
        msh.RecalculateNormals();
        msh.RecalculateBounds();

        return msh;
    }
}
