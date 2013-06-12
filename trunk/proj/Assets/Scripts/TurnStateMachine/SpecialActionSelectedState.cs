using UnityEngine;

/// <summary>
/// Unit area attack selection state of single player turn state machine.
/// Supports unit selected event for attack enemy capable units.
/// Supports all other events shipped with SelectedState.
/// </summary>
public class SpecialActionSelectedState : SelectedState
{
    internal SpecialActionSelectedState(InGameUI ui, Unit unit) : base(ui, unit) { }

    public override void Enter()
    {
        base.Enter();
        unit.UseSpecial();
    }
}
