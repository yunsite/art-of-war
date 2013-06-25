using UnityEngine;

/// <summary>
/// Singleton object class, representing root node, for accessing UI elements and game controller.
/// Game object with this script is attached is never removed from scene.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Root node of all UI elements.
    /// </summary>
	public GameUI GameUIInstance;

    /// <summary>
    /// Game controller, responsible for game state management.
    /// </summary>
	public GameController GameControllerInstance;

    /// <summary>
    /// Returns singleton game manager instance.
    /// </summary>
    /// <returns>Singleton game manager object.</returns>
	public static GameManager Instance()
	{
		return (GameManager)FindObjectOfType(typeof(GameManager));
	}

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
