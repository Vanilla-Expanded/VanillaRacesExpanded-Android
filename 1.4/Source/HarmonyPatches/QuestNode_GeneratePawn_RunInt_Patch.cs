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
    [HarmonyPatch(typeof(QuestNode_GeneratePawn), "RunInt")]
    public static class QuestNode_GeneratePawn_RunInt_Patch
    {
        public static void Prefix()
        {
            Slate slate = QuestGen.slate;
            if (slate.Get<bool>("lodgersAreParalyzed") || slate.Get<bool>("lodgersHaveBloodRot") || slate.Get<bool>("lodgersHaveBloodRotAndParalysis"))
            {
                PawnGenerator_XenotypesAvailableFor_Patch.preventAndroidGeneration = true;
            }
        }

        public static void Postfix()
        {
            PawnGenerator_XenotypesAvailableFor_Patch.preventAndroidGeneration = false;
        }
    }
}
