using UnityEngine;
using UnityEngine.Splines;

namespace Terrain2D
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class Terrain2D_BlockRenderer : MonoBehaviour 
    {
        public Sprite sprite;
        public Material[] materials;

        [HideInInspector]
        public GameObject prefabReference;

        MeshFilter meshFilter;
        SkinnedMeshRenderer skinnedMeshRenderer;

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        }

        public void Build(Spline spline, float t)
        {
            if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
            if (skinnedMeshRenderer == null) skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

            var mesh = sprite.ToMesh();
                mesh.name = "Terrain";
                // mesh.vertices = MathUtils.DeformVertices(spline, t, mesh.vertices);
            
            meshFilter.sharedMesh = mesh;
            skinnedMeshRenderer.sharedMesh = mesh;
            skinnedMeshRenderer.materials = materials;
        }
    }
}