using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "XenotypesAvailableFor")]
    public static class PawnGenerator_XenotypesAvailableFor_Patch
    {
        public static bool preventAndroidGeneration;
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(PawnKindDef kind, FactionDef factionDef = null, Faction faction = null)
        {
            PawnGenerator.tmpXenotypeChances.RemoveAll(x => ShouldExcludeForAndroid(x.Key));
        }

        public static bool ShouldExcludeForAndroid(this XenotypeDef xenotypeDef)
        {
            if (xenotypeDef.IsAndroidType())
            {
                if (xenotypeDef.IsNonAwakeneedAndroidType())
                {
                    return true;
                }
                if (preventAndroidGeneration)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
