using System.Linq;
using System.Collections.Generic;
using System;

namespace Game.Terrain.Service
{
    public class UVMap
    {
        private List<UVData> datas;

        public UVMap()
        {
            datas = new List<UVData>();
        }

        public void AddUVData(int ID, float x, float y, float width, float height)
        {
            datas.Add(new UVData(ID, x, y, width, height));
        }

        public void AddUVData(float x, float y, float width, float height)
        {
            int id = 0;

            if(datas.Count > 0)
            {
                id = datas.Max(data => data.ID) + 1;
            }

            AddUVData(id, x, y, width, height);
        }

        public UVData GetUV(int b_type)
        {
            return datas.Find(x => x.ID == b_type);
        }
    }
}
