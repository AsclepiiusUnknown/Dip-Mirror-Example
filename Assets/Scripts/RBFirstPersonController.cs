using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RBFirstPersonController : MonoBehaviour
{
    #region class variables
    private Camera playerCamera;
    private Transform cameraTransform;
    private Rigidbody rigidbodyController;

    public Vector3 inputVector;
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;
    [SerializeField]
    private Vector3 velocity;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;

    float distanceToGround;
    #endregion


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //references
        playerCamera = GetComponentInChildren<Camera>();
        cameraTransform = playerCamera.transform;
        rigidbodyController = GetComponent<Rigidbody>();

        Collider collider = GetComponent<Collider>();
        distanceToGround = collider.bounds.extents.y;
    }

    void Update()
    {
        MouseLook();
        PlayerMovement();
    }
    void FixedUpdate()
    {
        rigidbodyController.velocity = inputVector;
    }

    void PlayerMovement()
    {
        //input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 velocity = new Vector3(0, rigidbodyController.velocity.y, 0);
        
        //jumping
        if(Input.GetButtonDown("Jump") && isGrounded())
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -1 * gravity);
        }

        inputVector = (transform.right * x * speed) + velocity + (transform.forward * z * speed);
    }    

    void MouseLook()
    {
        //input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //camera rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        //character rotation
        transform.Rotate(Vector3.up * mouseX);
    }


    bool isGrounded()
    {
        //Debug.DrawRay(transform.position, -Vector3.up * (distanceToGround + 0.1f), Color.red);
        return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
    }    
}
