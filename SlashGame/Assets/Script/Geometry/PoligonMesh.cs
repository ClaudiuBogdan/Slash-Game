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
        msh = InvertMesh(msh);
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
        Mesh invMsh = InvertMesh(msh);
        msh = CombineMeshes(invMsh, msh);
        return msh;
    }

    public static Mesh InvertMesh(Mesh mesh)
    {
        Mesh invertedMesh = new Mesh();
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
            
        invertedMesh.vertices = mesh.vertices;
        invertedMesh.normals = normals;
        
        for (int m = 0; m < mesh.subMeshCount; m++)
        {
            int[] triangles = mesh.GetTriangles(m);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                int temp = triangles[i + 0];
                triangles[i + 0] = triangles[i + 1];
                triangles[i + 1] = temp;
            }
            invertedMesh.SetTriangles(triangles, m);
        }

        return invertedMesh;
    }

    public static Mesh CombineMeshes(Mesh meshA, Mesh meshB)
    {
        Mesh combinedMesh = new Mesh();

        Vector3[] combinedVertices = new Vector3[meshA.vertexCount + meshB.vertexCount];
        meshA.vertices.CopyTo(combinedVertices, 0);
        meshB.vertices.CopyTo(combinedVertices, meshB.vertexCount);
        combinedMesh.vertices = combinedVertices;

        Vector3[] combinedNormals = new Vector3[meshA.normals.Length + meshB.normals.Length];
        meshA.normals.CopyTo(combinedNormals, 0);
        meshB.normals.CopyTo(combinedNormals, meshB.normals.Length);
        combinedMesh.normals = combinedNormals;

        int[] combinedTriangles = new int[meshA.triangles.Length + meshB.triangles.Length];
        meshA.triangles.CopyTo(combinedTriangles, 0);
        meshB.triangles.CopyTo(combinedTriangles, meshB.triangles.Length);
        combinedMesh.triangles = combinedTriangles;

        combinedMesh.RecalculateNormals();
        combinedMesh.RecalculateBounds();

        return combinedMesh;
    }
}
