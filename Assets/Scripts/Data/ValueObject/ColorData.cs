using System;
using System.Collections.Generic;
using UnityEngine;
namespace Data.ValueObject
{
    [Serializable]
    public class ColorData
    {
        public Material RainbowMaterial;
        public Material RainbowGageMaterial;
        public Material ColorMaterial;
        public List<Color> color = new List<Color>();
    }
}