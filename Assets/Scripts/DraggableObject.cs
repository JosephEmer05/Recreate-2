using UnityEngine;
using TMPro;

public class DraggableObject : MonoBehaviour
{
    [SerializeField] private float pickupRange = 2f;
    [SerializeField] private Transform player;
    [SerializeField] private float holdDistance = 2f;

    [SerializeField] private TextMeshProUGUI pickUpText;
    [SerializeField] private TextMeshProUGUI dropText;

    private bool isBeingDragged = false;
    private Camera mainCamera;
    private Rigidbody rb;

    public bool IsBeingDragged => isBeingDragged;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();

        pickUpText.gameObject.SetActive(false);
        dropText.gameObject.SetActive(false);
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        bool canPickUp = distance <= pickupRange;

        if (!isBeingDragged && canPickUp)
        {
            pickUpText.gameObject.SetActive(true);
            dropText.gameObject.SetActive(false);
        }
        else if (isBeingDragged)
        {
            pickUpText.gameObject.SetActive(false);
            dropText.gameObject.SetActive(true);
        }
        else
        {
            pickUpText.gameObject.SetActive(false);
            dropText.gameObject.SetActive(false);
        }

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0)) && (canPickUp || isBeingDragged))
        {
            ToggleDrag();
        }

        if (isBeingDragged)
        {
            MoveWithMouse();
        }
    }

    private void ToggleDrag()
    {
        isBeingDragged = !isBeingDragged;

        if (isBeingDragged)
        {
            rb.isKinematic = true;
            dropText.gameObject.SetActive(true);
            pickUpText.gameObject.SetActive(false);
        }
        else
        {
            rb.isKinematic = false;
            dropText.gameObject.SetActive(false);
            pickUpText.gameObject.SetActive(false);
        }
    }

    private void MoveWithMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = holdDistance;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * 10f);
    }
}
