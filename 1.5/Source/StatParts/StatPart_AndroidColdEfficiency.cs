
using RimWorld;
using Verse;
namespace VREAndroids
{
    public class StatPart_AndroidColdEfficiency : StatPart
	{
        public override string ExplanationPart(StatRequest req)
        {
			if (req.Thing is Pawn pawn && pawn.HasActiveGene(VREA_DefOf.VREA_ColdEfficiency))
            {
                var temperature = pawn.AmbientTemperature;
                if (temperature <= -40)
                {
                    return "VREA.ColdEfficiencyBonusStatExplanation".Translate(temperature.ToStringTemperature()) + ": x" + 1.5f.ToStringPercent();
                }                                                                                               
                else if (temperature <= -20)                                                                    
                {                                                                                               
                    return "VREA.ColdEfficiencyBonusStatExplanation".Translate(temperature.ToStringTemperature()) + ": x" + 1.3f.ToStringPercent();
                }                                                                                               
                else if (temperature <= 0)                                                                      
                {                                                                                               
                    return "VREA.ColdEfficiencyBonusStatExplanation".Translate(temperature.ToStringTemperature()) + ": x" + 1.1f.ToStringPercent();
                }
            }
            return null;
        }

        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.Thing is Pawn pawn && pawn.HasActiveGene(VREA_DefOf.VREA_ColdEfficiency))
            {
                var temperature = pawn.AmbientTemperature;
                if (temperature <= -40)
                {
                    val *= 1.5f;
                }
                else if (temperature <= -20)
                {
                    val *= 1.3f;
                }
                else if (temperature <= 0)
                {
                    val *= 1.1f;
                }
            }
        }
    }
}
