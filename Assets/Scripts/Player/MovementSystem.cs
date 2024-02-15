using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    public GameObject Character;
    public Transform CameraRoot;

    public float speed = 5f;
    public float accSpeed = 1f;
    public float jumpHeight = 10f;
    public float mouseSpeed = 2f;

    public bool onGround = false;

    private Rigidbody rb;

    private float headRotation;

    Vector3 velocity;
    public float acc;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        float rightSpeed = Input.GetAxis("Horizontal") * speed;
        float forwardSpeed = 0;

        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        
        if (onGround)
        {
            forwardSpeed = Input.GetAxis("Vertical") * speed;

            if (forwardSpeed == 0)
            {
                acc = Mathf.Lerp(acc, 0, Time.deltaTime * 5f);
                if(acc < 1f)
                    acc = 0;
            }

        }

        acc += moveDir.magnitude * accSpeed * Time.deltaTime;

        rb.velocity = transform.forward * forwardSpeed + transform.right * rightSpeed + Vector3.up * rb.velocity.y;
        rb.velocity += transform.forward * acc;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (onGround)
            {
                rb.velocity = new Vector3(rb.velocity.x, Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude), rb.velocity.z);
                onGround = false;
            }
        }

        CameraRotation();
    }

    void CameraRotation()
    {
        headRotation -= Input.GetAxis("Mouse Y") * mouseSpeed;
        headRotation = Mathf.Clamp(headRotation, -90, 90);

        transform.Rotate(0, Input.GetAxis("Mouse X") * mouseSpeed, 0);
        CameraRoot.localRotation = Quaternion.Euler(headRotation, 0, 0);
    }

    private void OnTriggerStay(Collider other)
    {
        onGround = true;
    }

    private void OnTriggerExit(Collider other)
    {
        onGround = false;
    }
}
