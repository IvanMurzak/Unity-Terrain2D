using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Terrain2D
{
    static partial class MathUtils
    {
        public static Vector3[] DeformVertices(Spline spline, float t, Vector3[] vertices)
        {
            var splineLength = spline.GetLength();

            var left = vertices.Min(x => x.x);
            // var right = vertices.Max(x => x.x);
            // var length = right - left;

            var deformedVertices = new Vector3[vertices.Length];

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
                v.x += upVector.x;
                v.y += upVector.y;
                v.z += upVector.z;

                deformedVertices[i] = v;
            }
            return deformedVertices;
        }
    }
}