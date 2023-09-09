using UnityEngine;

namespace Terrain2D
{
    static class Extensions
    {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
                component = gameObject.AddComponent<T>();

            return component;
        }
    }
}