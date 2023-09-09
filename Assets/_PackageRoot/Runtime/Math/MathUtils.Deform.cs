using System;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Terrain2D
{
    static partial class MathUtils
    {
        public static Vector2[] DeformVertices(Spline spline, float t, Vector2[] vertices)
        {
            var vertices3d = Array.ConvertAll(vertices, x => (Vector3)x);
            vertices3d = DeformVertices(spline, t, vertices3d);
            return Array.ConvertAll(vertices3d, x => (Vector2)x);
        }
        public static Vector3[] DeformVertices(Spline spline, float t, Vector3[] vertices)
        {
            var splineLength = spline.GetLength();
            var left = vertices.Min(x => x.x);

            float3 position, tangent, upVector;
            for (var i = 0; i < vertices.Length; i++)
            {
                var v = vertices[i];
                var internalT = t + (v.x - left) / splineLength;
                if (!spline.Evaluate(internalT, out position, out tangent, out upVector))
                {
                    Debug.LogError($"spline.Evaluate(t = {internalT}) returned false");
                    return null;
                }
                upVector = Quaternion.Euler(-90, 0, 0) * upVector;
                v = position + upVector * v.y;

                vertices[i] = v;
            }
            return vertices;
        }
    }
}