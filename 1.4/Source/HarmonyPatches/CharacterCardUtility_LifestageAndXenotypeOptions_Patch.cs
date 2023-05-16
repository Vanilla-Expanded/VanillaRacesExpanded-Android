using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    [HarmonyPatch(typeof(CharacterCardUtility), "LifestageAndXenotypeOptions")]
    public static class CharacterCardUtility_LifestageAndXenotypeOptions_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            var get_AllDefsInfo = AccessTools.PropertyGetter(typeof(DefDatabase<XenotypeDef>), "AllDefs");
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (code.opcode == OpCodes.Stloc_S && code.operand is LocalBuilder lb && lb.LocalIndex == 15)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 15);
                    yield return new CodeInstruction(OpCodes.Ldarg_S, 4);
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(typeof(CharacterCardUtility_LifestageAndXenotypeOptions_Patch),
                        nameof(AddAndroidEditorOption)));
                }

                if (code.Calls(get_AllDefsInfo))
                {
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CharacterCardUtility_LifestageAndXenotypeOptions_Patch),
                        nameof(FilterAwakenedAndroidTypes)));
                }
            }
        }

        public static IEnumerable<XenotypeDef> FilterAwakenedAndroidTypes(IEnumerable<XenotypeDef> list)
        {
            return list.Where(x => x.IsAndroidType() is false || x.genes.Contains(VREA_DefOf.VREA_PsychologyDisabled));
        }

        public static void AddAndroidEditorOption(Pawn pawn, List<FloatMenuOption> list, Action randomizeCallback)
        {
            list.Add(new FloatMenuOption("VREA.AndroidEditor".Translate() + "...", delegate
            {
                Find.WindowStack.Add(new Window_CreateAndroidXenotype(StartingPawnUtility.PawnIndex(pawn), delegate
                {
                    CharacterCardUtility.cachedCustomXenotypes = null;
                    randomizeCallback();
                }));
            }));
        }
    }
}
