﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Terrain.Service
{
    public class LoadedTerrainRegion
    {
        private const string map_path = @"Map";

        private int region_size = 1024;

        private TerrainRegion lastUsedTerrain;
        private List<TerrainRegion> loadedRegions;

        public LoadedTerrainRegion()
        {
            loadedRegions = new List<TerrainRegion>();

            CreateDirectionIfNotCreated();
        }

        public float GetHeight(float x, float z)
        {
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

            var region = LoadIfNotLoaded(region_x, region_z);

            return region.GetHeight(region_local_x, region_local_z);
        }

        private void CreateDirectionIfNotCreated()
        {
            if(Directory.Exists(map_path))
            {
                return;
            }

            Directory.CreateDirectory(map_path);
        }

        private TerrainRegion LoadIfNotLoaded(int x, int z)
        {
            TerrainRegion terrainRegion = null;

            if (lastUsedTerrain != null && lastUsedTerrain.X == x && lastUsedTerrain.Z == z)
            {
                terrainRegion = lastUsedTerrain;
            }
            else
            {
                terrainRegion = loadedRegions.Find(region => region.X == x && region.Z == z);
            }

            if (terrainRegion == null)
            {
                string path = map_path + $"//region{x}_{z}.region";

                if (File.Exists(Path.Combine(path)))
                {
                    terrainRegion = new TerrainRegion(path);

                    loadedRegions.Add(terrainRegion);
                }
                else
                {
                    terrainRegion = CreateRegion(x, z);

                    loadedRegions.Add(terrainRegion);

                    terrainRegion.Save(path);
                }
            }

            lastUsedTerrain = terrainRegion;

            return terrainRegion;
        }

        private TerrainRegion CreateRegion(int x, int z)
        {
            TerrainRegion region = new TerrainRegion(x, z, region_size, region_size);

            for (int local_x = 0; local_x < region_size; local_x++)
            {
                for (int local_z = 0; local_z < region_size; local_z++)
                {
                    float perlin_x = Mathf.Round(x + local_x) * 0.065F;
                    float perlin_z = Mathf.Round(z + local_z) * 0.065F;

                    float height = Mathf.PerlinNoise(perlin_x, perlin_z) * 8.0F;

                    region.SetHeight(local_x, local_z, height);
                }
            }

            return region;
        }
    }
}
