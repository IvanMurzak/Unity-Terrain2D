using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain2D
{
    static partial class MathUtils
    {
        public static Mesh ToMesh(this Sprite sprite)
        {
            var mesh = new Mesh();

            //create vertices
            var vertices = new List<Vector3>();
            vertices.AddRange(Array.ConvertAll(sprite.vertices, i => (Vector3)i));
            var backVertices = new List<Vector3>(vertices);
            backVertices = ShiftVertices(backVertices);
            vertices.AddRange(backVertices);
            mesh.vertices = vertices.ToArray();

            //create triangles
            var triangles = new List<int>();
            triangles.AddRange(Array.ConvertAll(sprite.triangles, i => (int)i));
            var backTriangles = new List<int>(triangles);
            backTriangles = ShiftAndFlipTriangleIndexes(backTriangles, backVertices.Count);
            var middleTriangles = CreateMiddleTriangles(triangles, backTriangles);
            triangles.AddRange(backTriangles);
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            triangles.AddRange(middleTriangles);
            mesh.triangles = triangles.ToArray();

            //create uvs
            var uvs = new List<Vector2>(sprite.uv);
            var newUvs = new List<Vector2>(uvs);
            uvs.AddRange(newUvs);
            mesh.uv = uvs.ToArray();

            return mesh;
        }

        private static List<Vector3> ShiftVertices(List<Vector3> verticies)
        {
            var dist = 1f;

            for (int i = 0; i < verticies.Count; i++)
                verticies[i] = new Vector3(verticies[i].x, verticies[i].y, verticies[i].z + dist);

            return verticies;
        }

        private static List<int> ShiftAndFlipTriangleIndexes(List<int> indecies, int shiftAmount)
        {
            var toReturn = new List<int>(indecies);
            for (int i = 0; i < toReturn.Count; i++)
                toReturn[i] += shiftAmount;

            for (int i = 0; i < toReturn.Count; i += 3)
            {
                int i0 = toReturn[i];
                int i2 = toReturn[i + 2];

                toReturn[i] = i2;
                toReturn[i + 2] = i0;
            }

            return toReturn;
        }

        private static List<int> CreateMiddleTriangles(List<int> frontTriangles, List<int> backTriangles)
        {
            var toReturn = new List<int>();

            for (int i = 0; i < frontTriangles.Count; i += 3)
            {
                toReturn.AddRange(MakeQuad(frontTriangles[i], frontTriangles[i + 1], backTriangles[i + 2], backTriangles[i + 1]));
                toReturn.AddRange(MakeQuad(frontTriangles[i + 1], frontTriangles[i + 2], backTriangles[i + 1], backTriangles[i]));
                toReturn.AddRange(MakeQuad(frontTriangles[i + 2], frontTriangles[i], backTriangles[i], backTriangles[i + 2]));
            }
        
            return toReturn;
        }

        private static List<int> MakeQuad(int x0, int x1, int y0, int y1) => new List<int>()
        {
            x0, y0, y1,
            x0, y1, x1
        };
    }
}