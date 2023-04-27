using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace VREAndroids
{
    [HotSwappable]
    [HarmonyPatch(typeof(Page_ConfigureStartingPawns), "DrawXenotypeEditorButton")]
    public static class Page_ConfigureStartingPawns_DrawXenotypeEditorButton_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> codeInstructions)
        {
            var codes = codeInstructions.ToList();
            for (var i = 0; i < codes.Count; i++)
            {
                var code = codes[i];
                yield return code;
                if (code.opcode == OpCodes.Stloc_1)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldloca_S, 0);
                    yield return new CodeInstruction(OpCodes.Ldloc_1);
                    yield return new CodeInstruction(OpCodes.Call, 
                        AccessTools.Method(typeof(Page_ConfigureStartingPawns_DrawXenotypeEditorButton_Patch), 
                        nameof(AddAndroidEditorButton)));
                }
            }
        }

        public static void AddAndroidEditorButton(Page_ConfigureStartingPawns __instance, ref float x, float y)
        {
            x -= Page.BottomButSize.x / 2f;
            if (Widgets.ButtonText(new Rect(x, y, Page.BottomButSize.x, Page.BottomButSize.y), "VREA.AndroidEditor".Translate()))
            {
                Find.WindowStack.Add(new Dialog_CreateAndroid(StartingPawnUtility.PawnIndex(__instance.curPawn), delegate
                {
                    CharacterCardUtility.cachedCustomXenotypes = null;
                    __instance.RandomizeCurPawn();
                }));
            }
            x += Page.BottomButSize.x + 15;
        }
    }
}
