using RimWorld;
using System.Linq;
using Verse;
namespace VREAndroids
{
	public class ThoughtWorker_Precept_AndroidPresent : ThoughtWorker_Precept
	{
		public override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || Utils.IsAndroid(p) || p.MapHeld is null)
			{
				return ThoughtState.Inactive;
			}
			foreach (Pawn item in p.MapHeld.mapPawns.AllPawnsSpawned)
			{
				if (Utils.IsAndroid(item) && (item.IsPrisonerOfColony || item.IsSlaveOfColony || item.IsColonist))
				{
					return ThoughtState.ActiveDefault;
				}
			}

			foreach (var androidSleepMode in p.MapHeld.listerThings.ThingsOfDef(VREA_DefOf.VREA_AndroidSleepMode).OfType<Building_AndroidSleepMode>())
			{
				if (androidSleepMode.android.IsPrisonerOfColony || androidSleepMode.android.IsSlaveOfColony 
					|| androidSleepMode.android.IsColonist)
				{
                    return ThoughtState.ActiveDefault;
                }
            }

			return ThoughtState.Inactive;
		}
	}
}
