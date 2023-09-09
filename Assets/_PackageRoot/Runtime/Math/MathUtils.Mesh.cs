using System;
using UnityEngine;

namespace Terrain2D
{
    static partial class MathUtils
    {
        public static Mesh ToMesh(this Sprite sprite)
        {
            var mesh = new Mesh();

            mesh.vertices = Array.ConvertAll(sprite.vertices, i => (Vector3)i);
            mesh.triangles = Array.ConvertAll(sprite.triangles, i => (int)i);
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.uv = sprite.uv;

            return mesh;
        }
        public static Mesh SetVertexColor(this Mesh mesh, Color color)
        {
            mesh.colors = new Color[mesh.vertexCount];
            
            for (int i = 0; i < mesh.vertexCount; i++)
                mesh.colors[i] = color;
            
            return mesh;
        }
    }
}