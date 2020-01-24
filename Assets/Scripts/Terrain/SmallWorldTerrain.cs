using Game.Service;
using Game.Terrain.Service;
using UnityEngine;

public class SmallWorldTerrain : MonoBehaviour
{
    public Material material;
    public SmallWorldTerrainSettings terrainSettings;

    #region UV data
    private UVMap uvmap;
    #endregion

    public void UpdateTerrain(Region region)
    {
        int childIndex = 0;

        float width = 32;
        float length = 32;

        InitializeTexture();

        foreach (Region sub_region in region.GetSubRegions(width, region.Height, length))
        {
            if (childIndex >= transform.childCount)
            {
                CreateChunk(sub_region);
            }

            var gameObject = transform.GetChild(childIndex++).gameObject;
        }
    }

    private void InitializeTexture()
    {
        if(uvmap != null)
        {
            return;
        }

        uvmap = new UVMap();

        Texture2D unwrap = new Texture2D(128, 128);

        if (material != null)
        {
            material.SetTexture("_BaseMap", unwrap);
        }

        AddSubTexture(0, 0, terrainSettings.rockTexture, unwrap);
        AddSubTexture(64, 0, terrainSettings.groundTexture, unwrap);
        AddSubTexture(64, 64, terrainSettings.grassTexture, unwrap);

        unwrap.filterMode = FilterMode.Point;

        unwrap.Apply();
    }

    private void AddSubTexture(int x, int y, Texture2D texture, Texture2D unwrap)
    {
        float uv_width = unwrap.width;
        float uv_height = unwrap.height;

        if (uvmap == null)
        {
            uvmap = new UVMap();
        }

        unwrap.SetPixels(x, y, texture.width, texture.height, texture.GetPixels());

        //TODO: Need fix
        float uv_x = (x + texture.width / 2) / uv_width;
        float uv_y = (y + texture.height / 2) / uv_height;
        float uv_z = 1 / uv_width;
        float uv_w = 1 / uv_height;

        //uv.x = x / uv_width;
        //uv.y = y / uv_height;
        //uv.z = texture.width / uv_width;
        //uv.w = texture.height / uv_height;

        uvmap.AddUVData(uv_x, uv_y, uv_z, uv_w);
    }

    private void CreateChunk(Region region)
    {
        GameObject chunk = new GameObject($"chunk {transform.childCount}");

        chunk.transform.parent = transform;
        chunk.transform.localPosition = new Vector3(region.X, region.Y, region.Z);

        CheckChunkComponents(chunk);

        chunk.GetComponent<Chunk>().InitializeChunk(region, uvmap);
    }

    private void CheckChunkComponents(GameObject gameObject)
    {
        if (!gameObject.GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();

        if (!gameObject.GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();

        if (!gameObject.GetComponent<Chunk>())
            gameObject.AddComponent<Chunk>();

        if (!gameObject.GetComponent<MeshCollider>())
            gameObject.AddComponent<MeshCollider>();

        gameObject.GetComponent<MeshRenderer>().material = material;
    }
}
