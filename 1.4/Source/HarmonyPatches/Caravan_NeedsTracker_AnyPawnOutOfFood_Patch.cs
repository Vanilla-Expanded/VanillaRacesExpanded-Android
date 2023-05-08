using HarmonyLib;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch(typeof(Caravan_NeedsTracker), "AnyPawnOutOfFood")]
    public static class Caravan_NeedsTracker_AnyPawnOutOfFood_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var PawnsListForReadingInfo = AccessTools.PropertyGetter(typeof(Caravan), nameof(Caravan.PawnsListForReading));
            bool patched = false;
            foreach (var code in codes)
            {
                yield return code;
                if (code.Calls(PawnsListForReadingInfo))
                {
                    patched = true;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Caravan_NeedsTracker_AnyPawnOutOfFood_Patch), "NoAndroids"));
                }
            }
            if (!patched)
            {
                Log.Error("VREAndroids failed to patch Xenogerm_GetGizmos");
            }
        }

        public static List<Pawn> NoAndroids(List<Pawn> pawns)
        {
            return pawns.Where(x => x.IsAndroid() is false).ToList();
        }
    }
}
