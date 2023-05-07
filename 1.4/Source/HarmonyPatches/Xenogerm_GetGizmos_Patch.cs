using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace VREAndroids
{
    [HarmonyPatch]
    public static class Xenogerm_GetGizmos_Patch
    {
        public static MethodBase TargetMethod()
        {
            return AccessTools.Method("RimWorld.Xenogerm:<GetGizmos>b__16_0");
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var AllPawnsSpawnedInfo = AccessTools.PropertyGetter(typeof(MapPawns), nameof(MapPawns.AllPawnsSpawned));
            bool patched = false;
            foreach ( var code in codes)
            {
                yield return code;
                if (code.Calls(AllPawnsSpawnedInfo))
                {
                    patched = true;
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Xenogerm_GetGizmos_Patch), "NoAndroids"));
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
