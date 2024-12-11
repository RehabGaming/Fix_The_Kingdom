using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreviewManager : MonoBehaviour
{
    public Button previewButton; // הכפתור שמציג את התמונה
    public Image previewImage; // התמונה לתצוגה מקדימה

    private bool isPreviewVisible = false; // מצב התצוגה המקדימה

    void Start()
    {
        // ודא שהתמונה מוסתרת כברירת מחדל
        previewImage.color = new Color(previewImage.color.r, previewImage.color.g, previewImage.color.b, 0);

        // הוסף מאזין לכפתור
        // previewButton.onClick.AddListener(TogglePreview);

        Debug.Log("PreviewManager initialized: Preview image hidden by default. isPreviewVisible = " + isPreviewVisible);
    }

    void Update()
    {
        // אם התמונה מוצגת והמשתמש לוחץ בכל מקום במסך
        if (isPreviewVisible && Input.GetMouseButtonDown(0))
        {
            // בדיקה אם הלחיצה לא התבצעה על הכפתור הספציפי
            if (!IsPointerOverSpecificButton(previewButton))
            {
                Debug.Log("Screen clicked: Hiding preview image. isPreviewVisible = " + isPreviewVisible);
                HidePreview();
            }
        }

    }

    private bool IsPointerOverSpecificButton(Button button)
    {
        // בדוק אם העכבר נמצא מעל רכיב ה-Button הספציפי
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // השג את האובייקט עליו הלחיצה בוצעה
            GameObject currentObject = EventSystem.current.currentSelectedGameObject;
            if (currentObject != null && currentObject == button.gameObject)
            {
                return true; // הלחיצה על הכפתור
            }
        }
        return false; // הלחיצה לא על הכפתור
    }

    public void TogglePreview()
    {
        if (isPreviewVisible)
        {
            Debug.Log("Button clicked: Hiding preview image.");
            HidePreview(); // אם התמונה כבר מוצגת, הסתר אותה
        }
        else
        {
            Debug.Log("Button clicked: Showing preview image.");
            ShowPreview(); // אם התמונה מוסתרת, הצג אותה
        }
    }

    public void ShowPreview()
    {
        // הצג את התמונה במרכז המסך עם שקיפות מלאה
        previewImage.color = new Color(previewImage.color.r, previewImage.color.g, previewImage.color.b, 1f);
        isPreviewVisible = true;

        Debug.Log("Preview image shown: Fully visible. isPreviewVisible = " + isPreviewVisible);
        Debug.Log("Color set to: " + previewImage.color);
    }

    void HidePreview()
    {
        // הסתר את התמונה
        previewImage.color = new Color(previewImage.color.r, previewImage.color.g, previewImage.color.b, 0);
        isPreviewVisible = false;

        Debug.Log("Preview image hidden: Fully transparent. isPreviewVisible = " + isPreviewVisible);
    }
}

