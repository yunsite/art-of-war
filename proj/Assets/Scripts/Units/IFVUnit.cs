using System;
using UnityEngine;

/// <summary>
/// Represents IFV type of unit.
/// </summary>
public class IFVUnit : Unit
{
    private int toursToEndSpecial;
    private float movementRangeBeforeSpecial;

    /// <summary>
    /// Creates IFV unit.
    /// </summary>
    public IFVUnit()
    {
        toursToEndSpecial = 0;
    }
	/// <summary>
	/// Uses the special ability which increases movement ability.
	/// </summary>
    public override void UseSpecial()
    {
        if (toursToEndSpecial == 0) //use special only if is not in use already
        {
            toursToEndSpecial = 1;
            movementRangeBeforeSpecial = MovementStatistics.RemainingRange;
            MovementStatistics.RemainingRange *= 2;            
        }
    }

    /// <summary>
    /// Resets per turn statistics.
    /// </summary>
    public override void EndTurn()
    {
        base.EndTurn();
        if (toursToEndSpecial > 0)
            --toursToEndSpecial;
        if (toursToEndSpecial == 0)
        {
            //HealthStatistics.Deffence = defenceBeforeSpecial;
            //MovementStatistics.TotalRange = MovementStatistics.RemainingRange = movementRangeBeforeSpecial;
        }
    }
    
}
