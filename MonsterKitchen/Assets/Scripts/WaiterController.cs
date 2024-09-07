using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class WaiterController : MonoBehaviour
{
    [Header("Movement")]
    private Rigidbody rb;
    private PlayerInput playerInput;
    [SerializeField] private float speedMovement;
    [SerializeField] private float rotationSpeed;

    [Header("Switch Player")]
    [SerializeField] private PlayerInput chefInput;
    [SerializeField] private ChefController chefController;
    [SerializeField] private PlayerInput waiterInput;
    [SerializeField] private WaiterController waiterController;
    private Animator anim;

    [SerializeField] private TMP_Text chancesOnScreen;

    private int chances = 5;

    private bool hasAPlate;

    public bool HasAPlate { get => hasAPlate; set => hasAPlate = value; }
    public int Chances { get => chances; set => chances = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        hasAPlate = false;
        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        chancesOnScreen.text = "Chances: " + chances.ToString();
        Vector2 input = playerInput.actions["Movement"].ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(input.x, 0, input.y);
        moveDir.Normalize();
        rb.MovePosition(rb.position + moveDir * speedMovement * Time.deltaTime);
        if (moveDir != Vector3.zero)
        {
            anim.SetBool("Walking", true);
            Quaternion rotation = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            anim.SetBool("Walking", false);
        }
        if (chances <= 0)
        {
            //Volver al menu
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
