using RimWorld;
using Verse;
namespace VREAndroids
{
	public class ThoughtWorker_Precept_AndroidsInColony : ThoughtWorker_Precept
	{
		public override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive || p.Faction == null)
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
            if (num > 10)
            {
				num = 10;
            }
			return ThoughtState.ActiveAtStage(num);
		}
	}
}
