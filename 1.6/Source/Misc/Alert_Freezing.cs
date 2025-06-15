using RimWorld;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace VREAndroids
{
    public class Alert_Freezing : Alert_Critical
    {
        private List<Pawn> hypothermiaDangerColonistsResult = new List<Pawn>();
        private List<Pawn> HypothermiaDangerColonists
        {
            get
            {
                hypothermiaDangerColonistsResult.Clear();
                foreach (Pawn item in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoCryptosleep)
                {
                    if (!item.SafeTemperatureRange().Includes(item.AmbientTemperature))
                    {
                        Hediff firstHediffOfDef = item.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_Freezing);
                        if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex >= 3)
                        {
                            hypothermiaDangerColonistsResult.Add(item);
                        }
                    }
                }
                return hypothermiaDangerColonistsResult;
            }
        }

        public Alert_Freezing()
        {
            defaultLabel = "VREA.AlertFreezing".Translate();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Pawn hypothermiaDangerColonist in HypothermiaDangerColonists)
            {
                stringBuilder.AppendLine("  - " + hypothermiaDangerColonist.NameShortColored.Resolve());
            }
            return "VREA.AlertFreezingDesc".Translate(stringBuilder.ToString());
        }

        public override AlertReport GetReport()
        {
            return AlertReport.CulpritsAre(HypothermiaDangerColonists);
        }
    }
}
