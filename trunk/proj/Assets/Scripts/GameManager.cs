using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	//private HighScore highScoreInstance;
	
	public GameUI GameUIInstance;
	public GameController GameControllerInstance;
	
	void Awake () {
		// Obiekt do którego przyłączony jest GameManager powinien przetrwać przełądowanie sceny.
		DontDestroyOnLoad(transform.gameObject);
	}
	
	private void Initialize()
	{
		//new GameState();
	}
	
	public static GameManager Instance()
	{
		return (GameManager)FindObjectOfType(typeof(GameManager));
	}
}
