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

        AddSubTexture(0, 0, terrainSettings.rockTexture, unwrap);
        AddSubTexture(64, 0, terrainSettings.groundTexture, unwrap);
        AddSubTexture(64, 64, terrainSettings.grassTexture, unwrap);

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
                Transform child = chunkGameObject.transform.GetChild(childIndex);

                child.localPosition = new Vector3(child.localPosition.x, 0, child.localPosition.z);

                int b_type = (int)(6 - loadedTerrain.GetHeight(region.X + x, region.Z + z) * 2);

                Vector4 uv = blockUVs[1];

                Vector3 v0 = new Vector3(x, loadedTerrain.GetHeight(region.X + x, region.Z + z) / 2, z);
                Vector3 v1 = new Vector3(x + 1, loadedTerrain.GetHeight(region.X + x + 1, region.Z + z) / 2, z);
                Vector3 v2 = new Vector3(x + 1, loadedTerrain.GetHeight(region.X + x + 1, region.Z + z + 1) / 2, z + 1);
                Vector3 v3 = new Vector3(x, loadedTerrain.GetHeight(region.X + x, region.Z + z + 1) / 2, z + 1);

                if (b_type < 0)
                {
                    uv = blockUVs[0];
                }
                else if(b_type < blockUVs.Length)
                {
                    uv = blockUVs[blockUVs.Length - 1];
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

                #region Normals
                Vector3 normal0 = Vector3.Cross(v1 - v2, v3 - v2);

                normals[vertices_count + 0] = normal0;
                normals[vertices_count + 1] = normal0;
                normals[vertices_count + 2] = normal0;

                normals[vertices_count + 3] = normal0;
                normals[vertices_count + 4] = normal0;
                normals[vertices_count + 5] = normal0;
                #endregion

                #region Verts
                vertices[vertices_count + 5] = v0;
                vertices[vertices_count + 4] = v1;
                vertices[vertices_count + 3] = v3;

                vertices[vertices_count + 2] = v1;
                vertices[vertices_count + 1] = v2;
                vertices[vertices_count + 0] = v3;
                #endregion

                #region Tris
                triangles[triangles_count + 0] = vertices_count;
                triangles[triangles_count + 1] = vertices_count + 1;
                triangles[triangles_count + 2] = vertices_count + 2;

                triangles[triangles_count + 3] = vertices_count + 3;
                triangles[triangles_count + 4] = vertices_count + 4;
                triangles[triangles_count + 5] = vertices_count + 5;
                #endregion

                vertices_count += 6;
                triangles_count += 6;

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
