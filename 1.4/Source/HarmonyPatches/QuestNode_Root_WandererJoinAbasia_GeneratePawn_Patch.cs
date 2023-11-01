using HarmonyLib;
using RimWorld;
using RimWorld.QuestGen;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(QuestNode_Root_WandererJoinAbasia), "GeneratePawn")]
    public static class QuestNode_Root_WandererJoinAbasia_GeneratePawn_Patch
    {
        public static void Prefix()
        {
            PawnGenerator_XenotypesAvailableFor_Patch.preventAndroidGeneration = true;
        }
        public static void Postfix()
        {
            PawnGenerator_XenotypesAvailableFor_Patch.preventAndroidGeneration = false;
        }
    }
}
