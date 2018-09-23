using Assets.Script.Mesh;
using UnityEngine;

namespace Assets.Script.Geometry
{
    public class PolygonMesh
    {

        private Polygon _polygon;
        private UnityEngine.Mesh poligonMesh;
        private Triangle triangle;
        private LineRenderer poligonOutline;

        public PolygonMesh(Polygon polygon)
        {
            this._polygon = polygon;
        }
    
        public static UnityEngine.Mesh GetPolygonMesh(Polygon polygonFig)
        {
            // Use the triangulator to get indices for creating triangles
            Point[] vertices2D = polygonFig.GetPolygonVertices().ToArray(typeof(Point)) as Point[];
            Triangulator tr = new Triangulator(vertices2D);
            int[] indices = tr.Triangulate();

            // Create the Vector3 vertices
            Vector3[] vertices = new Vector3[vertices2D.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = new Vector3(vertices2D[i].x, vertices2D[i].y, 0);
            }

            float separationBetweenFaces = 0.5f;

            // Create the mesh
            UnityEngine.Mesh msh = new UnityEngine.Mesh();
            msh.vertices = vertices;
            msh.triangles = indices;

            msh.RecalculateNormals();
            msh.RecalculateBounds();

            UnityEngine.Mesh invMsh = InvertMesh(msh);
            UnityEngine.Mesh sepMsh = SeparateMeshes(msh, -separationBetweenFaces);
            UnityEngine.Mesh lateralMesh = GenerateLateralMesh(polygonFig, -separationBetweenFaces);
            UnityEngine.Mesh facesMsh = CombineMeshes(invMsh, sepMsh);
            UnityEngine.Mesh finalMsh = CombineMeshes(lateralMesh, facesMsh);
            return finalMsh;
        }

        public static UnityEngine.Mesh InvertMesh(UnityEngine.Mesh mesh)
        {
            UnityEngine.Mesh invertedMesh = new UnityEngine.Mesh();
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
            invertedMesh.RecalculateBounds();
            return invertedMesh;
        }

        public static UnityEngine.Mesh CombineMeshes(UnityEngine.Mesh meshA, UnityEngine.Mesh meshB)
        {
            UnityEngine.Mesh combinedMesh = new UnityEngine.Mesh();

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

        public static UnityEngine.Mesh SeparateMeshes(UnityEngine.Mesh meshA, float distance)
        {
            UnityEngine.Mesh separatedMesh = new UnityEngine.Mesh();
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
        public static UnityEngine.Mesh GenerateLateralMesh(Polygon polygon, float distance)
        {
            UnityEngine.Mesh lateralMesh  = new UnityEngine.Mesh();
            Vector3[] poligonVertices = polygon.GetPoligonVerticesAsVectors();

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

            return lateralMesh;
        }
    }
}
