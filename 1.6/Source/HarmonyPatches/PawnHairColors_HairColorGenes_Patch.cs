using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnHairColors), "HairColorGenes", MethodType.Getter)]
    public static class PawnHairColors_HairColorGenes_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var get_AllDefsInfo = AccessTools.PropertyGetter(typeof(DefDatabase<GeneDef>), "AllDefs");
            foreach (var code in codes)
            {
                yield return code;
                if (code.Calls(get_AllDefsInfo))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PawnSkinColors_SkinColorGenesInOrder_Patch),
                        nameof(NonAndroidGenesOnly)));
                }
            }
        }

        public static IEnumerable<GeneDef> NonAndroidGenesOnly(IEnumerable<GeneDef> list)
        {
            return list.Where(x => x.IsAndroidGene() is false);
        }
    }
}
