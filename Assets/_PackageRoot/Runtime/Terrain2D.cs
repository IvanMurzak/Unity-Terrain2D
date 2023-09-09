using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using Terrain2D.Data;

namespace Terrain2D
{
    [ExecuteInEditMode]
    public class Terrain2D : MonoBehaviour
    {
        [SerializeField] Terrain2DConfig config;
        [SerializeField] SplineContainer spline;

        [HideInInspector]
        [SerializeField] List<InstanceData> instances = new List<InstanceData>();

#if UNITY_EDITOR
        void Awake()
        {
            if (spline == null)
                spline = GetComponent<SplineContainer>();
        }
        void Update()
        //void OnEnable()
        {
            if (enabled && config && spline)
                Build();
        }
        void OnDrawGizmosSelected() 
        {
            float3 position, tangent, upVector;
            var lines = 30;
            for (int i = 0; i < lines; i++)
            {
                var t = i / lines;
                if (!spline.Evaluate(t, out position, out tangent, out upVector))
                {
                    Debug.LogError($"spline.Evaluate(t = {t}) returned false");
                }
                Debug.DrawLine(position, position + upVector, Color.red, 10f, true);
            }
        }
#endif

        public void Build()
        {
            spline.Spline.Knots = spline.Spline.Knots.Select(x =>
            {
                x.Rotation = Quaternion.Euler(90, 0, 0);
                return x;
            }).ToList();
            var length = spline.Spline.GetLength();
            var fillLength = 0f;
            var spawnedInstanceIndex = 0;

            for (int i = 0; i < instances.Count; i++)
            {
                if (instances[i] == null || instances[i].instance == null || instances[i].block == null)
                    instances.RemoveAt(i--);
            }

            while (fillLength < length)
            {
                var t = fillLength / length;
                var instanceData = spawnedInstanceIndex < instances.Count
                    ? instances[spawnedInstanceIndex]
                    : null;

                if (instanceData == null)
                {
                    var block = config.blocks[0];
                    if (block.GetLength() > length - fillLength)
                        break;
#if UNITY_EDITOR
                    var instance = UnityEditor.PrefabUtility.InstantiatePrefab(block.prefab, transform) as GameObject;
#else
                    var instance = Instantiate(block.prefab, Vector3.zero, quaternion.identity, transform);
#endif
                    instanceData = new InstanceData(block, instance);
                    instances.Add(instanceData);
                }
                spawnedInstanceIndex++;

                ApplyDeformVisual(instanceData, spline.Spline, t);
                ApplyDeformCollider(instanceData, spline.Spline, t);

                fillLength += instanceData.block.GetLength();
            }
            
            while(instances.Count > spawnedInstanceIndex)
            {
                DestroyImmediate(instances.Last().instance.gameObject);
                instances.RemoveAt(instances.Count - 1);
            }
        }
        bool ApplyDeformCollider(InstanceData instanceData, Spline spline, float t)
        {
            switch (instanceData.block.deformCollider)
            {
                case DeformCollider.None:
                    break;
                case DeformCollider.PolygonCollider2D:
                {
                    var refColliders = instanceData.block.prefab.GetComponentsInChildren<PolygonCollider2D>();
                    var colliders = instanceData.instance.GetComponentsInChildren<PolygonCollider2D>();
                    for (var i = 0; i < refColliders.Length; i ++)
                        colliders[i].points = MathUtils.DeformVertices(spline, t, refColliders[i].points);
                    break;
                }
                case DeformCollider.EdgeCollider2D:
                {
                    var refColliders = instanceData.block.prefab.GetComponentsInChildren<EdgeCollider2D>();
                    var colliders = instanceData.instance.GetComponentsInChildren<EdgeCollider2D>();
                    for (var i = 0; i < refColliders.Length; i ++)
                        colliders[i].points = MathUtils.DeformVertices(spline, t, refColliders[i].points);
                    break;
                }
            }
            return false;
        }
        bool ApplyDeformVisual(InstanceData instanceData, Spline spline, float t)
        {
            switch (instanceData.block.visualSource)
            {
                case VisualSource.Sprite:
                    foreach (var terrainBlockRenderer in instanceData.instance.GetComponentsInChildren<Terrain2D_BlockRenderer>())
                        terrainBlockRenderer.Build(spline, t);
                    return true;
                default:
                    return false;
            }
        }
    }
}
