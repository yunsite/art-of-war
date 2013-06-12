using System;

namespace AssemblyCSharp
{
	public class HeavyTank : Unit
	{
		private int toursToEndSpecial;
		private float defenceBeforeSpecial;
		private float movementRangeBeforeSpecial;
		public HeavyTank()
		{
			toursToEndSpecial = 0;
		}
		public override void UseSpecial ()
		{
			if(toursToEndSpecial == 0) //use special only if is not in use already
			{
				toursToEndSpecial = 2;
				defenceBeforeSpecial = HealthStatistics.Deffence;
				HealthStatistics.Deffence *= 2;
				movementRangeBeforeSpecial = MovementStatistics.TotalRange;
				MovementStatistics.TotalRange = 0.0f;
				MovementStatistics.RemainingRange = 0.0f;
			}
		}
		
		public override void EndTour()
		{
			if(toursToEndSpecial > 0)
				--toursToEndSpecial;
			if(toursToEndSpecial == 0)
			{
				HealthStatistics.Deffence = defenceBeforeSpecial;
				MovementStatistics.TotalRange = MovementStatistics.RemainingRange = movementRangeBeforeSpecial;
			}
		}
	}
}
