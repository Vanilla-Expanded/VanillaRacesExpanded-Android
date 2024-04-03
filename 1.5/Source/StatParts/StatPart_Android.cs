
using RimWorld;
using Verse;
namespace VREAndroids
{

    public class StatPart_Android : StatPart
	{
		private float factor = 1.2f;

		public override void TransformValue(StatRequest req, ref float val)
		{
			if (ActiveFor(req.Thing))
			{
				val *= factor;
			}
		}

		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && ActiveFor(req.Thing))
			{
				return "VREA.IsAndroidStat".Translate() + (": x" + factor.ToStringPercent());
			}
			return null;
		}

		private bool ActiveFor(Thing t)
		{
			Pawn pawn;
			if ((pawn = (t as Pawn)) != null)
			{
				return pawn.Ideo?.HasPrecept(VREA_DefOf.MechanoidLabor_Enhanced) == true && Utils.IsAndroid(pawn);
			}
			return false;
		}
	}
}
