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
#endif

        public void Build()
        {
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
                    ? instances[spawnedInstanceIndex++]
                    : null;

                if (instanceData == null)
                {
                    var block = config.blocks[0];
#if UNITY_EDITOR
                    var instance = UnityEditor.PrefabUtility.InstantiatePrefab(block.prefab, transform) as GameObject;
#else
                    var instance = Instantiate(block.prefab, Vector3.zero, quaternion.identity, transform);
#endif
                    instanceData = new InstanceData(block, instance);
                    instances.Add(instanceData);
                }

                fillLength += instanceData.block.GetLength();
                ApplyDeformVisual(instanceData, spline.Spline, t);
                ApplyDeformCollider(instanceData, spline.Spline, t);
            }
        }
        bool ApplyDeformCollider(InstanceData instanceData, Spline spline, float t)
        {
            switch (instanceData.block.deformCollider)
            {
                case DeformCollider.None:
                    break;
                case DeformCollider.PolygonCollider2D:
                    break;
                case DeformCollider.EdgeCollider2D:
                    break;
            }
            return false;
        }
        bool ApplyDeformVisual(InstanceData instanceData, Spline spline, float t)
        {
            switch (instanceData.block.visualSource)
            {
                case VisualSource.Sprite:
                    return ApplyDeformSprite(instanceData, spline, t);
                default:
                    return false;
            }
        }
        bool ApplyDeformSprite(InstanceData instanceData, Spline spline, float t)
        {
            foreach (var terrainBlockRenderer in instanceData.instance.GetComponentsInChildren<Terrain2D_BlockRenderer>())
                terrainBlockRenderer.Build(spline, t);

            return true;
        }
    }
}
