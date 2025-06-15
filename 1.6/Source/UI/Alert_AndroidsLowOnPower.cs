using RimWorld;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    public class Alert_AndroidsLowOnPower : Alert_Critical
    {
        private static List<Pawn> androidsWithLowPower = new List<Pawn>();
        private List<Pawn> Culprits
        {
            get
            {
                androidsWithLowPower.Clear();
                foreach (Pawn item in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists_NoSuspended)
                {
                    if (item.IsAndroid() && item.needs.TryGetNeed(VREA_DefOf.VREA_ReactorPower).CurLevelPercentage < 0.2f)
                    {
                        androidsWithLowPower.Add(item);
                    }
                }
                return androidsWithLowPower;
            }
        }

        public override string GetLabel()
        {
            return "VREA.AndroidsLowOnPower".Translate();
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (Culprits.Any())
            {
                StringBuilder stringBuilder2 = new StringBuilder();
                foreach (Pawn item in Culprits)
                {
                    stringBuilder2.AppendLine("  - " + item.NameShortColored.Resolve());
                }
                stringBuilder.Append("VREA.AndroidsLowOnPowerDesc".Translate(stringBuilder2).Resolve());
            }
            return stringBuilder.ToString();
        }

        public override AlertReport GetReport()
        {
            return AlertReport.CulpritsAre(Culprits);
        }
    }
}
