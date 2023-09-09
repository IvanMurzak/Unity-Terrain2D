using System;
using UnityEngine;
using UnityEngine.Splines;

namespace Terrain2D
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class Terrain2D_BlockRenderer : MonoBehaviour 
    {
        public Sprite sprite;
        public Color tint;
        public bool deformVertices = true;
        public bool applyTintToVertexColor = true;
        public string materialPropertyTexture = "_MainTex";
        public string materialPropertyColor = "_Color";

        public string materialPropertyNormalMap = "_NormalMap";
        public string spriteSecondaryTextureNormalMapName = "_NormalMap";

        public Material[] materials;

        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void Build(Spline spline, float t)
        {
            if (meshFilter == null) meshFilter = GetComponent<MeshFilter>();
            if (meshRenderer == null) meshRenderer = GetComponent<MeshRenderer>();

            var mesh = sprite.ToMesh();
                mesh.name = $"Terrain-{sprite.name}";

            if (applyTintToVertexColor)
                mesh = mesh.SetVertexColor(tint);

            if (deformVertices)
                mesh.vertices = MathUtils.DeformVertices(spline, t, mesh.vertices);

            var center = mesh.bounds.center;
            mesh.vertices = Array.ConvertAll(mesh.vertices, x => x - center);
            transform.position = center;
            
            meshFilter.sharedMesh = mesh;
            meshRenderer.sharedMaterials = materials;

            foreach (var material in meshRenderer.sharedMaterials)
            {
                material.SetTexture(materialPropertyTexture, sprite.texture);
                material.SetColor(materialPropertyColor, tint);
            }
        }
    }
}