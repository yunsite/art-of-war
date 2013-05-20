using System;

namespace AssemblyCSharp
{
	public class PlaceUnitState : GameState
	{
		private void SelectUnit(Unit unit)
		{
			unit.OnClick();
		}
		
		//co się dzieje po zakończeniu rundy?
		private void EndTurnButtonClickedHandler(Object sender, EventArgs args)
		{
			throw new NotImplementedException();			
		}
		
		private void DeselectUnitButtonClickedHandler(Object sender, EventArgs args)
		{
			DeselectCurrnetUnit();		
		}
		
		//nie wiem jak to ma działać
		private void DeselectCurrnetUnit()
		{
			throw new NotImplementedException();			
		}
		
		//nie wiem jak to ma działać
		private void CreateUnitOnPosition(UnityEngine.Vector3 worldPosition, UnityEngine.Object prefab)
		{
			throw new NotImplementedException();
		}
		
		private void RemoveUnit(Unit unit)
		{
			unit.Die();			
		}
		
		private void BWPButtonClickedHandler(Object sender, EventArgs args)
		{
			GameObject unit = (GameObject)Instantiate(Resources.Load("Prefab_BWPUnit"));
			CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), (Object) Unit);
		}
		
		private void TankButtonClickedHandler(Object sender, EventArgs args)
		{
			GameObject unit = (GameObject)Instantiate(Resources.Load("Prefab_TankUnit"));
			CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), (Object) Unit);
		}
		
		private void HeliButtonClickedHandler(Object sender, EventArgs args)
		{
			GameObject unit = (GameObject)Instantiate(Resources.Load("Prefab_HeliUnit"));
			CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), (Object) Unit);
		}
		
		private void HeavyTankButtonClickedHandler(Object sender, EventArgs args)
		{
			GameObject unit = (GameObject)Instantiate(Resources.Load("Prefab_HeavyTankUnit"));
			CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), (Object) Unit);
		}
		
		private void AltyleryButtonClickedHandler(Object sender, EventArgs args)
		{
			GameObject unit = (GameObject)Instantiate(Resources.Load("Prefab_AltyleryUnit"));
			CreateUnitOnPosition(new UnityEngine.Vector3(0,0,0), (Object) Unit);
		}
		
		//nie wiem jak to ma działać
		private void UpdatePoints(Object sender, EventArgs args)
		{
			throw new NotImplementedException();			
		}
		
		private void ContinueButtonClickedHandler(Object sender, EventArgs args)
		{
			GameState = GameController().GoToInGameState();			
		}
	}
}

