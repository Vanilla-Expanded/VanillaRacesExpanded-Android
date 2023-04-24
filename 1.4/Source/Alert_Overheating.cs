using RimWorld;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace VREAndroids
{
    public class Alert_Overheating : Alert
    {
        private List<Pawn> heatstrokePawnsResult = new List<Pawn>();

        private List<Pawn> HeatstrokePawns
        {
            get
            {
                heatstrokePawnsResult.Clear();
                List<Pawn> list = PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer);
                for (int i = 0; i < list.Count; i++)
                {
                    Pawn pawn = list[i];
                    if (pawn.health != null && !pawn.RaceProps.Animal && pawn.health.hediffSet.GetFirstHediffOfDef(VREA_DefOf.VREA_Overheating, mustBeVisible: true) != null)
                    {
                        heatstrokePawnsResult.Add(pawn);
                    }
                }
                return heatstrokePawnsResult;
            }
        }

        public Alert_Overheating()
        {
            defaultLabel = "VREA.Overheating".Translate();
            defaultPriority = AlertPriority.High;
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Pawn heatstrokePawn in HeatstrokePawns)
            {
                stringBuilder.AppendLine("  - " + heatstrokePawn.NameShortColored.Resolve());
            }
            return string.Format("VREA.AlertOverheatingDesc".Translate(), stringBuilder.ToString());
        }

        public override AlertReport GetReport()
        {
            return AlertReport.CulpritsAre(HeatstrokePawns);
        }
    }
}
