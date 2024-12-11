using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 startLocalPosition; // המיקום ההתחלתי היחסי להורה
    private Vector3 originalScale; // הגודל המקורי של הפריט
    private int originalSortingOrder; // הערך המקורי של הסדר בשכבה (Sorting Order)

    [Header("Scaling Settings")]
    public float scaleFactor = 1.9f; // גורם ההגדלה בזמן גרירה, ניתן לשינוי באינספקטור
    public Vector3 slotScale = new Vector3(130, 130, 1); // הגודל כאשר הפריט ננעל ב-Slot

    [Header("Snapping Settings")]
    public float snapDistance = 50f; // מרחק שמאפשר הצמדה לנקודה הנכונה
    private bool isInCorrectSlot = false; // האם הפריט נמצא במקום הנכון
    private bool isDragging = false; // האם הפריט כרגע נגרר
    private bool isTouchingSlot = false; // האם הפריט נוגע ב-Slot

    public Transform correctSlot; // הסלוט הנכון של הפריט

    private SpriteRenderer spriteRenderer; // רכיב ה-SpriteRenderer

    void Start()
    {
        // שמור את המיקום ההתחלתי היחסי להורה
        startLocalPosition = transform.localPosition;

        // שמור את הגודל שהוגדר ב-Inspector
        originalScale = transform.localScale;
        // קבל את ה-SpriteRenderer ושמור את ה-sortingOrder המקורי
        spriteRenderer = GetComponent<SpriteRenderer>(); //To get value of order layer
        originalSortingOrder = spriteRenderer.sortingOrder; //To get value of order layer

        Debug.Log($"Start Local Position: {startLocalPosition}"); // Debug למעקב
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPosition;
            if (Input.GetMouseButton(0)) // גרירה עם עכבר
            {
                newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Input.touchCount > 0) // גרירה עם מסך מגע
            {
                Touch touch = Input.GetTouch(0);
                newPosition = Camera.main.ScreenToWorldPoint(touch.position);
            }
            else
            {
                return; // אם אין לחיצה או מגע, אל תזיז את האובייקט
            }

            transform.position = new Vector3(newPosition.x, newPosition.y, 0); // עדכון מיקום, שמירה על Z = 0
        }
    }

    private void OnMouseDown()
    {
        if (!isInCorrectSlot)
        {
            isDragging = true; // התחלת גרירה
            transform.localScale = originalScale * scaleFactor; // הגדל את הפריט לפי ה-Scale Factor

            // העלה את ה-sortingOrder בזמן הגרירה
            spriteRenderer.sortingOrder = 2;

            Debug.Log("Started dragging."); // הודעה על תחילת הגרירה
        }
    }

    private void OnMouseUp()
    {
        if (!isInCorrectSlot)
        {
            isDragging = false; // סיום גרירה
            Debug.Log($"Released at Position: {transform.position}"); // הדפס את מיקום השחרור

            // בדוק אם הפריט נוגע ב-Slot ומרחקו מתאים
            if (isTouchingSlot && Vector3.Distance(transform.position, correctSlot.position) < snapDistance)
            {
                // הצמד למיקום הגלובלי של הסלוט
                transform.position = correctSlot.position;
                transform.localScale = slotScale; // שנה את הגודל ל-130x130x1
                isInCorrectSlot = true; // עדכון סטטוס
                Debug.Log($"Snapped to correct slot at Global Position: {correctSlot.position}"); // הודעה על נעילה ל-Slot
            }
            else
            {
                // החזר למיקום ההתחלתי היחסי להורה ושמור על הגודל המקורי
                transform.localPosition = startLocalPosition;
                transform.localScale = originalScale; // חזרה לגודל המקורי
                Debug.Log($"Returned to start Local Position: {startLocalPosition}"); // הדפס הודעה על חזרה למיקום ההתחלתי
            }
            //Get order layer back to OG layer
            spriteRenderer.sortingOrder = originalSortingOrder;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // בדוק אם הפריט נוגע ב-Slot הנכון
        if (other.transform == correctSlot)
        {
            isTouchingSlot = true; // עדכון סטטוס של נגיעה ב-Slot
            Debug.Log("Entered correct slot."); // הודעה על כניסה ל-Slot
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // אם הפריט יוצא מה-Slot
        if (other.transform == correctSlot)
        {
            isTouchingSlot = false; // עדכון סטטוס של יציאה מה-Slot
            Debug.Log("Exited correct slot."); // הודעה על יציאה מ-Slot
        }
    }
}
