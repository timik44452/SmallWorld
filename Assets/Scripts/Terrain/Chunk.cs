using Game.Service;
using Game.Terrain.Service;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    #region Loaded data
    private LoadedTerrainRegion loadedTerrain;
    private LoadedWorldObjects loadedWorldObjects;
    #endregion

    #region UV data
    private UVMap uvmap;
    #endregion

    #region Noize
    private Vector3[,] points = new Vector3[0, 0];
    #endregion

    public void InitializeChunk(Region region, UVMap uvmap)
    {
        this.uvmap = uvmap;

        InitializeLoader();
        UpdateChunk(region);
    }

    private void InitializeLoader()
    {
        loadedTerrain = new LoadedTerrainRegion();
        loadedWorldObjects = new LoadedWorldObjects();
    }

    private void UpdateChunk(Region region)
    {
        int region_width = (int)region.Width + 1;
        int region_length = (int)region.Length + 1;

        if (points.GetLength(0) < region.Width || points.GetLength(1) < region.Length)
        {
            points = new Vector3[region_width, region_length];
        }

        for (int x = 0; x < region_width; x++)
            for (int z = 0; z < region_length; z++)
            {
                float height = loadedTerrain.GetHeight(region.X + x, region.Z + z);

                float dx = Mathf.Sin(height * Mathf.PI * 2);
                float dz = Mathf.Cos(height * Mathf.PI * 2);

                points[x, z] = new Vector3(x + dx, height / 2, z + dz);
            }

        loadedWorldObjects.UpdateRegion(region);

        CreatePolyMesh(region, gameObject);
    }

    private void CreatePolyMesh(Region region, GameObject chunkGameObject)
    {
        int childIndex = 0;
        int vertices_count = 0;
        int triangles_count = 0;

        int resolution = (int)(region.Width * region.Length + 0.5F);

        #region Mesh data
        int[] triangles = new int[resolution * 6];
        Vector2[] uvs = new Vector2[resolution * 6];
        Vector3[] normals = new Vector3[resolution * 6];
        Vector3[] vertices = new Vector3[resolution * 6];
        #endregion

        for (int x = 0; x < region.Width; x++)
        {
            for (int z = 0; z < region.Length; z++)
            {
                int b_type = (int)(6 - loadedTerrain.GetHeight(region.X + x, region.Z + z) * 2);

                Vector3 v3 = points[x, z];
                Vector3 v2 = points[x, z + 1];
                Vector3 v1 = points[x + 1, z + 1];
                Vector3 v0 = points[x + 1, z];

                UVData uv = uvmap.GetUV(b_type);

                #region UV
                uvs[vertices_count] = new Vector2(uv.x, uv.y);
                uvs[vertices_count + 1] = new Vector2(uv.x + uv.width, uv.y);
                uvs[vertices_count + 2] = new Vector2(uv.x + uv.width, uv.y + uv.height);

                uvs[vertices_count + 3] = new Vector2(uv.x, uv.y);
                uvs[vertices_count + 4] = new Vector2(uv.x + uv.width, uv.y + uv.height);
                uvs[vertices_count + 5] = new Vector2(uv.x, uv.y + uv.height);
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
}
