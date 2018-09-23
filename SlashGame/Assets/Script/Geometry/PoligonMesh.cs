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

    /*private void SetPoligonMesh(Poligon aPoligon)
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
         #1#

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
                poligonOutline.SetPositions(aPoligon.GetPoligonVerticesAsVectors()); //Generates the outline of the polygon with the given vertices#1#

    }*/

    public LineRenderer GetPoligonOutline()
    {
        return poligonOutline;
    }

/*    public void SetPoligonOutline(LineRenderer lineRenderer)
    {
        this.poligonOutline = lineRenderer;
        SetPoligonMesh(this.poligon);
    }*/

    public static Mesh GetPoligonMesh(Poligon poligonFig)
    {
        // Use the triangulator to get indices for creating triangles
        Point[] vertices2D = poligonFig.GetPoligonVertices().ToArray(typeof(Point)) as Point[];
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

        Mesh invMsh = InvertMesh(msh);
        Mesh sepMsh = SeparateMeshes(msh, -10f);
        Mesh lateralMesh = GenerateLateralMesh(poligonFig, -10f);
        Mesh facesMsh = CombineMeshes(invMsh, sepMsh);
        Mesh finalMsh = CombineMeshes(lateralMesh, facesMsh);
        return finalMsh;
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
        invertedMesh.RecalculateNormals();
        invertedMesh.RecalculateBounds();
        return invertedMesh;
    }

    public static Mesh CombineMeshes(Mesh meshA, Mesh meshB)
    {
        Mesh combinedMesh = new Mesh();

        /*        Vector3[] combinedVertices = new Vector3[meshA.vertexCount + meshB.vertexCount];
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
                combinedMesh.triangles = combinedTriangles;*/

        /*Vector3[] combinedVertices = new Vector3[meshA.vertexCount + meshB.vertexCount];
        for (int i = 0; i < meshA.vertexCount; i++)
        {
            combinedVertices[i] = meshA.vertices[i];
        }
        for (int i = 0; i < meshB.vertexCount; i++)
        {
            combinedVertices[meshA.vertexCount + i] = meshB.vertices[i];
        }
        combinedMesh.vertices = combinedVertices;*/

        Vector3[] combinedVertices = new Vector3[meshA.vertexCount + meshB.vertexCount];
        meshA.vertices.CopyTo(combinedVertices, 0);
        meshB.vertices.CopyTo(combinedVertices, meshA.vertexCount);
        combinedMesh.vertices = combinedVertices;

        int[] combinedTriangles = new int[meshA.triangles.Length + meshB.triangles.Length];
        for (int i = 0; i < meshA.triangles.Length; i++)
        {
            combinedTriangles[i] = meshA.triangles[i];
            Debug.Log($"Triangle A: { meshA.triangles[i]}");
        }
        for (int i = 0; i < meshB.triangles.Length; i++)
        {
            combinedTriangles[meshA.triangles.Length + i] = meshB.triangles[i] + meshA.vertexCount;
            Debug.Log($"Triangle B: {meshB.triangles[i]}");
        }
        combinedMesh.triangles = combinedTriangles;

        combinedMesh.RecalculateNormals();
        combinedMesh.RecalculateBounds();
        Debug.Log("Volume: " + combinedMesh.bounds.size.x * combinedMesh.bounds.size.y * combinedMesh.bounds.size.z);

        return combinedMesh;
    }

    public static Mesh SeparateMeshes(Mesh meshA, float distance)
    {
        Mesh separatedMesh = new Mesh();
        Vector3[] separatedVertices = meshA.vertices;
        for (int i=0; i<meshA.vertices.Length; i++)
        {
            separatedVertices[i] = new Vector3(meshA.vertices[i].x , meshA.vertices[i].y , distance);
        }
        separatedMesh.vertices = separatedVertices;
        separatedMesh.triangles = meshA.triangles;

        separatedMesh.RecalculateNormals();
        separatedMesh.RecalculateBounds();

        return separatedMesh;
    }

    /**
     * Method that generate the lateral mesh to a 3D figure.
     * meshA and meshB must have the same vertices count
     */
    public static Mesh GenerateLateralMesh(Poligon poligon, float distance)
    {
        Mesh lateralMesh  = new Mesh();
        Vector3[] poligonVertices = poligon.GetPoligonVerticesAsVectors();

        int verticesPerFace = 4;
        //Lateral vertices
        Vector3[] lateralVertices = new Vector3[poligonVertices.Length  * verticesPerFace];
        for (int i = 0; i < lateralVertices.Length; i = i + verticesPerFace)
        {
            lateralVertices[i] = poligonVertices[ i/ verticesPerFace];
            lateralVertices[i + 1] = poligonVertices[ i / verticesPerFace] + new Vector3(0,0, distance);
            lateralVertices[i + 2] = poligonVertices[1 + i / verticesPerFace < poligonVertices.Length ? 1 + i / verticesPerFace : 0];
            lateralVertices[i + 3] = poligonVertices[1 + i / verticesPerFace < poligonVertices.Length ? 1 + i / verticesPerFace : 0] + new Vector3(0, 0, distance);
        }
        lateralMesh.vertices = lateralVertices;

        //Lateral mesh
        int triangleVerticesPerLateralFAce = 2 * 3; //Two triangles with 3 vertices each one
        int[] lateralTriangles = new int[poligonVertices.Length * triangleVerticesPerLateralFAce];
        for (int i = 0; i < poligonVertices.Length * triangleVerticesPerLateralFAce ; i = i + triangleVerticesPerLateralFAce)
        {
            //Two triangle that form a rectangle
            lateralTriangles[i] = verticesPerFace*i / triangleVerticesPerLateralFAce;
            lateralTriangles[i + 1] = 3 + verticesPerFace * ( i / triangleVerticesPerLateralFAce);
            lateralTriangles[i + 2] = 1 + verticesPerFace * ( i / triangleVerticesPerLateralFAce);

            lateralTriangles[i + 3] = verticesPerFace*(i / triangleVerticesPerLateralFAce);
            lateralTriangles[i + 4] = 2 + verticesPerFace * ( i / triangleVerticesPerLateralFAce);
            lateralTriangles[i + 5] = 3 + verticesPerFace * ( i / triangleVerticesPerLateralFAce);
        }
        lateralMesh.triangles = lateralTriangles;

        lateralMesh.RecalculateNormals();
        lateralMesh.RecalculateBounds();
        /*Vector3[] lateralMeshNormals = lateralMesh.normals;
        int indexCount = 0;
        foreach (Vector3 meshNormal in lateralMeshNormals)
        {
            lateralMeshNormals[indexCount] = -meshNormal;
            indexCount++;
        }
        lateralMesh.normals = lateralMeshNormals;
*/

        return lateralMesh;
    }
}
