using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Dialog_CreateXenotype), "DrawSearchRect")]
    public static class Dialog_CreateXenotype_DrawSearchRect_Patch
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
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Dialog_CreateXenotype_DrawSearchRect_Patch),
                        nameof(FilterNonAndroidTypes)));
                }
            }
        }

        public static IEnumerable<XenotypeDef> FilterNonAndroidTypes(IEnumerable<XenotypeDef> list)
        {
            return list.Where(x => x.IsAndroidType() is false);
        }
    }
}
