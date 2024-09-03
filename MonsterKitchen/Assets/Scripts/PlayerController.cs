using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private float speedMovement;
    [SerializeField] private float rotationSpeed;

    [Header("Switch Player")]
    [SerializeField] private PlayerInput chefInput;
    [SerializeField] private PlayerController chefController;
    [SerializeField] private PlayerInput waiterInput;
    [SerializeField] private PlayerController waiterController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        moveDir.Normalize();
        rb.MovePosition(rb.position + moveDir * speedMovement * Time.deltaTime); 
        if(moveDir != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void SwitchPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (chefController.isActiveAndEnabled)
            {
                chefController.enabled = false;
                chefInput.enabled = false;
                waiterController.enabled = true;
                waiterInput.enabled = true;
            }
            else
            {
                waiterController.enabled = false;
                waiterInput.enabled = false;
                chefController.enabled = true;
                chefInput.enabled = true;
            }
        }
    }


}
