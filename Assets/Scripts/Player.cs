using UnityEngine;
using Game.Service;

public class Player : MonoBehaviour
{
    public float speed = 1.0F;

    public float areaSize = 10.0F;

    private Vector3 position;
    private Vector3 terrain_area;

    private SmallWorldTerrain terrain;
    

    private void Start()
    {
        terrain_area = new Vector3(areaSize, 5, areaSize);
        terrain = FindObjectOfType<SmallWorldTerrain>();
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (InArea(transform.position + direction * speed))
        {
            transform.position += direction * speed;
        }
        else
        {
            position += direction * speed;
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector3 point = hit.point;

            point.y = transform.position.y;

            transform.rotation = Quaternion.LookRotation(point - transform.position);
        }

        if(terrain != null)
        {
            terrain.UpdateTerrain(new Region(position, terrain_area));
        }
    }

    private bool InArea(Vector3 point)
    {
        return
            point.x >= -areaSize * 0.25F && point.x <= areaSize * 0.25F &&
            point.z >= -areaSize * 0.25F && point.z <= areaSize * 0.25F;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize, 0, areaSize) * 0.5F);
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(areaSize, 0, areaSize));
    }
}
