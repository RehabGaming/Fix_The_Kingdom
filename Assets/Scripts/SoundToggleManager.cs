using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the sound state in the game, allowing the user to toggle sound ON or OFF.
/// Saves the sound state persistently using PlayerPrefs.
/// Please note: 
/// - 1 represents "Sound ON".
/// - 0 represents "Sound OFF".
/// </summary>
public class SoundToggleManager : MonoBehaviour
{
    [Header("Sprites for Sound Button")]
    [Tooltip("Sprite displayed when the sound is ON.")]
    public Sprite soundOnSprite; // Sprite when sound is ON
   
    [Tooltip("Sprite displayed when the sound is OFF.")]
    public Sprite soundOffSprite; // Sprite when sound is OFF
   
    [Tooltip("The Image component of the sound toggle button.")]
    public Image soundButtonImage; // Image component to display the sprite

    /// <summary>
    /// Indicates whether the sound is ON or OFF.
    /// True if sound is ON, false if sound is OFF.
    /// *For the creation of this function i received help from Chat GPT
    /// </summary>
    public bool IsSoundOn { get; private set; } // Tracks if sound is ON/OFF

    /// <summary>
    /// Called when the script instance is loaded. Initializes the sound state
    /// by loading the saved preference (or defaulting to sound ON).
    /// </summary>
    private void Start()
    {
        // Load saved sound state
        // Defaults to 1 (sound ON) if no value is saved.
        IsSoundOn = PlayerPrefs.GetInt("SoundState", 1) == 1; // Default to sound ON (1)
        
        // Update the sprite and button and sound state.
        UpdateSoundState();
    }

    /// <summary>
    /// Toggles the sound state between ON and OFF when the user interacts with the sound button.
    /// Saves the updated state in PlayerPrefs.
    /// </summary>
    public void ToggleSound()
    {
        IsSoundOn = !IsSoundOn; // Toggle the sound state

        // Save the updated state to PlayerPrefs
        PlayerPrefs.SetInt("SoundState", IsSoundOn ? 1 : 0);
        PlayerPrefs.Save();

        // Update the button sprite and the sound state
        UpdateSoundState();
    }

    /// <summary>
    /// Updates the visual representation (button sprite) and the sound volume in the game.
    /// - Changes the button sprite to indicate the current sound state.
    /// - Adjusts the global sound volume using AudioListener.
    /// </summary>
    private void UpdateSoundState()
    {
        // Update button sprite
        soundButtonImage.sprite = IsSoundOn ? soundOnSprite : soundOffSprite;

        // Mute or unmute the sound
        AudioListener.volume = IsSoundOn ? 1.0f : 0.0f;
    }
}
