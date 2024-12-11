using UnityEngine;

/// <summary>
/// Manages drag-and-drop functionality for objects, including snapping to slots and maintaining original positions.
/// </summary>
public class DragAndDrop : MonoBehaviour
{
    // Original local position relative to the parent object
    private Vector3 startLocalPosition;

    // Original scale of the object
    private Vector3 originalScale;

    // Original sorting order of the object's sprite
    private int originalSortingOrder;

    [Header("Scaling Settings")]
    [Tooltip("Scale factor when the object is being dragged.")]
    public float scaleFactor; // Assigned in the Inspector

    [Tooltip("Object scale when it snaps to the correct slot.")]
    public Vector3 slotScale; // Assigned in the Inspector

    [Header("Snapping Settings")]
    [Tooltip("Maximum distance allowed for snapping to the correct slot.")]
    public float snapDistance; // Assigned in the Inspector

    // Whether the object is currently in the correct slot
    private bool isInCorrectSlot = false;

    // Whether the object is currently being dragged
    private bool isDragging = false;

    // Whether the object is currently touching the correct slot
    private bool isTouchingSlot = false;

    [Header("Slot Settings")]
    [Tooltip("The correct slot to which this object should snap.")]
    public Transform correctSlot; // Assigned in the Inspector

    // Reference to the SpriteRenderer component
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Initializes the object's default settings, such as position, scale, and sorting order.
    /// </summary>
    void Start()
    {
        // Store the initial local position relative to the parent
        startLocalPosition = transform.localPosition;

        // Store the original scale as defined in the Inspector
        originalScale = transform.localScale;

        // Get the SpriteRenderer component and store its original sorting order
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSortingOrder = spriteRenderer.sortingOrder;

        Debug.Log($"Start Local Position: {startLocalPosition}");
    }

    /// <summary>
    /// Updates the position of the object while it is being dragged.
    /// </summary>
    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition;

            // Update position based on mouse or touch input
            if (Input.GetMouseButton(0))
            {
                newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                newPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
            else
            {
                return; // Do not update if there's no input
            }

            // Set the new position while maintaining a Z value of 0
            transform.position = new Vector3(newPosition.x, newPosition.y, 0);
        }
    }

    /// <summary>
    /// Called when the user clicks on the object to start dragging.
    /// </summary>
    private void OnMouseDown()
    {
        if (!isInCorrectSlot)
        {
            isDragging = true;

            // Scale up the object while dragging
            transform.localScale = originalScale * scaleFactor;

            // Increase sorting order so the object appears above others
            spriteRenderer.sortingOrder = 2;

            Debug.Log("Started dragging.");
        }
    }

    /// <summary>
    /// Called when the user releases the object to stop dragging.
    /// </summary>
    private void OnMouseUp()
    {
        if (!isInCorrectSlot)
        {
            isDragging = false;
            Debug.Log($"Released at Position: {transform.position}");

            // Check if the object is touching the correct slot and within snapping distance
            if (isTouchingSlot && Vector3.Distance(transform.position, correctSlot.position) < snapDistance)
            {
                transform.position = correctSlot.position; // Snap to the slot
                transform.localScale = slotScale; // Set the slot-specific scale
                isInCorrectSlot = true;

                Debug.Log($"Snapped to correct slot at Global Position: {correctSlot.position}");
            }
            else
            {
                // Reset to the original local position and scale
                transform.localPosition = startLocalPosition;
                transform.localScale = originalScale;

                Debug.Log($"Returned to start Local Position: {startLocalPosition}");
            }

            // Reset sorting order to the original value
            spriteRenderer.sortingOrder = originalSortingOrder;
        }
    }

    /// <summary>
    /// Called when the object enters a trigger collider.
    /// Updates the state if the object is touching the correct slot.
    /// </summary>
    /// <param name="other">The collider the object has entered.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform == correctSlot)
        {
            isTouchingSlot = true;
            Debug.Log("Entered correct slot.");
        }
    }

    /// <summary>
    /// Called when the object exits a trigger collider.
    /// Updates the state if the object leaves the correct slot.
    /// </summary>
    /// <param name="other">The collider the object has exited.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform == correctSlot)
        {
            isTouchingSlot = false;
            Debug.Log("Exited correct slot.");
        }
    }
}
