using RimWorld;
using Verse;
namespace VREAndroids
{
	public class ThoughtWorker_Precept_Android : ThoughtWorker_Precept
	{
		public override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return Utils.IsAndroid(p);
		}
	}
}
