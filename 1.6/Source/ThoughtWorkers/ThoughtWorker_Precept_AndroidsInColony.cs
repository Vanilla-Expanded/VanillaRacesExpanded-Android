using RimWorld;
using System.Linq;
using Verse;
namespace VREAndroids
{
	public class ThoughtWorker_Precept_AndroidsInColony : ThoughtWorker_Precept
	{
		public override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.Faction == null || p.MapHeld is null)
			{
				return ThoughtState.Inactive;
			}

			int num = 0;
			foreach (Pawn item in p.MapHeld.mapPawns.SpawnedPawnsInFaction(p.Faction))
			{
				if (Utils.IsAndroid(item))
				{
					num++;
				}
			}

            foreach (var androidSleepMode in p.MapHeld.listerThings.ThingsOfDef(VREA_DefOf.VREA_AndroidSleepMode).OfType<Building_AndroidSleepMode>())
            {
                if (androidSleepMode.android.Faction == p.Faction)
                {
                    num++;
                }
            }

            if (num > 10)
            {
				num = 10;
            }
			return ThoughtState.ActiveAtStage(num);
		}
	}
}
