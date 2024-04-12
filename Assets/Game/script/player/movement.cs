using UnityEngine;

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
    [SerializeField] public Animator animator;

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

        Debug.Log("if: " + (xMov != 0 || zMov != 0));
        if (xMov !=  0 || zMov != 0)
        {
            animator.SetBool("IsMoving", true);
        }
        else
        {
            animator.SetBool("IsMoving", false);
        }
    }
}