using System;
using UnityEngine;
using System.Collections.Generic;

public class Artillery : Unit
{
	private bool canUse = true;
    public void UseSpecial(Vector3 position)
    {
		if(canUse)
		{
		}
		Deselect();
		//SelectRange(SelectionMode.NoAction, 0.0f);
		OnActionCompleted();
    }   
	public override void EndTour ()
	{
		canUse = true;
	}
	public override void SelectSpecialAbility ()
	{
		if(canUse)
			SelectRange(SelectionMode.SpecialAbility, 0.0f);
		else
			SelectRange(SelectionMode.NoAction, 0.0f);
	}
}
