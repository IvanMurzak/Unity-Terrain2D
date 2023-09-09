using System;
using System.Collections.Generic;
using UnityEngine;

namespace Terrain2D.Data
{
    [Serializable]
    public class InstanceData
    {
        public GameObject instance;
        public BlockData block;

        public GameObject GetPrefab(GameObject instance) => refInstanceToPrefab[instance];

        [SerializeField] Dictionary<GameObject, GameObject> refInstanceToPrefab = new Dictionary<GameObject, GameObject>();

        public InstanceData(BlockData block, GameObject instance)
        {
            this.block = block;
            this.instance = instance;

            AddToRefDictionary(instance, block.prefab);
        }
        void AddToRefDictionary(GameObject instance, GameObject prefab)
        {
            refInstanceToPrefab.Add(instance, block.prefab);
            
            for (var i = 0; i < instance.transform.childCount; i++)
                AddToRefDictionary(instance.transform.GetChild(i).gameObject, prefab.transform.GetChild(i).gameObject);
        }
    }
}
