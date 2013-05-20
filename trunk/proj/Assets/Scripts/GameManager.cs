using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public class GameManager : MonoBehaviour
	{
		//private HighScore highScoreInstance;
		
		private void Initialize()
		{
			new GameState();
		}
		
		public GameManager Instance()
		{
			throw new NotImplementedException();
		}
	}
}

