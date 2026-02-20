using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class characterMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 1.0f;
    
    [SerializeField]
    private CharacterController controller;
    
    [SerializeField]
    private Vector3 moveDirection = Vector3.zero;

    [SerializeField] 
    private Transform cameraTransform;
    
    private float horizontalInput;
    private float verticalInput;

    public GameObject patientNotes;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (patientNotes.activeInHierarchy == false) 
        {
            getInputs();

            moveDirection = cameraTransform.forward.normalized * verticalInput + cameraTransform.right * horizontalInput;
            Vector3 moveDirectionButNoY = new Vector3(moveDirection.x, 0, moveDirection.z);

            controller.Move(moveDirectionButNoY.normalized * speed * Time.deltaTime);


        }
            
    }

    private void getInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }
}
