using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    #region class variables
    private Camera playerCamera;
    private Transform cameraTransform;
    private CharacterController controller;
    ///<summary> 
    ///this controls speed
    ///</summary>
    public float speed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 4f;
    [SerializeField]
    private Vector3 velocity;

    public float mouseSensitivity = 100f;

    float xRotation = 0f;
    #endregion

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //references
        playerCamera = GetComponentInChildren<Camera>();
        cameraTransform = playerCamera.transform;
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        MouseLook();

        PlayerMovement();
    }

/// <summary>
/// 
/// </summary>
/// <param name="x"></param>
/// <returns></returns>
    void PlayerMovement()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //movement
        Vector3 move = (transform.right * x) + (transform.forward * z);
        controller.Move(move * speed * Time.deltaTime);

        //jumping
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -1 * gravity);
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
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
}
