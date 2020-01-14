using Game.Service;
using Game.Terrain.Service;
using UnityEngine;

public class SmallWorldTerrain : MonoBehaviour
{
    private const int terrain_cubes_count = 50 * 50 * 50;

    public Material material;
    public SmallWorldTerrainSettings terrainSettings;

    #region Loaded data
    private LoadedTerrainRegion loadedTerrain;
    private LoadedWorldObjects loadedWorldObjects;
    #endregion

    #region UV data
    private Vector4[] blockUVs;
    #endregion

    #region Mesh data
    private int[] triangles = new int[terrain_cubes_count * 12];
    private Vector2[] uvs = new Vector2[terrain_cubes_count * 24];
    private Vector3[] normals = new Vector3[terrain_cubes_count * 24];
    private Vector3[] vertices = new Vector3[terrain_cubes_count * 24];
    #endregion

    public void UpdateTerrain(Region region)
    {
        int childIndex = 0;

        float width = region.Width * 0.5F;
        float length = region.Length * 0.5F;

        loadedWorldObjects.UpdateRegion(region);

        foreach (Region sub_region in region.GetSubRegions(width, region.Height, length))
        {
            if (childIndex >= transform.childCount)
            {
                CreateChunk(sub_region);
            }

            var gameObject = transform.GetChild(childIndex++).gameObject;

            CreatePolyMesh(sub_region, gameObject);
        }
    }

    private void Start()
    {
        InitializeLoader();
        InitializeNormals();
        InitializeTexture();
    }

    private void InitializeLoader()
    {
        loadedTerrain = new LoadedTerrainRegion();
        loadedWorldObjects = new LoadedWorldObjects();
    }

    private void InitializeNormals()
    {
        for (int i = 0; i < normals.Length; i += 4)
        {
            normals[i + 0] =
            normals[i + 1] =
            normals[i + 2] =
            normals[i + 3] = Vector3.up;
        }
    }

    private void InitializeTexture()
    {
        Texture2D unwrap = new Texture2D(128, 128);

        if (material != null)
        {
            material.SetTexture("_BaseMap", unwrap);
            //material.mainTexture = unwrap;
        }

        AddSubTexture(0, 0, terrainSettings.grassTexture, unwrap);
        AddSubTexture(64, 0, terrainSettings.groundTexture, unwrap);
        AddSubTexture(64, 64, terrainSettings.rockTexture, unwrap);

        unwrap.filterMode = FilterMode.Point;

        unwrap.Apply();
    }

