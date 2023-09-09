using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Terrain2D.Data
{
    [Serializable]
    public class BlockData
    {
        public GameObject prefab;

        public bool deformVisual = true;

        public VisualSource visualSource;
        public DeformCollider deformCollider;

        public float3 minScale = new float3(1, 1, 1);
        public float3 maxScale = new float3(1, 1, 1);

        public bool flippableX;
        public bool flippableY;
        public bool flippableZ;

        public float GetLength()
        {
            switch (visualSource)
            {
                case VisualSource.Sprite:
                    var renderers = prefab.GetComponentsInChildren<Terrain2D_BlockRenderer>();
                    var left = renderers.Min(x => x.transform.TransformPoint(x.sprite.bounds.min).x);
                    var right = renderers.Min(x => x.transform.TransformPoint(x.sprite.bounds.max).x);
                    return math.abs(right - left);
                default:
                    return 0;
            }
        }
    }

    public enum VisualSource
    {
        Sprite
    }

    public enum DeformCollider
    {
        None, PolygonCollider2D, EdgeCollider2D
    }
}
