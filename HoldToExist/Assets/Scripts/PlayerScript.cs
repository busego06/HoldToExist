using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private float playerSpeed;
    [SerializeField] private float jumpStrength;
    [SerializeField] private Rigidbody2D playerRB;
    public Vector3 playerPosition;
    private Vector2 playerVelocity;
    private float playerXVelocity;

    private InputSystem playerInput;

    private void Awake()
    {
        playerInput = new InputSystem();
    }
    private void OnEnable()
    {
        playerInput.Player.Enable();
        playerInput.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
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
        playerRB.linearVelocityX = playerXVelocity * playerSpeed;
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (onGround())
            playerRB.AddForceY(jumpStrength, ForceMode2D.Impulse);
    }

    private bool onGround()
    {
        return Physics2D.Raycast(playerPosition, Vector2.down, 1.2f, LayerMask.GetMask("Ground"));
    }

}
