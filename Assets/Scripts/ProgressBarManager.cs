using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the progress bar functionality in the game.
/// It updates the progress bar UI Image as progress increases.
/// </summary>
public class ProgressBarManager : MonoBehaviour
{
    [Header("Progress Tracking")]
    [Tooltip("Tracks the current progress in game according to the amount of items puts in correct place.")]
    public int currentProgress; // Tracks the current progress level by items puts in correct place.
   
    [Tooltip("Amount of items to be placed in correct slot.")]
    public int totalItems; // Total number of items required to complete the progress.

    [Header("Progress Bar Images")]
    [Tooltip("List of Progress Bar images to represent progress stages.")]
    public Sprite[] progressBarStages; // Array of Progress Bar images (0-100% stages).

    [Tooltip("UI Image component displaying the Progress Bar.")]
    public Image progressBarImage; // The Image component used to display progress.

    [Header("Audio Settings")]
    [Tooltip("Sound for progress updates.")]
    public AudioClip progressSound;
    [Tooltip("Sound for level completion.")]
    public AudioClip levelEndSound;

    private AudioSource audioSource;


    void Start()
    {
        // הוספת AudioSource למקרה שאין אותו
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Initializes the progress bar with a given number of items.
    /// </summary>
    /// <param name="itemsCount">The total number of draggable items.</param>
    public void InitializeProgressBar(int itemsCount)
    {
        // Debug to ensure this method runs correctly
        Debug.Log($"Initializing Progress Bar with {itemsCount} items.");
        UpdateProgressBar(); // Initial update to ensure the progress starts at 0
    }

    /// <summary>
    /// Adds progress when an item is successfully placed in the correct slot including for sounds part.
    /// </summary>
    public void AddProgress()
    {
        // Check if progress can be incremented
        if (currentProgress < totalItems )
        {
            currentProgress++;
            Debug.Log($"Progress Added: Current Progress = {currentProgress} / {totalItems}");

            UpdateProgressBar(); // Update the progress bar UI
                                 //play progress sound
            if (progressSound != null && currentProgress != totalItems)
            {
                audioSource.PlayOneShot(progressSound);
            }
        }
        else
        {
            
            Debug.LogWarning("Progress is already at maximum. Cannot add more progress.");
        }
        if (currentProgress == totalItems && levelEndSound != null)
            {
                audioSource.PlayOneShot(levelEndSound);
                Debug.Log("Level Complete!");
            }
        
    }

    /// <summary>
    /// Updates the progress bar image to reflect the current progress.
    /// </summary>
    private void UpdateProgressBar()
    {
        // Debug to ensure UpdateProgressBar runs with valid values
        Debug.Log($"Updating Progress Bar: Current Progress = {currentProgress}, Total Items = {totalItems}");

        // Clamp the progress to stay within bounds
        int progressIndex = Mathf.Clamp(currentProgress, 0, progressBarStages.Length );

        if (progressBarImage != null && progressBarStages.Length > 0)
        {
            progressBarImage.sprite = progressBarStages[progressIndex];
            Debug.Log($"Progress Bar Image Updated: Index = {progressIndex}, Sprite = {progressBarStages[progressIndex].name}");
        }
        else
        {
            Debug.LogError("ProgressBarImage or ProgressBarStages not properly assigned!");
        }
    }

    /// <summary>
    /// Debug helper to check the status of the progress bar.
    /// </summary>
    public void DebugProgressBarStatus()
    {
        Debug.Log($"[Debug Status] Current Progress: {currentProgress}, Total Items: {totalItems}");
        Debug.Log($"[Debug Status] Image Component: {(progressBarImage != null ? "Assigned" : "Not Assigned")}");
        Debug.Log($"[Debug Status] Stages Array Length: {progressBarStages.Length}");
    }
}
