using RimWorld;
using Verse;
namespace VREAndroids
{
	public class ThoughtWorker_Precept_AndroidPresent : ThoughtWorker_Precept
	{
		public override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || Utils.IsAndroid(p))
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
			return ThoughtState.Inactive;
		}
	}
}
