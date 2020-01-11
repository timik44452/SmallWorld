using UnityEngine;

namespace Game.Terrain.Service
{
    [System.Serializable]
    public class SmallWorldTerrainSettings
    {
        public float power = 0.1F;

        public Texture2D groundTexture;
        public Texture2D grassTexture;
        public Texture2D rockTexture;
        public Texture2D snowTexture;
    }
}