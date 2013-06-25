using UnityEngine;
using System.Collections.Generic;

public class HelicopterSpecialAttackSelectedState : SelectedState
{
    /// <summary>
    /// Selected helicopter unit.
    /// </summary>
	protected Helicopter helicopter;

	private List<Unit> unitsToAttack = new List<Unit>();

    /// <summary>
    /// Create helicopter special attack selection state.
    /// </summary>
    /// <param name="ui">UI reference.</param>
    /// <param name="player">Player info reference.</param>
    /// <param name="unit">Selected unit.</param>
    /// <exception cref="System.ArgumentNullException">Thrown when any parameter is null.</exception>
    /// <exception cref="System.InvalidCastException">Thrown when unit is not Helicopter.</exception>
    internal HelicopterSpecialAttackSelectedState(InGameUI ui, PlayerInfo player, Unit unit) : base(ui, player, unit)
	{
		helicopter = (Helicopter)unit;
	}

    /// <summary>
    /// State entry behaviour, called in case of in-transition occurrence.
    /// </summary>
    /// <remarks>
    /// Marks unit for special ability.
    /// </remarks>
    public override void Enter()
    {
        unit.SelectSpecialAbility();
    }

    #region Events
    /// <summary>
    /// Unit selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Selects unit for attack and triggers attack if four units selected.
    /// Return selected state if same player unit selected.
    /// </remarks>
    /// <param name="unit">Unit being selected.</param>
    /// <returns>New state of single player turn state machine.</returns>
    public override TurnState UnitSelected(Unit enemy)
    {			
		if(helicopter.CanUseSpecial())
		{
	        if (enemy.PlayerOwner != player.Index)
	        {
				enemy.SelectAsTargetForHelicopterSpecial();
				unitsToAttack.Add(enemy);
				if(unitsToAttack.Count == 4)
				{
					DeselectAllInList();
					helicopter.UseSpecial(unitsToAttack);
					return new SelectedState(ui, player, unit);
				}
				return this;
	        }
			DeselectAllInList();
			return new SelectedState(ui, player, enemy);
		}
        return this;
    }

    /// <summary>
    /// Unit special action selected event behaviour.
    /// </summary>
    /// <remarks>
    /// Triggers special attack when more than 2 units selected and returns selected state for current unit.
    /// Clears selected units otherwise and returns self state.
    /// </remarks>
    /// <returns>New state of single player turn state machine.</returns>
	public override TurnState SpecialActionSelected ()
	{
		DeselectAllInList();
		if(unitsToAttack.Count >= 2)
		{
			helicopter.UseSpecial(unitsToAttack);
			return new SelectedState(ui, player, unit);
		}
		unitsToAttack.Clear();
		return this;
	}
    #endregion
	
	private void DeselectAllInList()
	{
		foreach(Unit u in unitsToAttack)
			u.Deselect();
	}
}




