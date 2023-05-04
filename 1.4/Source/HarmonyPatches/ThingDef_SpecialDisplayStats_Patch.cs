using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(ThingDef), "SpecialDisplayStats")]
    public static class ThingDef_SpecialDisplayStats_Patch
    {
        public static IEnumerable<StatDrawEntry> Postfix(IEnumerable<StatDrawEntry> __result, StatRequest req)
        {
            if (req.Thing is Pawn pawn && pawn.IsAndroid())
            {
                foreach (var entry in __result)
                {
                    if (Pawn_SpecialDisplayStats_Patch.statLabelsPreventFromAndroids.Contains(entry.labelInt))
                    {
                        continue;
                    }
                    else
                    {
                        yield return entry;
                    }
                }
            }
            else
            {
                foreach (var entry in __result)
                {
                    yield return entry;
                } 
            }
        }
    }
}
