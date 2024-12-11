using System.Collections; // Provides access to collections like arrays and lists.
using System.Collections.Generic; // Provides access to generic collections like List<T>.
using UnityEngine; // Provides access to Unity-specific features like MonoBehaviour.
using UnityEngine.SceneManagement; // Provides access to scene management functionalities.

public class RestartGame : MonoBehaviour
{
    // Public method to restart the game by reloading the scene.
    public void Restart() 
    {
        // Reloads the scene with the name "King's_Room_L1".
        SceneManager.LoadScene("King's_Room_L1");
    }
}