    private void CreatePolyMesh(Region region, GameObject chunkGameObject)
    {
        int childIndex = 0;
        int vertices_count = 0;
        int triangles_count = 0;


        for (int x = 0; x < region.Width; x++)
        {
            for (int z = 0; z < region.Length; z++)
            {
                float Height = loadedTerrain.GetHeight(region.X + x, region.Z + z);
                float y = Height / 2.0F;

                Transform child = chunkGameObject.transform.GetChild(childIndex);

                child.localPosition = new Vector3(child.localPosition.x, y, child.localPosition.z);

                int b_type = (int)(3 - Height * 0.5F);

                Vector4 uv = blockUVs[2];

                if (b_type >= 0 && b_type < blockUVs.Length)
                {
                    uv = blockUVs[b_type];
                }

                #region UV
                uvs[vertices_count] =
                uvs[vertices_count + 4] =
                uvs[vertices_count + 8] =
                uvs[vertices_count + 12] =
                uvs[vertices_count + 16] = new Vector2(uv.x, uv.y);

                uvs[vertices_count + 1] =
                uvs[vertices_count + 5] =
                uvs[vertices_count + 9] =
                uvs[vertices_count + 13] =
                uvs[vertices_count + 17] = new Vector2(uv.x + uv.z, uv.y);

                uvs[vertices_count + 2] =
                uvs[vertices_count + 6] =
                uvs[vertices_count + 10] =
                uvs[vertices_count + 14] =
                uvs[vertices_count + 18] = new Vector2(uv.x + uv.z, uv.y + uv.w);

                uvs[vertices_count + 3] =
                uvs[vertices_count + 7] =
                uvs[vertices_count + 11] =
                uvs[vertices_count + 15] =
                uvs[vertices_count + 19] = new Vector2(uv.x, uv.y + uv.w);
                #endregion

                #region Verts
                Vector3 v0 = new Vector3(x - 0.5F, y + 0.5F, z - 0.5F);
                Vector3 v1 = new Vector3(x - 0.5F, y + 0.5F, z + 0.5F);
                Vector3 v2 = new Vector3(x + 0.5F, y + 0.5F, z + 0.5F);
                Vector3 v3 = new Vector3(x + 0.5F, y + 0.5F, z - 0.5F);
                Vector3 v4 = new Vector3(x + 0.5F, y - 0.5F, z + 0.5F);
                Vector3 v5 = new Vector3(x + 0.5F, y - 0.5F, z - 0.5F);
                Vector3 v6 = new Vector3(x - 0.5F, y - 0.5F, z + 0.5F);
                Vector3 v7 = new Vector3(x - 0.5F, y - 0.5F, z - 0.5F);

                vertices[vertices_count] = v0;
                vertices[vertices_count + 1] = v1;
                vertices[vertices_count + 2] = v2;
                vertices[vertices_count + 3] = v3;

                vertices[vertices_count + 4] = v4;
                vertices[vertices_count + 5] = v2;
                vertices[vertices_count + 6] = v3;
                vertices[vertices_count + 7] = v5;

                vertices[vertices_count + 8] = v6;
                vertices[vertices_count + 9] = v1;
                vertices[vertices_count + 10] = v0;
                vertices[vertices_count + 11] = v7;

                vertices[vertices_count + 12] = v6;
                vertices[vertices_count + 13] = v1;
                vertices[vertices_count + 14] = v2;
                vertices[vertices_count + 15] = v4;

                vertices[vertices_count + 16] = v7;
                vertices[vertices_count + 17] = v0;
                vertices[vertices_count + 18] = v3;
                vertices[vertices_count + 19] = v5;
                #endregion

                #region Tris
                #region up
                triangles[triangles_count + 0] = vertices_count;
                triangles[triangles_count + 1] = vertices_count + 1;
                triangles[triangles_count + 2] = vertices_count + 2;

                triangles[triangles_count + 3] = vertices_count;
                triangles[triangles_count + 4] = vertices_count + 2;
                triangles[triangles_count + 5] = vertices_count + 3;
                #endregion

                #region left
                triangles[triangles_count + 6] = vertices_count + 8;
                triangles[triangles_count + 7] = vertices_count + 9;
                triangles[triangles_count + 8] = vertices_count + 10;

                triangles[triangles_count + 9] = vertices_count + 8;
                triangles[triangles_count + 10] = vertices_count + 10;
                triangles[triangles_count + 11] = vertices_count + 11;
                #endregion

                #region front
                triangles[triangles_count + 12] = vertices_count + 15;
                triangles[triangles_count + 13] = vertices_count + 14;
                triangles[triangles_count + 14] = vertices_count + 13;

                triangles[triangles_count + 15] = vertices_count + 15;
                triangles[triangles_count + 16] = vertices_count + 13;
                triangles[triangles_count + 17] = vertices_count + 12;
                #endregion

                #region right
                triangles[triangles_count + 18] = vertices_count + 7;
                triangles[triangles_count + 19] = vertices_count + 6;
                triangles[triangles_count + 20] = vertices_count + 5;

                triangles[triangles_count + 21] = vertices_count + 7;
                triangles[triangles_count + 22] = vertices_count + 5;
                triangles[triangles_count + 23] = vertices_count + 4;
                #endregion

                #region back
                triangles[triangles_count + 24] = vertices_count + 16;
                triangles[triangles_count + 25] = vertices_count + 17;
                triangles[triangles_count + 26] = vertices_count + 18;

                triangles[triangles_count + 27] = vertices_count + 16;
                triangles[triangles_count + 28] = vertices_count + 18;
                triangles[triangles_count + 29] = vertices_count + 19;
                #endregion
                #endregion

                vertices_count += 20;
                triangles_count += 30;

                childIndex++;
            }
        }

        Mesh mesh = chunkGameObject.GetComponent<MeshFilter>().mesh;

        if (mesh == null)
        {
            mesh = new Mesh();
        }

        mesh.Clear();

        mesh.SetVertices(vertices, 0, vertices_count);
        mesh.SetNormals(normals, 0, vertices_count);
        mesh.SetUVs(0, uvs, 0, vertices_count);
        mesh.SetTriangles(triangles, 0, triangles_count, 0);
    }

    private void AddSubTexture(int x, int y, Texture2D texture, Texture2D unwrap)
    {
        float uv_width = unwrap.width;
        float uv_height = unwrap.height;

        if (blockUVs == null)
        {
            blockUVs = new Vector4[0];
        }

        unwrap.SetPixels(x, y, texture.width, texture.height, texture.GetPixels());

        var buffer = blockUVs;

        Vector4 uv = new Vector4();

        //TODO: Need fix
        uv.x = (x + texture.width / 2) / uv_width;
        uv.y = (y + texture.height / 2) / uv_height;
        uv.z = 1 / uv_width;
        uv.w = 1 / uv_height;

        //uv.x = x / uv_width;
        //uv.y = y / uv_height;
        //uv.z = texture.width / uv_width;
        //uv.w = texture.height / uv_height;

        blockUVs = new Vector4[buffer.Length + 1];
        blockUVs[buffer.Length] = uv;

        buffer.CopyTo(blockUVs, 0);
    }

    private void CreateChunk(Region region)
    {
        GameObject child = new GameObject($"child {transform.childCount}");

        child.transform.parent = transform;
        child.transform.localPosition = new Vector3(region.X, region.Y, region.Z);

        for (int x = 0; x < region.Width; x++)
            for (int z = 0; z < region.Length; z++)
            {
                GameObject collider = new GameObject($"collider");

                collider.AddComponent<BoxCollider>();

                collider.transform.parent = child.transform;
                collider.transform.localPosition = new Vector3(x, 0, z);
            }

        CheckChunkComponents(child);
    }

    private void CheckChunkComponents(GameObject gameObject)
    {
        if (!gameObject.GetComponent<MeshFilter>())
            gameObject.AddComponent<MeshFilter>();

        if (!gameObject.GetComponent<MeshRenderer>())
            gameObject.AddComponent<MeshRenderer>();

        gameObject.GetComponent<MeshRenderer>().material = material;
    }

}
