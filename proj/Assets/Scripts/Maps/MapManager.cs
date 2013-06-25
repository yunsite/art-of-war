using UnityEngine;

/// <summary>
/// Represents single map gameplay management.
/// </summary>
[ExecuteInEditMode]
public class MapManager : MonoBehaviour {

    /// <summary>
    /// Includes info about all players in game.
    /// </summary>
    public PlayerInfo[] Players;

    /// <summary>
    /// References terrain observation utility.
    /// </summary>
    public PositionObserver Observer;

	void Awake () {
        if (Players == null || Players.Length == 0)
        {
            Players = GetComponentsInChildren<PlayerInfo>();
        }

        if (Observer == null)
        {
            Observer = GetComponentInChildren<PositionObserver>();
        }
	}
}
