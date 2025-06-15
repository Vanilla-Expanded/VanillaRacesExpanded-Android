using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SkillUI), "GetSkillDescription")]
    public static class SkillUI_GetSkillDescription_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            foreach (var code in codes)
            {
                yield return code;
                if (code.opcode == OpCodes.Stloc_S && code.operand is LocalBuilder lb && lb.LocalIndex == 7)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 6);
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 7);
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(SkillRecord_Aptitude_Patch), nameof(SkillRecord_Aptitude_Patch.AdjustAptitude)));
                }
            }
        }
    }
}
