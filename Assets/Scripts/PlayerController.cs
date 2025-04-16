using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 15f;
    public float rotationSpeed = 15f;

    [Header("Jump/Ground")]
    public float jumpForce = 7f;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float groundStickForce = 25f;

    private Rigidbody rb;
    private Vector3 currentVelocity;
    private bool isGrounded;







    public float currentDigTime;
    private bool isDigging;
    public float totalDigTime;
    private GameObject currentDigTarget;
    public float toolMultiplier = 1f;
    public float[] upgradeMultipliers = new float[3]; // For different upgrade types
    public int[] blockInventory = new int[6]; // For 6 tiers
    public int backpackCapacity = 20;








    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        SetupRigidbody();
    }

    private void SetupRigidbody()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.linearDamping = 5f;
        rb.angularDamping = 10f;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();

        if (isGrounded)
        {
            rb.AddForce(Vector3.down * groundStickForce, ForceMode.Acceleration);
        }
    }













    public float GetDigTimeForTier(int tier)
    {
        // Base times for each tier (adjust as needed)
        float[] baseTimes = { 1f, 2f, 4f, 8f, 16f, 32f };

        // Calculate total time with all multipliers
        return baseTimes[tier - 1] / (toolMultiplier * upgradeMultipliers[0]);
    }
    public void StartDigging()
    {
        isDigging = true;
        currentDigTime = 0f;

        // Get block tier from tag (e.g., "Tier1Block" -> tier 1)
        int tier = int.Parse(currentDigTarget.tag.Replace("Tier", "").Replace("Block", ""));
        totalDigTime = GetDigTimeForTier(tier);
    }

    public void ContinueDigging()
    {
        currentDigTime += Time.deltaTime;

        if (currentDigTime >= totalDigTime)
        {
            CompleteDigging();
        }
    }

    public void CompleteDigging()
    {
        int tier = int.Parse(currentDigTarget.tag.Replace("Tier", "").Replace("Block", ""));

        if (blockInventory[tier - 1] < backpackCapacity)
        {
            blockInventory[tier - 1]++;
            Destroy(currentDigTarget);
        }

        isDigging = false;
        currentDigTarget = null;
    }









    private void HandleMovement()
    {
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 cameraForward = Camera.main.transform.forward;
            Vector3 cameraRight = Camera.main.transform.right;
            cameraForward.y = 0f;
            cameraRight.y = 0f;
            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDirection = cameraForward * inputDirection.z + cameraRight * inputDirection.x;

            // Smooth acceleration
            currentVelocity = Vector3.Lerp(currentVelocity,
                moveDirection * moveSpeed,
                acceleration * Time.deltaTime);

            // Face movement direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                    targetRotation,
                    rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            // Smooth deceleration
            currentVelocity = Vector3.Lerp(currentVelocity,
                Vector3.zero,
                deceleration * Time.deltaTime);
        }

        currentVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = currentVelocity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}