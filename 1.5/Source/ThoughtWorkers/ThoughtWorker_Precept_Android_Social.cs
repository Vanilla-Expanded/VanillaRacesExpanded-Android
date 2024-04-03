using RimWorld;
using Verse;
namespace VREAndroids
{
	public class ThoughtWorker_Precept_Android_Social : ThoughtWorker_Precept_Social
	{
		public override ThoughtState ShouldHaveThought(Pawn p, Pawn otherPawn)
		{
			if (!ModsConfig.BiotechActive || !ModsConfig.IdeologyActive)
			{
				return ThoughtState.Inactive;
			}
			return Utils.IsAndroid(otherPawn);
		}




	}
}
