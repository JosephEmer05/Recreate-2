using UnityEngine;

public class ThrowableObject : MonoBehaviour
{
    [SerializeField] private Transform playerCamera;
    [SerializeField] private float holdDistance = 2f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float followSpeed = 10f;

    private bool isBeingDragged = false;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        bool canPickUp = Physics.Raycast(ray, out hit, Mathf.Infinity) &&
                         hit.collider.GetComponent<ThrowableObject>() != null &&
                         hit.collider.gameObject == gameObject;

        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }

        if (isBeingDragged)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Drop();
            }
            else if (Input.GetMouseButtonDown(1))
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
            rb.linearVelocity = (targetPosition - transform.position) * followSpeed;
        }
    }

    private void PickUp()
    {
        isBeingDragged = true;
        rb.useGravity = true;
        rb.linearDamping = 5f;
    }

    private void Drop()
    {
        isBeingDragged = false;
        rb.useGravity = true;
        rb.linearDamping = 0f;
    }

    private void Throw()
    {
        isBeingDragged = false;
        rb.useGravity = true;
        rb.linearDamping = 0f;
        rb.AddForce(playerCamera.forward * throwForce, ForceMode.Impulse);
    }
}
