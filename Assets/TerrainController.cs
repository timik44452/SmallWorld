using UnityEngine;
using System.Collections;

public class TerrainController : MonoBehaviour
{
    public Vector3 area = Vector3.one;

    private SmallWorldTerrain terrain;
    private Player player;

    private void Start()
    {
        terrain = GetComponent<SmallWorldTerrain>();
        player = FindObjectOfType<Player>();

        terrain.UpdateTerrain(new Game.Service.Region(Vector3.zero, area));
    }
}
