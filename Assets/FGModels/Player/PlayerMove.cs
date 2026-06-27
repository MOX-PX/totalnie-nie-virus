using UnityEngine;
using UnityEngine.UI;
public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float shiftSpeed = 10f;
    [SerializeField] private float jumpSpeed = 5f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float staminaDrain = 20f; // ile staminy znika na sekundê
    [SerializeField] private float staminaRecovery = 15f; // ile staminy wraca na sekundê

    [Header("UI")]
    [SerializeField] private Slider staminaSlider;

    private float currentSpeed;
    private float currentStamina;

    private Rigidbody rb;
    private Animator anim;

    private Vector3 direction;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        currentSpeed = movementSpeed;

        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        direction = transform.TransformDirection(x, 0, z);

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
        }

        HandleSprint();
        UpdateStaminaUI();
    }

    private void HandleSprint()
    {
        bool isMoving = direction.magnitude > 0.1f;
        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);

        if (wantsToSprint && isMoving && currentStamina > 0)
        {
            currentSpeed = shiftSpeed;

            currentStamina -= staminaDrain * Time.deltaTime;
            currentStamina = Mathf.Max(currentStamina, 0);
        }
        else
        {
            currentSpeed = movementSpeed;

            currentStamina += staminaRecovery * Time.deltaTime;
            currentStamina = Mathf.Min(currentStamina, maxStamina);
        }
    }

    private void UpdateStaminaUI()
    {
        if (staminaSlider != null)
        {
            staminaSlider.value = currentStamina;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * currentSpeed * Time.fixedDeltaTime);
    }

    private void OnCollisionStay(Collision other)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(Collision other)
    {
        isGrounded = false;
    }
}