using Unity.Jobs;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private float playerSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private Rigidbody2D playerRB;
    public Vector3 playerPosition;
    private Vector2 playerVelocity;
    private float playerXVelocity;
    private bool playerExist = false;
    private bool playerSprinting = false;

    private InputSystem playerInput;

    private void Awake()
    {
        playerInput = new InputSystem();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += OnJump;
        playerInput.Player.Sprint.performed += OnSprintButton;
        playerInput.Player.Exist.performed += OnExist;
    }

    private void OnDisable()
    {
        playerInput.Player.Exist.performed -= OnExist;
        playerInput.Player.Sprint.performed -= OnSprintButton;
        playerInput.Player.Jump.performed -= OnJump;
        playerInput.Player.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = playerInput.Player.Move.ReadValue<Vector2>();
        playerXVelocity = playerVelocity.x;
        playerPosition = transform.position;

        //Debug.DrawRay(transform.position, Vector2.down * 1.2f, Color.red);
    }

    private void FixedUpdate()
    {
        if (playerExist)
        {
            playerRB.linearVelocityX = playerXVelocity * playerSpeed;
        }
        else
        {
            playerRB.linearVelocity = new Vector2(0,0);
        }  
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (onGround())
            playerRB.AddForceY(jumpStrength, ForceMode2D.Impulse);
    }

    private void OnSprintButton(InputAction.CallbackContext context)
    {
        if (playerSprinting)
        {
            playerSprinting = false;
            playerSpeed -= sprintSpeed;
        } 
        else
        {
            playerSprinting = true;
            playerSpeed += sprintSpeed;
        }
    }

    private void OnExist(InputAction.CallbackContext context)
    {
        if (playerExist)
        {
            playerExist = false;
            // Disappear
            playerRB.gravityScale = 0;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            playerExist = true;
            // Appear
            playerRB.gravityScale = 10;
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    private bool onGround()
    {
        return Physics2D.Raycast(playerPosition, Vector2.down, 1.2f, LayerMask.GetMask("Ground"));
    }

}
