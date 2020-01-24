using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float speed = 1.0F;
    public float jumpPower = 1.0F;
    public float gracity = 0.98F;

    private Vector3 gravity_velocity;
    private Vector3 move_velocity;

    private new Rigidbody rigidbody;

    private Vector3 destination;
    private bool is_moved;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gravity_velocity = Vector3.up * jumpPower;
        }
    }

    private void FixedUpdate()
    {
        //Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 direction = 
            transform.right * Input.GetAxis("Horizontal") +
            transform.forward * Input.GetAxis("Vertical");

        if (direction.magnitude > 0)
        {
            is_moved = false;
        }

        if(is_moved)
        {
            direction = (destination - transform.position).normalized;

            if(Vector3.Distance(destination, transform.position) < 0.25F)
            {
                is_moved = false;
            }
        }

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            Vector3 point = hit.point;

            point.y = transform.position.y;

            transform.rotation = Quaternion.LookRotation(point - transform.position);

            if (Input.GetMouseButton(0))
            {
                MoveTo(point);
            }
        }

        gravity_velocity -= Vector3.up * gracity;
        move_velocity = direction * speed;

        GetComponent<CharacterController>().Move(move_velocity + gravity_velocity);
    }

    private void MoveTo(Vector3 position)
    {
        destination = position;

        is_moved = true;
    }
}
