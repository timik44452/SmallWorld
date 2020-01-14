using System;
using System.Collections.Generic;
using System.IO;
using Game.Service;
using Objects;
using UnityEngine;

namespace Game.Terrain.Service
{
    public class LoadedWorldObjects
    {
        private const string map_path = @"Map";

        private int region_size = 128;

        private List<WorldObject> loadedObjects;

        public LoadedWorldObjects()
        {
            loadedObjects = new List<WorldObject>();

            CreateDirectionIfNotCreated();
        }

        private void CreateDirectionIfNotCreated()
        {
            if (Directory.Exists(map_path))
            {
                return;
            }

            Directory.CreateDirectory(map_path);
        }

        public void UpdateRegion(Region region)
        {
            float x = region.X;
            float z = region.Z;

            int region_x = (int)(x / region_size);
            int region_z = (int)(z / region_size);

            int region_local_x = (int)((region_x == 0) ? x : x % region_x);
            int region_local_z = (int)((region_z == 0) ? z : z % region_z);

            if (region_local_x < 0)
            {
                region_local_x = region_size + region_local_x;
                region_x--;
            }

            if (region_local_z < 0)
            {
                region_local_z = region_size + region_local_z;
                region_z--;
            }

        }
    }
}
