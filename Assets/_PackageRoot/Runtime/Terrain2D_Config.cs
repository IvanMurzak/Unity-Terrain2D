using System.Collections.Generic;
using Terrain2D.Data;
using UnityEngine;

namespace Terrain2D
{   
    [CreateAssetMenu(fileName = "Terrain2D_Config", menuName = "Tools/Terrain2D/Create Config", order = 0)]
    public class Terrain2DConfig : ScriptableObject 
    {
        public List<BlockData> blocks;
    }
}