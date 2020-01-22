using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(-5,5,-5);

    public float speed = 1.0F;

    private void LateUpdate()
    {
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position + offset, speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(target.position - transform.position);
        }
    }
}
