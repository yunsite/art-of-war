using UnityEngine;

[ExecuteInEditMode]
public class MapManager : MonoBehaviour {

    public PlayerInfo[] Players;
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
