using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
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

    [Header("Animation")]
    [SerializeField] private Animator animator;


    Rigidbody rb;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    void Update()
    {
        

        //movement
        Vector3 currentVelocity = new(0, 0, 0);
        if (Input.GetKey("z"))
        {
            currentVelocity.z += movementSpeed;
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

        *//*currentVelocity = transform.rotation * currentVelocity;
        GetComponent<Rigidbody>().velocity = currentVelocity;*//*
        
        transform.position += new Vector3(currentVelocity.x * transform.forward.x, 0, currentVelocity.z * transform.forward.z) * Time.deltaTime;


        // jump 
        if (Input.GetKeyDown(KeyCode.Space) && feet.isGrounded)
        {
            GetComponent<Rigidbody>().AddForce(Vector3.up * powerJump);
        }



        if(currentVelocity.x != 0 || currentVelocity.y != 0) 
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }

        float yRot = Input.GetAxisRaw("Mouse X");
        rotation = new Vector3(0, yRot, 0) * sensitivity;
        cam.transform.Rotate(rotation);


        float xRot = Input.GetAxisRaw("Mouse Y");
        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * sensitivity; 
        if ((cam.transform.eulerAngles.x - cameraRotation.x) <= 90f || (cam.transform.eulerAngles.x - cameraRotation.x) >= 270f)
        {
            cam.transform.Rotate(-cameraRotation);
        }
    }
}
*/

public class Movement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private float speed;

    [SerializeField]
    private float sensivity;

    [SerializeField]
    private float jumpForce = 100f;

    [Header("Utilities")]
    [SerializeField]
    private playerFeet feet;

    private PlayerMotor motor;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
    }

    private void Update()
    {
        // Player movement
        float xMov = Input.GetAxisRaw("Horizontal");
        float zMov = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * speed;

        motor.Move(velocity);


        // mMouse movement horizontal
        float yRot = Input.GetAxisRaw("Mouse X");

        Vector3 rotation = new Vector3(0, yRot, 0) * sensivity;

        motor.Rotate(rotation);


        // Mouse movement vertical 
        float xRot = Input.GetAxisRaw("Mouse Y");

        Vector3 cameraRotation = new Vector3(xRot, 0, 0) * sensivity;

        motor.CameraRotate(cameraRotation);

        // Jump 
        if (Input.GetKeyDown(KeyCode.Space) && feet.isGrounded)
            GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce * 100);
        // Animation
        /*if (  != 0 || currentVelocity.y != 0)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }*/

    }
}


