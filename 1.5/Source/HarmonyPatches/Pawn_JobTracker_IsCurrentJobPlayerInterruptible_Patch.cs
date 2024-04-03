using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using Verse.AI;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Pawn_JobTracker), "IsCurrentJobPlayerInterruptible")]
    public static class Pawn_JobTracker_IsCurrentJobPlayerInterruptible_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var check = AccessTools.Method(typeof(Pawn_JobTracker_IsCurrentJobPlayerInterruptible_Patch), "IsFireVulnerable");
            var hasAttachment = AccessTools.Method(typeof(AttachmentUtility), "HasAttachment");
            for (var i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (i > 0 && codes[i].opcode == OpCodes.Brfalse_S && codes[i - 1].Calls(hasAttachment))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, check);
                    yield return new CodeInstruction(OpCodes.Brfalse_S, codes[i].operand);
                }
            }
        }

        public static bool IsFireVulnerable(Pawn_JobTracker jobTracker)
        {
            return jobTracker.pawn.HasActiveGene(VREA_DefOf.VREA_FireVulnerability);
        }
    }
}
