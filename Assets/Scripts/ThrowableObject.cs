using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private Transform playerCamera; // Reference to the player’s camera
    [SerializeField] private float holdDistance = 2f; // Distance in front of the player
    [SerializeField] private float throwForce = 10f; // Force applied when throwing
    [SerializeField] private float followSpeed = 10f; // Speed at which the object moves while carried

    private bool isBeingDragged = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Raycast infinitely and only pick up the first "ThrowableObject" in sight
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        bool canPickUp = Physics.Raycast(ray, out hit, Mathf.Infinity) &&
                         hit.collider.GetComponent<ThrowableObject>() != null &&
                         hit.collider.gameObject == gameObject;

        // Pick up object when looking at it and pressing E
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }

        // Drop or throw the object when holding it
        if (isBeingDragged)
        {
            if (Input.GetMouseButtonDown(0)) // Left Click → Drop
            {
                Drop();
            }
            else if (Input.GetMouseButtonDown(1)) // Right Click → Throw
            {
                Throw();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isBeingDragged)
        {
            Vector3 targetPosition = playerCamera.position + playerCamera.forward * holdDistance;
            rb.linearVelocity = (targetPosition - transform.position) * followSpeed; // Smooth movement with gravity
        }
    }

    private void PickUp()
    {
        isBeingDragged = true;
        rb.useGravity = true; // Allow gravity
        rb.linearDamping = 5f; // Add slight drag for smooth movement
    }

    private void Drop()
    {
        isBeingDragged = false;
        rb.useGravity = true;
        rb.linearDamping = 0f; // Reset drag
    }

    private void Throw()
    {
        isBeingDragged = false;
        rb.useGravity = true;
        rb.linearDamping = 0f;
        rb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
    }
}
