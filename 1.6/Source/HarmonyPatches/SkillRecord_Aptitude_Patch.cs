using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(SkillRecord), "Aptitude", MethodType.Getter)]
    public static class SkillRecord_Aptitude_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            foreach (var code in codes)
            {
                yield return code;
                if (code.opcode == OpCodes.Stloc_3)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 3);
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(SkillRecord_Aptitude_Patch), nameof(AdjustAptitude)));
                }
            }
        }

        public static void AdjustAptitude(Gene gene, ref int aptidude)
        {
            if (gene is Gene_MemoryDecay memoryDecayGene)
            {
                aptidude += memoryDecayGene.skillDecayOffset;
            }
        }
    }
}
