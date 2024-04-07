using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Camera cam;

    public float movementSpeed;
    public float sensitivity;
    public float powerJump;
    public playerFeet feet;
    private Vector3 rotation;
    void Start()
    {
        Debug.Log("Hello world");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = new(0, GetComponent<Rigidbody>().velocity.y, 0);
        if (Input.GetKey("z"))
        {
            currentVelocity.z += movementSpeed;
            //GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + currentVelocity * Time.fixedDeltaTime);
        }
        if (Input.GetKey("s"))
        {
            currentVelocity.z += -movementSpeed;
        }
        if (Input.GetKey("q"))
        {
            currentVelocity.x += -movementSpeed;
        }
        if (Input.GetKey("d"))
        {
            currentVelocity.x += movementSpeed;
        }

        currentVelocity = transform.rotation * currentVelocity;
        GetComponent<Rigidbody>().velocity = currentVelocity;

        if (Input.GetKeyDown(KeyCode.Space) && feet.isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * powerJump);
        }


        float yRot = Input.GetAxisRaw("Mouse X");
        rotation = new Vector3(0, yRot, 0) * sensitivity;
        GetComponent<Rigidbody>().MoveRotation(GetComponent<Rigidbody>().rotation * Quaternion.Euler(rotation));


        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * sensitivity; 
        if ((cam.transform.eulerAngles.x - cameraRotation.x) <= 90f || (cam.transform.eulerAngles.x - cameraRotation.x) >= 270f)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }
}
