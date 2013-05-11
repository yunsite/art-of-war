using System;
using UnityEngine;

//wiem ze nie w mojej kwestii byla implementacja tego ale potrzebniwalem chociaz prototypu (Marek Kokot)
public abstract class GameState
{
	private GameController parent;

	public GameState SwitchState(GameStateEnum nextState)
	{
		throw new NotImplementedException();
	}
}


