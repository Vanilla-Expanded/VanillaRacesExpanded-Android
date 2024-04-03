using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(PawnGenerator), "AdjustXenotypeForFactionlessPawn")]
    public static class PawnGenerator_AdjustXenotypeForFactionlessPawn_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var get_AllDefsInfo = AccessTools.PropertyGetter(typeof(DefDatabase<XenotypeDef>), "AllDefs");
            foreach (var code in codes)
            {
                yield return code;
                if (code.Calls(get_AllDefsInfo))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(PawnGenerator_AdjustXenotypeForFactionlessPawn_Patch),
                        nameof(FilterAndroidXenotypes)));
                }
            }
        }

        public static IEnumerable<XenotypeDef> FilterAndroidXenotypes(IEnumerable<XenotypeDef> list)
        {
            return list.Where(x => x.ShouldExcludeForAndroid() is false);
        }
    }
}
