using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(WealthWatcher), "ForceRecount")]
    public static class WealthWatcher_ForceRecount_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var check = AccessTools.Method(typeof(WealthWatcher_ForceRecount_Patch), "HasPresenceFirewall");
            var isQuestLodgerInfo = AccessTools.Method(typeof(QuestUtility), "IsQuestLodger");
            for (var i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (i > 0 && codes[i].opcode == OpCodes.Brtrue_S && codes[i - 1].Calls(isQuestLodgerInfo))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 4);
                    yield return new CodeInstruction(OpCodes.Call, check);
                    yield return new CodeInstruction(OpCodes.Brtrue_S, codes[i].operand);
                }
            }
        }

        public static bool HasPresenceFirewall(Pawn pawn)
        {
            return pawn.HasActiveGene(VREA_DefOf.VREA_PresenceFirewall);
        }
    }
}
